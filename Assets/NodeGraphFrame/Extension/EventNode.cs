using UnityEngine;
using XNode;

namespace NodeGraphFrame.Editor
{
    [CreateNodeMenu("Event/EventNode")]
    public class EventNode : Node
    {
        [Output] public FlowPort FlowOut;

        [Tooltip("全局唯一 EventID")]
        public int EventID;

        public override object GetValue(NodePort port) => null;
    }
}


