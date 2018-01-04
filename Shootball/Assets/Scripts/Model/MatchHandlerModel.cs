using System.Collections.Generic;
using Shootball.Model.Player;
using Shootball.Model.Robot;
using Shootball.Utility;
using UnityEngine;

namespace Shootball.Model
{
    public class MatchHandlerModel
    {
        private readonly GameObject _robotPrefab;
        private readonly MapModel _map;
        private List<IPlayer> _players;
        private GameObject _mapGameObject;
        private GameObject _playersGameObject;

        public MatchHandlerModel(GameObject robotPrefab, MapModel map)
        {
            _robotPrefab = robotPrefab;
            _map = map;
        }

        public void OnStart()
        {
            _mapGameObject = new GameObject("Map");
            _playersGameObject = new GameObject("Players");

            BeginMatch();
        }

        public void OnUpdate()
        {
            if (_players != null)
            {
                _players.ForEach(player => player.OnUpdate());
            }
        }

        private void BeginMatch()
        {
            _map.Instantiate(_mapGameObject);

            _players = new List<IPlayer>();
            var player = SpawnRobot(true).GetComponent<GlobalScripts.Robot>();
            _players.Add(new LocalPlayerModel((PlayerRobotModel)player.RobotModel));

            for (int i = 0; i < 3; i++)
            {
                var enemy = SpawnRobot(false).GetComponent<GlobalScripts.Robot>();
                _players.Add(new AIPlayerModel((EnemyRobotModel)enemy.RobotModel));
            }
        }

        private GameObject SpawnRobot(bool isPlayer)
        {
            var spawnPoint = _map.GetSpawnPoint();
            var player = new GameObjectBuilder(_robotPrefab, spawnPoint.Position + Vector3.up * 3, new Quaternion(),
                    go =>
                    {
                        go.GetComponent<GlobalScripts.Robot>().IsPlayer = isPlayer;
                        go.GetComponent<GlobalScripts.Robot>().enabled = true;
                    });

            var spawn = spawnPoint.Instantiate(_mapGameObject.transform);
            GameObject.Destroy(spawn, 10);
            return player.Instantiate(_playersGameObject.transform);
        }
    }
}