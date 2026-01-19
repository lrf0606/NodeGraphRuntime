using XNode;
using UnityEngine;
using System.Collections.Generic;

#region FlowPort 定义
// 用于标识流程端口类型
[System.Serializable]
public class FlowPort { }
#endregion


#region EventNode
[CreateNodeMenu("Workflow/EventNode")]
public class EventNode : Node
{
    [Output] public FlowPort flowOut;

    [Tooltip("全局唯一 EventID")]
    public int eventID;

    public override object GetValue(NodePort port) => null;
}
#endregion


#region FlowNode
public abstract class FlowNode : Node
{
    [Input] public FlowPort flowIn;
    [Output] public FlowPort flowOut;

    // 数据端口示例
    [Input] public Dictionary<string, object> dataInputs = new();
    [Output] public Dictionary<string, object> dataOutputs = new();

    /// <summary>
    /// 节点执行逻辑
    /// </summary>
    /// <param name="context">运行时上下文</param>
    public abstract void Execute(RuntimeContext context);

    public override object GetValue(NodePort port)
    {
        if (port.ValueType == typeof(FlowPort))
            return null;
        if (dataOutputs.ContainsKey(port.fieldName))
            return dataOutputs[port.fieldName];
        return null;
    }
}
#endregion


#region DataNode
public abstract class DataNode : Node
{
    [Input] public Dictionary<string, object> dataInputs = new();
    [Output] public Dictionary<string, object> dataOutputs = new();

    public abstract void Execute(RuntimeContext context);

    public override object GetValue(NodePort port)
    {
        if (dataOutputs.ContainsKey(port.fieldName))
            return dataOutputs[port.fieldName];
        return null;
    }
}
#endregion
