using UnityEngine;
using XNode;

namespace NodeGraphFrame.Editor
{
    [CreateNodeMenu("Flow/TestFlowNode")]
    public class TestFlowNode : FlowNode
    {

        [Input] public int AInt;
        [Output] public string AString;

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}


