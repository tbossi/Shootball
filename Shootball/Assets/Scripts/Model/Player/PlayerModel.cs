using System;
using System.Collections.Generic;
using Shootball.Model.Robot;

namespace Shootball.Model.Player
{
    public interface IPlayer
    {
        void OnUpdate();
        void SetMatchStatus(MatchStatusModel matchStatusModel);
    }

    public abstract class PlayerModel<T> : IPlayer where T : RobotModel
    {
        public readonly string Name;
        public readonly T Robot;
        protected MatchStatusModel MatchStatusModel { get; private set; }

        public PlayerModel(string name, T robot)
        {
            if (robot == null)
                throw new ArgumentNullException();

            Name = name;
            Robot = robot;
        }

        public void SetMatchStatus(MatchStatusModel matchStatusModel)
        {
            MatchStatusModel = matchStatusModel;
        }

        public abstract void OnUpdate();
    }
}