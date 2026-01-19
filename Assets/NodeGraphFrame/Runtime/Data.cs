using System.Collections.Generic;


// 端口连接
public class LinkData
{
    public string fromNode; // 输出节点 UUID
    public string fromPort; // 输出端口名
    public string toNode;   // 输入节点 UUID
    public string toPort;   // 输入端口名
}

// JSON 序列化的节点数据
public class NodeData
{
    public string id; // UUID
    public string nodeType;
    public string type; // EventNode / FlowNode / DataNode
    public Dictionary<string, object> config = new Dictionary<string, object>();
}

// JSON 序列化整体 Graph
public class GraphData
{
    public List<NodeData> nodes = new List<NodeData>();
    public List<LinkData> links = new List<LinkData>();
    public Dictionary<int, string> eventIndex = new Dictionary<int, string>(); // EventID -> Node UUID
}

// 运行时上下文
public class RuntimeContext
{
    public int EventID;
    public object UserData;
}

public class FrameDefine
{

}
