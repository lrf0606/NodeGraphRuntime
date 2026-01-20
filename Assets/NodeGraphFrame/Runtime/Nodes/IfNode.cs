// USER LOGIC FILE - SAFE TO EDIT

using UnityEngine;

namespace NodeGraphFrame.Runtime
{
    public class IfNode : IfNodeBase
    {
        public override void Execute(RuntimeContext context)
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
