using System;
using Shootball.Model.Robot;

namespace Shootball.Model.Player
{
    public interface IPlayer
    {
        void OnUpdate();
    }

    public abstract class PlayerModel<T> : IPlayer where T : RobotModel
    {
        protected readonly T Robot;

        public PlayerModel(T robot)
        {
            if (robot == null)
                throw new ArgumentNullException();
                
            Robot = robot;
        }

        public abstract void OnUpdate();
    }
}