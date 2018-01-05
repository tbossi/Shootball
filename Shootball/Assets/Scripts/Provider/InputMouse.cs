using System;

namespace Shootball.Provider
{
    public class InputMouse : Input<float>
    {
        private readonly string _axis;
        private readonly bool _rawValue;

        public float Value
        {
            get
            {
                if (_rawValue)
                    return UnityEngine.Input.GetAxisRaw(_axis);
                else
                    return UnityEngine.Input.GetAxis(_axis);                
            }
        }

        public InputMouse(string axis, bool rawValue)
        {
            _axis = axis;
            _rawValue = rawValue;
        }
    }
}