using System.Collections.Generic;
using Shootball.Extensions;
using Shootball.Model.Behavior;
using Shootball.Model.Robot;
using Shootball.Utility.Navigation;
using UnityEngine;

namespace Shootball.Model.Player.AI
{
    public abstract class AIActions
    {
        protected readonly EnemyRobotModel RobotModel;
        private readonly Graph<Vector3> _navGraph;
        protected readonly IList<IPlayer> PlayersList;

        private AIPath _aiPath;
        protected AIPath AIPath => _aiPath ?? (_aiPath = new AIPath(_navGraph));

        public List<RobotModel> VisibleEnemies
        {
            get
            {
                var camera = RobotModel.Components.HeadCamera.GetComponent<Camera>();
                var enemies = new List<RobotModel>();
                foreach (var player in PlayersList)
                {
                    RobotModel robot;
                    if (player.GetType() == typeof(AIPlayerModel))
                    {
                        robot = ((AIPlayerModel)player).Robot;
                    }
                    else
                    {
                        robot = ((LocalPlayerModel)player).Robot;
                    }

                    if (!robot.Statistics.IsAlive)
                        continue;

                    if (camera.CanSee(robot.Components.RobotPosition.position))
                    {
                        enemies.Add(robot);
                    }
                }
                return enemies;
            }
        }

        public RobotModel NearestEnemy
        {
            get
            {
                var enemies = VisibleEnemies;

                if (enemies.Count == 0)
                {
                    return null;
                }

                var position = RobotModel.Components.RobotPosition.position;
                RobotModel nearestEnemy = null;
                float distance = float.PositiveInfinity;
                foreach (var enemy in enemies)
                {
                    var newDistance = Vector3.Distance(position, enemy.Components.RobotPosition.position);
                    if (distance > newDistance)
                    {
                        distance = newDistance;
                        nearestEnemy = enemy;
                    }
                }
                return nearestEnemy;
            }
        }

        public AIActions(EnemyRobotModel robotModel, Graph<Vector3> navGraph, IList<IPlayer> playersList)
        {
            RobotModel = robotModel;
            _navGraph = navGraph;
            PlayersList = playersList;
        }

        private RepeatedAction _slowDown;
        public BehaviorState SlowDown()
        {
            if (_slowDown == null) { _slowDown = new RepeatedAction(RobotModel.SlowDown, 5); }
            var result = _slowDown.RunOnce();
            if (result == BehaviorState.Complete) { _slowDown = null; }
            return result;
        }
        /*
        public BehaviorState CheckVisibleEnemies()
        {
            return NearestEnemy == null ? BehaviorState.Failed : BehaviorState.Complete;
        }

        public BehaviorState EnemyCanSeeMe()
        {
            var enemy = NearestEnemy;
            if (enemy == null)
                return BehaviorState.Failed;

            var camera = enemy.Components.HeadCamera.GetComponent<Camera>();
            if (camera.CanSee(RobotModel.Components.RobotPosition.position))
            {
                return BehaviorState.Complete;
            }
            return BehaviorState.Failed;
        }*/
    }
}