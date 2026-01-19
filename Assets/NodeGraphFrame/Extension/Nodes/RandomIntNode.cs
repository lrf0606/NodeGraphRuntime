using XNode;

namespace NodeGraphFrame.Editor
{
    [CreateNodeMenu("Data/RandomIntNode")]
    public class RandomIntNode : DataNode
    {
        [Output] public int RandomNum;
        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}

