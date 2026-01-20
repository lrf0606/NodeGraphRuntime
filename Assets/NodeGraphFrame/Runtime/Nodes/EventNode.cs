// USER LOGIC FILE - SAFE TO EDIT

using System.Collections.Generic;
using UnityEngine;

namespace NodeGraphFrame.Runtime
{
    public class EventNode : EventNodeBase
    {
        public override void Execute(RuntimeGraph graph, RuntimeContext context, HashSet<string> executed)
        {
            // Implement logic here
        }

        public override RuntimeNode GetNextExecuteNode(RuntimeGraph graph)
        {
            return base.GetNextExecuteNode(graph);
        }
    }
}
