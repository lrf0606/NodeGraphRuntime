using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.IO;
using XNode;


namespace NodeGraphFrame.Editor
{

    public static class CodeGenerate
    {
        public const string GENERATED_FOLDER = "Assets/NodeGraphFrame/Runtime/Generated";
        public const string USER_FOLDER = "Assets/NodeGraphFrame/Runtime/Nodes";

        public class NodeClassInfo
        {
            public string ClassName;
            public List<NodeFieldInfo> Fields = new();
        }

        public class NodeFieldInfo
        {
            public string Name;
            public NodeFieldType Type;
            public bool IsInput;
            public bool IsOutput;
        }

        public enum NodeFieldType
        {
            Int,
            Float,
            Bool,
            String
        }

        [MenuItem("Tools/NodeGraphFrame/Generate RuntimeNode")]
        public static void GenerateAllRuntimeNode()
        {
            var nodeTypes = TypeCache.GetTypesDerivedFrom<Node>();
            foreach (var nodeType in nodeTypes)
            {
                if (nodeType == typeof(FlowNode) || nodeType == typeof(DataNode) || nodeType == typeof(LogicNode))
                {
                    continue;
                }
                var name = nodeType.Name;
                var nodeClasInfo = new NodeClassInfo();
                nodeClasInfo.ClassName = name;
                var fieldInfoList = new List<NodeFieldInfo>();

                var fields = nodeType.GetFields();
                foreach (var field in fields)
                {
                    // 必须为public、使用input output特性、非FlowPort流程端口
                    if (!field.IsPublic) 
                    {
                        continue;
                    }
                    var inputAttribute = field.GetCustomAttribute<Node.InputAttribute>();
                    var outputAttribute = field.GetCustomAttribute<Node.OutputAttribute>();
                    
                    if (inputAttribute == null && outputAttribute == null)
                    {
                        continue;
                    }
                    if (field.FieldType == typeof(FlowPort))
                    {
                        continue;
                    }
                    var nodeFieldInfo = new NodeFieldInfo();
                    nodeFieldInfo.Name = field.Name;
                    nodeFieldInfo.Type = InferFieldType(field.FieldType);
                    if (inputAttribute != null)
                    {
                        nodeFieldInfo.IsInput = true;
                    }
                    else if (outputAttribute != null)
                    {
                        nodeFieldInfo.IsOutput = true;
                    }
                    fieldInfoList.Add(nodeFieldInfo);
                }
                nodeClasInfo.Fields = fieldInfoList;

                GenerateBaseClass(nodeClasInfo);
                GenerateUserClassIfNotExists(nodeClasInfo);
                AssetDatabase.Refresh();
                Debug.Log($"NodeGraphFrame RuntimeNode Generate Sucess.");
            }
        }

        private static void GenerateByBaseType<T>()
        {

        }
        private static void GenerateBaseClass(NodeClassInfo node)
        {
            var directory = GENERATED_FOLDER;
            string path = Path.Combine(directory, $"{node.ClassName}Base.gen.cs");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
           
            var sb = new StringBuilder();
            sb.AppendLine("// AUTO-GENERATED FILE - DO NOT MODIFY");
            sb.AppendLine("using System;");
            sb.AppendLine();
            sb.AppendLine("namespace NodeGraphFrame.Runtime");
            sb.AppendLine("{");
            sb.AppendLine($"    public abstract class {node.ClassName}Base : RuntimeNode");
            sb.AppendLine("    {");
            foreach (var f in node.Fields)
            {
                sb.AppendLine($"        public {ToCSharpType(f.Type)} {f.Name};");
            }
            sb.AppendLine();
            foreach (var f in node.Fields.Where(f => f.IsInput))
            {
                sb.AppendLine($"        protected void Set_{f.Name}(object value) => {f.Name} = {ConvertExpression(f.Type, "value")};");
            }
            sb.AppendLine();
            foreach (var f in node.Fields.Where(f => f.IsOutput))
            {
                sb.AppendLine($"        protected object Get_{f.Name}() => {f.Name};");
            }
            sb.AppendLine();
            sb.AppendLine("        protected override void RegisterPorts()");
            sb.AppendLine("        {");
            foreach (var f in node.Fields)
            {
                if (f.IsInput) sb.AppendLine($"            RegisterInput(\"{f.Name}\", Set_{f.Name});");
                if (f.IsOutput) sb.AppendLine($"            RegisterOutput(\"{f.Name}\", Get_{f.Name});");
            }
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(path, sb.ToString());
        }

        private static void GenerateUserClassIfNotExists(NodeClassInfo node)
        {
            var directory = USER_FOLDER;
            string path = Path.Combine(directory, $"{node.ClassName}.cs");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (File.Exists(path))
            {
                return;
            }
          
            var sb = new StringBuilder();
            sb.AppendLine("// USER LOGIC FILE - SAFE TO EDIT");
            sb.AppendLine();
            sb.AppendLine("namespace NodeGraphFrame.Runtime");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {node.ClassName} : {node.ClassName}Base");
            sb.AppendLine("    {");
            sb.AppendLine("        public override void Execute(RuntimeGraph graph, RuntimeContext context, HashSet<string> executed)");
            sb.AppendLine("        {");
            sb.AppendLine("            // Implement logic here");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public override RuntimeNode GetNextExecuteNode(RuntimeGraph graph)");
            sb.AppendLine("        {");
            sb.AppendLine("            return base.GetNextExecuteNode(graph);");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");

            File.WriteAllText(path, sb.ToString());
        }

        private static string ConvertExpression(NodeFieldType type, string value)
        {
            return type switch
            {
                NodeFieldType.Int => $"Convert.ToInt32({value})",
                NodeFieldType.Float => $"Convert.ToSingle({value})",
                NodeFieldType.Bool => $"Convert.ToBoolean({value})",
                NodeFieldType.String => $"{value}?.ToString()",
                _ => value
            };
        }

        private static string ToCSharpType(NodeFieldType type)
        {
            return type switch
            {
                NodeFieldType.Int => "int",
                NodeFieldType.Float => "float",
                NodeFieldType.Bool => "bool",
                NodeFieldType.String => "string",
                _ => "object"
            };
        }

        private static NodeFieldType InferFieldType(Type t)
        {
            if (t == typeof(int)) return NodeFieldType.Int;
            if (t == typeof(float)) return NodeFieldType.Float;
            if (t == typeof(bool)) return NodeFieldType.Bool;
            if (t == typeof(string)) return NodeFieldType.String;

            throw new Exception($"Unsupported port type {t.Name}");
        }
    }
}


