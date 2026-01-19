using System;
using UnityEngine;

// EventNode
public class RuntimeEventNode : RuntimeNode
{
    public override void Execute(RuntimeContext context)
    {
        Debug.Log($"[EventNode] Execute EventID={context.EventID}");
    }
}

// 数据节点 RandomDataNode
public class RuntimeRandomDataNode : RuntimeNode
{
    public override void Execute(RuntimeContext context)
    {
        int min = config.ContainsKey("min") ? Convert.ToInt32(config["min"]) : 0;
        int max = config.ContainsKey("max") ? Convert.ToInt32(config["max"]) : 100;
        var r = new System.Random();
        int val = r.Next(min, max + 1);
        dataOutputs["value"] = val;
        Debug.Log($"[RandomDataNode] Generate {val}");
    }
}

// 流程节点 MultiplyToStringNode
public class RuntimeMultiplyToStringNode : RuntimeNode
{
    public override void Execute(RuntimeContext context)
    {
        if (dataOutputs.TryGetValue("input", out var obj) && obj is int val)
        {
            string result = (val * 2).ToString();
            dataOutputs["output"] = result;
            Debug.Log($"[MultiplyToStringNode] {val}*2 -> {result}");
        }
    }
}
