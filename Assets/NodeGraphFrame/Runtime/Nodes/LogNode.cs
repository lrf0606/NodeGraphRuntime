// USER LOGIC FILE - SAFE TO EDIT
using UnityEngine;

namespace NodeGraphFrame.Runtime
{
    public class LogNode : LogNodeBase
    {
        public override void Execute(RuntimeContext context)
        {
            // Implement logic here
            Debug.Log($"LogNode Execute:LogString={LogString}");
        }

        public override RuntimeNode GetNextExecuteNode(RuntimeGraph graph)
        {
            return base.GetNextExecuteNode(graph);
        }
    }
}
