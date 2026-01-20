// USER LOGIC FILE - SAFE TO EDIT

using System.Collections.Generic;
using UnityEngine;

namespace NodeGraphFrame.Runtime
{
    public class IfNode : IfNodeBase
    {
        public override void Execute(RuntimeGraph graph, RuntimeContext context, HashSet<string> executed)
        {
            // Implement logic here
            Debug.Log($"IfNode Execute:Condition={Condition}");
        }

        public override RuntimeNode GetNextExecuteNode(RuntimeGraph graph)
        {
            if (Condition)
            {
                return graph.GetNode(OutputLinks[0].ToNode);
            }
            else
            {
                return graph.GetNode(OutputLinks[1].ToNode);
            }
        }
    }
}
