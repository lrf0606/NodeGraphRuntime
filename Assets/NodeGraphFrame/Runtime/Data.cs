using System.Collections.Generic;

namespace NodeGraphFrame.Runtime
{
    public class GraphData
    {
        public List<NodeData> Nodes = new();
        public List<LinkData> Links = new();
    }

    public class NodeData
    {
        public string UUID; // UUID
        public string NodeClass;
        public string NodeType; // EventNode / FlowNode / DataNode
        public Dictionary<string, object> Config = new();
    }

    public class LinkData
    {
        public string FromNode; // 输出节点 UUID
        public string FromPort; // 输出端口名
        public string ToNode;   // 输入节点 UUID
        public string ToPort;   // 输入端口名
    }


    // 运行时上下文
    public class RuntimeContext
    {
        public int EventID;
        public object UserData;
    }
}

