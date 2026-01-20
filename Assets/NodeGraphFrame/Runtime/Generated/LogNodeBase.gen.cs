// AUTO-GENERATED FILE - DO NOT MODIFY
using System;

namespace NodeGraphFrame.Runtime
{
    public abstract class LogNodeBase : RuntimeNode
    {
        public string LogString;

        protected void Set_LogString(object value) => LogString = value?.ToString();


        protected override void RegisterPorts()
        {
            RegisterInput("LogString", Set_LogString);
        }
    }
}
