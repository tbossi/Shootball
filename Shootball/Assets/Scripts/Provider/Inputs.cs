using UnityEngine;

namespace Shootball.Provider
{
    public class Inputs
    {
        public static InputKey MoveForward => new InputKey(KeyCode.W, KeyPression.Pressed);
        public static InputKey MoveBackward => new InputKey(KeyCode.S, KeyPression.Pressed);
        public static InputKey MoveLeft => new InputKey(KeyCode.A, KeyPression.Pressed);
        public static InputKey MoveRight => new InputKey(KeyCode.D, KeyPression.Pressed);
        public static InputMouse MouseX => new InputMouse("Mouse X", true);
        public static InputMouse MouseY => new InputMouse("Mouse Y", true);
        public static InputMouseKey Shoot => new InputMouseKey(0, KeyPression.Pressed);
        public static InputKey Target => new InputKey(KeyCode.Space, KeyPression.Pressed);
        public static InputKey Pause => new InputKey(KeyCode.P, KeyPression.Down);        
    }
}