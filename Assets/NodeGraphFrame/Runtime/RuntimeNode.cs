using System;
using System.Collections.Generic;
using UnityEngine;


namespace NodeGraphFrame.Runtime
{
    public abstract class RuntimeNode
    {
        public string UUID;
        // 节点连接数据
        public List<LinkData> InputLinks = new();
        public List<LinkData> OutputLinks = new();
        // 节点数据绑定
        private Dictionary<string, Action<object>> m_InputBindings = new();
        private Dictionary<string, Func<object>> m_OutputBindings = new();
        public IReadOnlyDictionary<string, Action<object>> InputBindings => m_InputBindings;
        public IReadOnlyDictionary<string, Func<object>> OutputBindings => m_OutputBindings;
        protected void RegisterInput(string name, Action<object> setter) => m_InputBindings[name] = setter;
        protected void RegisterOutput(string name, Func<object> getter) => m_OutputBindings[name] = getter;
        protected abstract void RegisterPorts();
        // 子类实现Execute逻辑
        public abstract void Execute(RuntimeContext context);

        public RuntimeNode()
        {
            // 注册
            RegisterPorts();
        }

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
}

