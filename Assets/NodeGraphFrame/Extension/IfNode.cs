using UnityEngine;
using XNode;


namespace NodeGraphFrame.Editor
{
    [CreateNodeMenu("Logic/If")]
    public class IfNode : LogicNode
    {
        [Input] public FlowPort FlowIn;
        [Input] public bool Condition;

        [Output] public FlowPort TrueOut;
        [Output] public FlowPort FalseOut;

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}

