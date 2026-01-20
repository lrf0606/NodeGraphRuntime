// USER LOGIC FILE - SAFE TO EDIT
using System.Collections.Generic;
using UnityEngine;


namespace NodeGraphFrame.Runtime
{
    public class RandomIntNode : RandomIntNodeBase
    {
        public override void Execute(RuntimeGraph graph, RuntimeContext context, HashSet<string> executed)
        {
            // Implement logic here
            RandomNum = Random.Range(-100, 100);
            Debug.Log($"RandomIntNode Execute:RandomNum={RandomNum}");
        }

        public override RuntimeNode GetNextExecuteNode(RuntimeGraph graph)
        {
            return base.GetNextExecuteNode(graph);
        }
    }
}
