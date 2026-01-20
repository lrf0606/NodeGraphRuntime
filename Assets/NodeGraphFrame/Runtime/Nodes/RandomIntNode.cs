// USER LOGIC FILE - SAFE TO EDIT
using UnityEngine;


namespace NodeGraphFrame.Runtime
{
    public class RandomIntNode : RandomIntNodeBase
    {
        public override void Execute(RuntimeContext context)
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
