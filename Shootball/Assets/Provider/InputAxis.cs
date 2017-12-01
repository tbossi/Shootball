using System;

namespace Shootball.Provider
{
    public class InputAxis : Input
    {
        private readonly string axis;
        private readonly Func<float, bool> activationCondition;

        public virtual bool Active => activationCondition.Invoke(UnityEngine.Input.GetAxis(axis));

        public InputAxis(string axis, Func<float, bool> activationCondition)
        {
            this.axis = axis;
            this.activationCondition = activationCondition;
        }
    }
}