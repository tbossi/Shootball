namespace Shootball.Provider
{
    public class InputMouseKey : Input<bool>
    {
        private readonly int _key;
        private readonly KeyPression _type;

        public bool Value 
        {
            get
            {
                switch(_type)
                {
                    case KeyPression.Up:
                        return UnityEngine.Input.GetMouseButtonUp(_key);
                    case KeyPression.Down:
                        return UnityEngine.Input.GetMouseButtonDown(_key);
                    case KeyPression.Pressed:
                        return UnityEngine.Input.GetMouseButton(_key);
                    default:
                        return false;
                }               
            }
        }

        public InputMouseKey(int key, KeyPression type)
        {
            _key = key;
            _type = type;
        }
    }  
}