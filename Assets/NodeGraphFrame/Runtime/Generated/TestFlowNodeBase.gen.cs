// AUTO-GENERATED FILE - DO NOT MODIFY
using System;

namespace NodeGraphFrame.Runtime
{
    public abstract class TestFlowNodeBase : RuntimeNode
    {
        public int AInt;
        public string AString;

        protected void Set_AInt(object value) => AInt = Convert.ToInt32(value);

        protected object Get_AString() => AString;

        protected override void RegisterPorts()
        {
            RegisterInput("AInt", Set_AInt);
            RegisterOutput("AString", Get_AString);
        }
    }
}
