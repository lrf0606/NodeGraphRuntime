using XNode;

namespace NodeGraphFrame.Editor
{
    [CreateNodeMenu("Flow/Log")]
    public class LogNode : FlowNode
    {
        [Input] public string LogString;

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}
