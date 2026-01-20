// USER LOGIC FILE - SAFE TO EDIT
using System.Collections.Generic;

namespace NodeGraphFrame.Runtime
{
    public class ForLoopNode : ForLoopNodeBase
    {
        public override void Execute(RuntimeGraph graph, RuntimeContext context, HashSet<string> executed)
        {
            var preLoopNode = graph.GetNode(OutputLinks[1].ToNode);
            var saveExecuted = new HashSet<string>(executed);
            for (int i = Start; i <= End; i += Step)
            {
                Index = i;
                executed.IntersectWith(saveExecuted);
                graph.ExecuteNode(preLoopNode, context, executed);
            }
            
        }

        public override RuntimeNode GetNextExecuteNode(RuntimeGraph graph)
        {
            return graph.GetNode(OutputLinks[0].ToNode);
        }

 
    }
}
