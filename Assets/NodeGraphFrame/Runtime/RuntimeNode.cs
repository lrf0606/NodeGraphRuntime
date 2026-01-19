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
    }

    public class EventNode : RuntimeNode
    {
        public override void Execute(RuntimeContext context)
        {
            // 事件节点没有任何功能
            Debug.Log($"EventNode Execute {Config["EventID"]}");
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
}

