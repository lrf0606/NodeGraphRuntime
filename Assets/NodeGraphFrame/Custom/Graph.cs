using System.Linq;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

/// <summary>
/// Workflow Graph Editor
/// 校验拖线规则，EventID 唯一性
/// </summary>
[CustomNodeGraphEditor(typeof(ExtensionNodeGraph))]
public class WorkflowGraphEditor : NodeGraphEditor
{
    public override bool CanConnect(NodePort from, NodePort to)
    {
        if (!base.CanConnect(from, to)) return false;

        // FlowPort 类型校验
        bool outIsFlow = from.ValueType == typeof(FlowPort);
        bool inIsFlow = to.ValueType == typeof(FlowPort);

        if (outIsFlow || inIsFlow)
        {
            if (!(outIsFlow && inIsFlow)) return false;

            // EventNode 不能作为流程输入
            if (to.node is EventNode) return false;

            // DataNode 不参与 Flow
            if (from.node is DataNode || to.node is DataNode) return false;

            // Flow 单主干
            if (from.IsConnected) return false;
        }

        // DataPort 输入时只能连1个 输出时不限制
        if (!inIsFlow && to.IsConnected) return false;

        return true;
    }

    public override void OnGUI()
    {
        base.OnGUI();

        // 校验 EventID 唯一性
        var graph = target as NodeGraph;
        var eventNodes = graph.nodes.OfType<EventNode>();

        var duplicates = eventNodes
            .GroupBy(e => e.eventID)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);

        foreach (var id in duplicates)
        {
            EditorGUILayout.HelpBox(
                $"EventID重复，请检查: {id}",
                MessageType.Error
            );
        }
    }
}



[CreateAssetMenu(menuName = "NodeGraphFrame/Create NodeGraph")]
public class ExtensionNodeGraph : NodeGraph
{

}
