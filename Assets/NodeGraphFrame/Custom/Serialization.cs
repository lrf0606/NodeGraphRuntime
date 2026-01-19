using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using XNode;

public static class SerializationUtil
{

    [MenuItem("Assets/NodeGraphFrame 序列化", false, 101)]
    public static void RunSerialization()
    {
        var selectedAsset = Selection.activeObject;
        if (selectedAsset == null)
        {
            EditorUtility.DisplayDialog("提示", "请选中一个NodeGraph.asset", "确定");
            return;
        }
        var graph = selectedAsset as ExtensionNodeGraph;
        if (graph == null)
        {
            EditorUtility.DisplayDialog("提示", "请选中一个NodeGraph.asset", "确定");
            return;
        }
        string contents = SerializeGraph(graph);
        
        var fileName = $"{selectedAsset.name}.json";
        var Directory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedAsset));
        var filePath = Path.Combine(Directory, fileName );
        File.WriteAllText(filePath, contents);
        Debug.Log($"序列化完成 {filePath}");
        AssetDatabase.Refresh();
    }


    /// <summary>
    /// 序列化 xNode ExtensionNodeGraph 到 JSON
    /// </summary>
    public static string SerializeGraph(ExtensionNodeGraph graph)
    {
        var graphData = new GraphData();
        var nodeIdMap = new Dictionary<Node, string>();

        // 1️⃣ 遍历节点，生成唯一 UUID
        foreach (var node in graph.nodes)
        {
            string nodeId = Guid.NewGuid().ToString();
            nodeIdMap[node] = nodeId;

            var n = new NodeData
            {
                id = nodeId,
                nodeType = node.GetType().Name,
                type = node is EventNode ? "EventNode" :
                       node is FlowNode ? "FlowNode" :
                       node is DataNode ? "DataNode" : "Unknown",
                config = new Dictionary<string, object>()
            };

            // EventNode 特殊处理 eventID
            if (node is EventNode evNode)
            {
                n.config["eventID"] = evNode.eventID;
                if (!graphData.eventIndex.ContainsKey(evNode.eventID))
                    graphData.eventIndex[evNode.eventID] = nodeId;
            }

            // 序列化公共字段到 config
            foreach (var field in node.GetType().GetFields())
            {
                var val = field.GetValue(node);
                if (val == null) continue;

                Type type = val.GetType();
                if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type.IsSerializable)
                    n.config[field.Name] = val;
            }

            graphData.nodes.Add(n);
        }

        // 2️⃣ 序列化连接
        foreach (var node in graph.nodes)
        {
            foreach (var port in node.Ports)
            {
                foreach (var connection in port.GetConnections())
                {
                    graphData.links.Add(new LinkData
                    {
                        fromNode = nodeIdMap[port.node],
                        fromPort = port.fieldName,
                        toNode = nodeIdMap[connection.node],
                        toPort = connection.fieldName
                    });
                }
            }
        }

        // 3️⃣ 输出 JSON
        return JsonConvert.SerializeObject(graphData, Formatting.Indented);
    }
}
