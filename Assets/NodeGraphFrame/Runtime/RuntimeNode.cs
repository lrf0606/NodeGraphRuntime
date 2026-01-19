using System;
using System.Collections.Generic;
using UnityEngine;


namespace NodeGraphFrame.Runtime
{

    public class PortBinding
    {
        public Action<RuntimeNode, object> Setter;
        public Func<RuntimeNode, object> Getter;
    }

    public abstract class RuntimeNode
    {
        public string UUID;
        public string NodeType;

        public Dictionary<string, object> Config = new();

        public List<LinkData> InputLinks = new();
        public List<LinkData> OutputLinks = new();

        public Dictionary<string, PortBinding> InputBindings = new();
        public Dictionary<string, PortBinding> OutputBindings = new();


        public abstract void Execute(RuntimeContext context);

        public virtual RuntimeNode GetNextExecuteNode(RuntimeGraph graph)
        {
            foreach(var linkData in OutputLinks)
            {
                if (graph.IsFlowPort(linkData.FromPort))
                {
                    var nextNode = graph.GetNode(linkData.ToNode);
                    if (nextNode != null)
                    {
                        return nextNode;
                    }
                }
            }
            return null;
        }
    }

    public class EventNode : RuntimeNode
    {
        public override void Execute(RuntimeContext context)
        {
            // 事件节点没有任何功能
        }
    }

    public class RandomIntNode : RuntimeNode
    {
        public int RandomNum;
        public override void Execute(RuntimeContext context)
        {
            RandomNum = UnityEngine.Random.Range(-100, 100);
            Debug.Log($"RandomIntNode Execute RandomNum={RandomNum}");
        }
    }

    public class TestFlowNode : RuntimeNode
    {
        public int AInt = 3;
        public string AString;
        public override void Execute(RuntimeContext context)
        {
            Debug.Log($"TestFlowNode Execute AInt={AInt}");
        }
    }

    public class LogNode : RuntimeNode
    {
        public string LogString;
        public override void Execute(RuntimeContext context)
        {
            Debug.Log($"LogNode:LogString={LogString}");
        }
    }

    public class IfNode : RuntimeNode
    {
        public bool Condition;
        public override void Execute(RuntimeContext context)
        {
            
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

    public class ForLoopNode : RuntimeNode
    {
        public int Start;
        public int End;
        public int Step;

        public int Index;

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

