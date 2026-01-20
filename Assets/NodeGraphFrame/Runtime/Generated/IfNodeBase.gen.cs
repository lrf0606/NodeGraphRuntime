// AUTO-GENERATED FILE - DO NOT MODIFY
using System;

namespace NodeGraphFrame.Runtime
{
    public abstract class IfNodeBase : RuntimeNode
    {
        public bool Condition;

        protected void Set_Condition(object value) => Condition = Convert.ToBoolean(value);


        protected override void RegisterPorts()
        {
            RegisterInput("Condition", Set_Condition);
        }
    }
}
