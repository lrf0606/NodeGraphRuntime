// USER LOGIC FILE - SAFE TO EDIT

using UnityEngine;

namespace NodeGraphFrame.Runtime
{
    public class ForLoopNode : ForLoopNodeBase
    {
        private int current;
        private bool initialized;
        public override void Execute(RuntimeContext context)
        {
            // 第一次进入 for 时初始化
            if (!initialized)
            {
                initialized = true;
                current = Start;
            }

            // 设置当前 index（供数据端口使用）
            Index = current;
        }

        public override RuntimeNode GetNextExecuteNode(RuntimeGraph graph)
        {
            var loopEndNode = graph.GetNode(OutputLinks[0].ToNode);
            var perLoopNode = graph.GetNode(OutputLinks[1].ToNode);
            // 判断是否还能继续循环
            bool canContinue = Step > 0 ? current < End : Step < 0 ? current > End : false;
            if (canContinue && perLoopNode != null)
            {
                current += Step;
                return perLoopNode;
            }
            // 循环结束，重置状态
            ResetLoopState();
            return loopEndNode;
        }

        private void ResetLoopState()
        {
            initialized = false;
            current = 0;
            Index = 0;
        }
    }
}
