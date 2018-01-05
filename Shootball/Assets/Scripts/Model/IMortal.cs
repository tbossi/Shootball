using System;

namespace Shootball.Model
{
    public interface IMortal
    {
        void Die();
        void SetOnDeathListener(Action callback);
    }
}