using UnityEngine;
using XNode;

namespace NodeGraphFrame.Editor
{
    [CreateNodeMenu("Logic/ForLoop")]
    public class ForLoopNode : LogicNode
    {
        [Input] public FlowPort FlowIn;
        [Input] public int Start;
        [Input] public int End;
        [Input] public int Step;

        [Output] public FlowPort FlowOut;
        [Output] public FlowPort PerLoopOut;
        [Output] public int Index;

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
