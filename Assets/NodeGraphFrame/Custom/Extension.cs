

[CreateNodeMenu("Workflow/Data/RandomDataNode")]
public class RandomDataNode : DataNode
{
    [Output] public int randomNumber;

    public override void Execute(RuntimeContext context)
    {
        // Editor 侧不执行逻辑
    }
}


[CreateNodeMenu("Workflow/Flow/MultiplyToStringNode")]
public class MultiplyToStringNode : FlowNode
{
    [Input] public int inputValue;
    [Output] public string outputValue;

    public override void Execute(RuntimeContext context)
    {
        // Editor 侧不执行逻辑
    }
}
