using UnityEngine;

namespace Shootball.Provider
{
    public class Inputs
    {
        public static Input MoveForward => new InputKey(KeyCode.W, KeyPression.Pressed);
        public static Input MoveBackward => new InputKey(KeyCode.S, KeyPression.Pressed);
        public static Input MoveLeft => new InputKey(KeyCode.A, KeyPression.Pressed);
        public static Input MoveRight => new InputKey(KeyCode.D, KeyPression.Pressed);
        public static Input RotateLeft => new InputAxis("Mouse X", value => value < 0);
        public static Input RotateRight => new InputAxis("Mouse X", value => value > 0);
        public static Input RotateUp => new InputAxis("Mouse Y", value => value > 0);        
        public static Input RotateDown => new InputAxis("Mouse Y", value => value < 0);
        public static Input Shoot => new InputKey(KeyCode.Space, KeyPression.Pressed);
        public static Input Target => new InputKey(KeyCode.LeftShift, KeyPression.Pressed);
        public static Input Pause => new InputKey(KeyCode.Escape, KeyPression.Down);        
    }
}