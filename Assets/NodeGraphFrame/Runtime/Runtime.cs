using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class RuntimeNode
{
    public string id; // UUID
    public string nodeType;
    public Dictionary<string, object> config = new Dictionary<string, object>();

    // 执行后产生的数据输出
    public Dictionary<string, object> dataOutputs = new Dictionary<string, object>();

    // 所有输出连接
    public List<LinkData> outputLinks = new List<LinkData>();

    // 所有输入连接（为了数据依赖递归执行）
    public List<LinkData> inputLinks = new List<LinkData>();

    public virtual void Execute(RuntimeContext context)
    {
        // 子类重写
    }
}

public class RuntimeGraph
{
    public Dictionary<string, RuntimeNode> nodes = new Dictionary<string, RuntimeNode>();
    public Dictionary<int, string> eventIndex = new Dictionary<int, string>();

    /// <summary>
    /// 构建 RuntimeGraph
    /// </summary>
    public RuntimeGraph(string assetPath)
    {
        var contents = File.ReadAllText(assetPath);
        var graphData = JsonConvert.DeserializeObject<GraphData>(contents);

        // 1. 创建节点
        foreach (var nodeData in graphData.nodes)
        {
            RuntimeNode node = CreateRuntimeNode(nodeData);
            node.id = nodeData.id;
            node.config = nodeData.config;
            nodes[node.id] = node;

            if (nodeData.type == "EventNode" && nodeData.config.TryGetValue("eventID", out var eidObj))
            {
                int eventID = Convert.ToInt32(eidObj);
                if (eventIndex.ContainsKey(eventID))
                    throw new Exception($"Duplicate EventID {eventID}");
                eventIndex[eventID] = node.id;
            }
        }

        // 2.建立输入/输出连接
        foreach (var link in graphData.links)
        {
            if (!nodes.TryGetValue(link.fromNode, out var fromNode))
                throw new Exception($"FromNode {link.fromNode} not found");
            if (!nodes.TryGetValue(link.toNode, out var toNode))
                throw new Exception($"ToNode {link.toNode} not found");

            // 添加 outputLinks
            fromNode.outputLinks.Add(link);

            // 添加 inputLinks
            toNode.inputLinks.Add(link);
        }
    }

    private RuntimeNode CreateRuntimeNode(NodeData data)
    {
        switch (data.nodeType)
        {
            case "RandomDataNode": return new RuntimeRandomDataNode();
            case "MultiplyToStringNode": return new RuntimeMultiplyToStringNode();
            case "EventNode": return new RuntimeEventNode();
            default: return null;
        }
    }

    /// <summary>
    /// 通过 EventID 执行 Graph
    /// </summary>
    public void RunGraph(int eventID, object userData)
    {
        if (!eventIndex.TryGetValue(eventID, out var startId))
        {
            Console.WriteLine($"Event {eventID} not found");
            return;
        }
      
        var context = new RuntimeContext
        {
            EventID = eventID,
            UserData = userData
        };

        var executed = new HashSet<string>();
        ExecuteFlow(nodes[startId], context, executed);
    }

    private void ExecuteFlow(RuntimeNode current, RuntimeContext context, HashSet<string> executed)
    {
        if (current == null || executed.Contains(current.id)) return;
        executed.Add(current.id);

        // 1️⃣ 先执行数据依赖
        foreach (var link in current.inputLinks)
        {
            if (!nodes.TryGetValue(link.fromNode, out var sourceNode)) continue;
            // 执行所有非流程端口的数据依赖
            if (link.fromPort != "flowOut" && link.fromPort != "flowIn")
            {
                ExecuteFlow(sourceNode, context, executed);
            }
        }

        // 2️⃣ 执行当前节点
        current.Execute(context);

        // 3️⃣ 数据端口映射到下游节点
        foreach (var link in current.outputLinks)
        {
            if (!nodes.TryGetValue(link.toNode, out var targetNode)) continue;
            if (link.fromPort != "flowOut" && link.fromPort != "flowIn")
            {
                if (current.dataOutputs.TryGetValue(link.fromPort, out var value))
                {
                    targetNode.dataOutputs[link.toPort] = value;
                }
            }
        }

        // 4️⃣ 沿 flowOut 推进主干
        foreach (var link in current.outputLinks)
        {
            if (link.fromPort == "flowOut" && nodes.TryGetValue(link.toNode, out var nextNode))
            {
                ExecuteFlow(nextNode, context, executed);
            }
        }
    }
}
