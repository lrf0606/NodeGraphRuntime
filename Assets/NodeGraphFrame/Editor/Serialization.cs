using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using XNode;
using NodeGraphFrame.Runtime;


namespace NodeGraphFrame.Editor
{
    public static class SerializationUtil
    {

        [MenuItem("Assets/NodeGraphFrame 序列化", false)]
        public static void RunSerialization()
        {
            var selectedAsset = Selection.activeObject;
            if (selectedAsset == null)
            {
                EditorUtility.DisplayDialog("提示", "请选中一个NodeGraph.asset", "确定");
            }
            var graph = selectedAsset as NodeGraphEx;
            if (graph == null)
            {
                EditorUtility.DisplayDialog("提示", "请选中一个NodeGraph.asset", "确定");
            }
            string contents = SerializeGraph(graph);

            var fileName = $"{selectedAsset.name}.json";
            var Directory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(selectedAsset));
            var filePath = Path.Combine(Directory, fileName);
            File.WriteAllText(filePath, contents);
            Debug.Log($"序列化完成 {filePath}");
            AssetDatabase.Refresh();
        }

        public static string SerializeGraph(NodeGraphEx graph)
        {
            var graphData = new GraphData();
            var nodeIDMap = new Dictionary<Node, string>();

            // 1.保存节点数据
            foreach (var node in graph.nodes)
            {
                var nodeUUID = Guid.NewGuid().ToString();
                nodeIDMap[node] = nodeUUID;

                var nodeData = new NodeData
                {
                    UUID = nodeUUID,
                    NodeClass = node.GetType().Name,
                    NodeType = node is EventNode ? "EventNode" :
                           node is FlowNode ? "FlowNode" :
                           node is DataNode ? "DataNode" : "Unknown",
                };

                // EventNode 特殊处理 EventID
                if (node is EventNode eventNode)
                {
                    nodeData.Config["EventID"] = eventNode.EventID;
                }

                // 序列化公共字段到 config
                foreach (var field in node.GetType().GetFields())
                {
                    var val = field.GetValue(node);
                    if (val == null)
                    {
                        continue;
                    }

                    var type = val.GetType();
                    if (type.IsPrimitive || type == typeof(string) || type == typeof(decimal) || type.IsSerializable)
                    {
                        nodeData.Config[field.Name] = val;
                    }
                        
                }

                graphData.Nodes.Add(nodeData);
            }

            // 2.保存连线数据据
            foreach (var node in graph.nodes)
            {
                foreach (var port in node.Ports)
                {
                    if (!port.IsOutput) // 只保留输出端口连线即可
                    {
                        continue;
                    }
                    foreach (var connection in port.GetConnections())
                    {
                        graphData.Links.Add(new LinkData
                        {
                            FromNode = nodeIDMap[port.node],
                            FromPort = port.fieldName,
                            ToNode = nodeIDMap[connection.node],
                            ToPort = connection.fieldName
                        });
                    }
                }
            }

            // 3.序列化为json
            return JsonConvert.SerializeObject(graphData, Formatting.Indented);
        }
    }

}

