using System;
using System.Collections.Generic;
using Shootball.Model.Robot;

namespace Shootball.Model.Player
{
    public interface IPlayer
    {
        void OnUpdate();
        void SetPlayersList(IList<IPlayer> playersList);
    }

    public abstract class PlayerModel<T> : IPlayer where T : RobotModel
    {
        public readonly T Robot;
        protected IList<IPlayer> PlayersList { get; private set; }

        public PlayerModel(T robot)
        {
            if (robot == null)
                throw new ArgumentNullException();

            Robot = robot;
        }

        public void SetPlayersList(IList<IPlayer> playersList)
        {
            PlayersList = playersList;
        }

        public abstract void OnUpdate();
    }
}