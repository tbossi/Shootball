using UnityEngine;

namespace Shootball.Provider
{
    public class InputKey : Input<bool>
    {
        private readonly KeyCode _key;
        private readonly KeyPression _type;

        public bool Value 
        {
            get
            {
                switch(_type)
                {
                    case KeyPression.Up:
                        return UnityEngine.Input.GetKeyUp(_key);
                    case KeyPression.Down:
                        return UnityEngine.Input.GetKeyDown(_key);
                    case KeyPression.Pressed:
                        return UnityEngine.Input.GetKey(_key);
                    default:
                        return false;
                }               
            }
        }

        public InputKey(KeyCode key, KeyPression type)
        {
            _key = key;
            _type = type;
        }
    }    
}