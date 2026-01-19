
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


namespace NodeGraphFrame.Runtime
{
    public class RuntimeGraph
    {
        private Dictionary<string, RuntimeNode> m_AllNodes = new();
        private Dictionary<int, RuntimeNode> m_EventNodes = new();

        public RuntimeGraph(string assetPath)
        {
            LoadAsset(assetPath);
        }

        public void LoadAsset(string assetPath)
        {
            var graphData = DeserializationUtil.DeserializeGraph(assetPath);
            // 1. 创建节点
            foreach(var nodeData in graphData.Nodes)
            {
                var node = CreateRuntimeNode(nodeData);
                node.UUID = nodeData.UUID;
                node.Config = nodeData.Config;
                m_AllNodes[node.UUID] = node;
                BulidPortBindings(node);
                if (nodeData.NodeClass == "EventNode")
                {
                    int eventId = Convert.ToInt32(nodeData.Config["EventID"]);
                    m_EventNodes[eventId] = node;
                }
            }
            // 2.连线
            foreach(var linkData in graphData.Links)
            {
                var fromNode = m_AllNodes[linkData.FromNode];
                var toNode = m_AllNodes[linkData.ToNode];
                fromNode.OutputLinks.Add(linkData);
                toNode.InputLinks.Add(linkData);
            }
        }

        /// <summary>
        /// 反射 + 委托缓存实现RuntimeNode的设置数据
        /// </summary>
        /// <param name="node"></param>
        private void BulidPortBindings(RuntimeNode node)
        {
            var type = node.GetType();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach(var field in fields)
            {
                var name = field.Name;
                node.InputBindings[name] = new PortBinding()
                {
                    Setter = (n, v) => field.SetValue(n, v)
                };
                node.OutputBindings[name] = new PortBinding()
                {
                    Getter = (n) => field.GetValue(n)
                };
            };
        }

        private RuntimeNode CreateRuntimeNode(NodeData data)
        {
            return data.NodeClass switch
            {
                "EventNode" => new EventNode(),
                "RandomIntNode" => new RandomIntNode(),
                "TestFlowNode" => new TestFlowNode(),
                _ => null,
            };
        }

        public void RunGraph(int eventId, object userData = null)
        {
            if (!m_EventNodes.TryGetValue(eventId, out var eventNode))
            {
                Debug.LogWarning($"RunGraph failed, {eventId} is eixst");
                return;
            }
            Debug.Log($"RunGraph {eventId}");
            var context = new RuntimeContext();
            context.EventID = eventId;
            context.UserData = userData;
            var executed = new HashSet<string>();
            ExecuteNode(eventNode, context, executed);
        }

        private void ExecuteNode(RuntimeNode currentNode, RuntimeContext context, HashSet<string> executed)
        {
            if (currentNode == null)
            {
                return;
            }
            if (executed.Contains(currentNode.UUID))
            {
                return;
            }
            // 1.先去执行之前的数据依赖
            ExecuteDataDependencies(currentNode, context, executed);
            // 2.执行当前节点
            currentNode.Execute(context);
            executed.Add(currentNode.UUID);
            // 3.流程推进
            ExecuteNextFlow(currentNode, context, executed);
        }

        private void ExecuteDataDependencies(RuntimeNode currentNode, RuntimeContext context, HashSet<string> executed)
        {
            foreach (var linkData in currentNode.InputLinks)
            {
                if (IsFlowPort(linkData.FromPort) || IsFlowPort(linkData.ToPort))
                {
                    continue;
                }
                var prevNode = m_AllNodes[linkData.FromNode];
                // 先执行节点
                ExecuteNode(prevNode, context, executed);
                // 更新数据
                var outBind = prevNode.OutputBindings[linkData.FromPort];
                var inBind = currentNode.InputBindings[linkData.ToPort];
                var value = outBind.Getter(prevNode);
                inBind.Setter(currentNode, value);
            }
        }
        private void ExecuteNextFlow(RuntimeNode currentNode, RuntimeContext context, HashSet<string> executed)
        {
  
            foreach(var linkData in currentNode.OutputLinks)
            {
                if (!IsFlowPort(linkData.FromPort))
                {
                    continue;
                }
                var nextNode = m_AllNodes[linkData.ToNode];
                ExecuteNode(nextNode, context, executed);
                break; // 规定一个线性主流程，不允许一个流程端口连接其他多个流程端口
            }
        }

        public bool IsFlowPort(string port)
        {
            return port == "FlowIn" || port == "FlowOut";
        }
    }
}
