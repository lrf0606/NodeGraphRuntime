// USER LOGIC FILE - SAFE TO EDIT

using UnityEngine;

namespace NodeGraphFrame.Runtime
{
    public class TestFlowNode : TestFlowNodeBase
    {
        public override void Execute(RuntimeContext context)
        {
            // TODO: Implement logic
            AString = $"==={AInt}===";
            Debug.Log($"TestFlowNode Execute:AInt={AInt},AString={AString}");

        }

        public override RuntimeNode GetNextExecuteNode(RuntimeGraph graph)
        {
            return base.GetNextExecuteNode(graph);
        }
    }
}
