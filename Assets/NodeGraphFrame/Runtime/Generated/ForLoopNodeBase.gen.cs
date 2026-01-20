// AUTO-GENERATED FILE - DO NOT MODIFY
using System;

namespace NodeGraphFrame.Runtime
{
    public abstract class ForLoopNodeBase : RuntimeNode
    {
        public int Start;
        public int End;
        public int Step;
        public int Index;

        protected void Set_Start(object value) => Start = Convert.ToInt32(value);
        protected void Set_End(object value) => End = Convert.ToInt32(value);
        protected void Set_Step(object value) => Step = Convert.ToInt32(value);

        protected object Get_Index() => Index;

        protected override void RegisterPorts()
        {
            RegisterInput("Start", Set_Start);
            RegisterInput("End", Set_End);
            RegisterInput("Step", Set_Step);
            RegisterOutput("Index", Get_Index);
        }
    }
}
