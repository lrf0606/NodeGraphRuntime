// AUTO-GENERATED FILE - DO NOT MODIFY
using System;

namespace NodeGraphFrame.Runtime
{
    public abstract class RandomIntNodeBase : RuntimeNode
    {
        public int RandomNum;


        protected object Get_RandomNum() => RandomNum;

        protected override void RegisterPorts()
        {
            RegisterOutput("RandomNum", Get_RandomNum);
        }
    }
}
