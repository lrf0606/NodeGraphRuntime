// USER LOGIC FILE - SAFE TO EDIT

using UnityEngine;

namespace NodeGraphFrame.Runtime
{
    public class EventNode : EventNodeBase
    {
        public override void Execute(RuntimeContext context)
        {
            // Implement logic here
        }

        public override RuntimeNode GetNextExecuteNode(RuntimeGraph graph)
        {
            return base.GetNextExecuteNode(graph);
        }
    }
}
