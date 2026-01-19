using XNode;

// 用于标识流程端口类型
[System.Serializable]
public class FlowPort { }

/// <summary>
/// 流程节点
/// </summary>
public abstract class FlowNode : Node
{
    [Input] public FlowPort FlowIn;
    [Output] public FlowPort FlowOut;
}


/// <summary>
/// 数据节点
/// </summary>
public abstract class DataNode : Node
{
}

