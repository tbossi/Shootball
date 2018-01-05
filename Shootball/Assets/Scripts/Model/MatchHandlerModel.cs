using System.Collections.Generic;
using Shootball.GlobalScripts.UI;
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
        private readonly Canvas _playerStatisticsPrefab;
        private List<IPlayer> _players;
        private GameObject _mapGameObject;
        private GameObject _playersGameObject;
        private GameObject _HUDCanvas;

        public bool IsMatchEnded { get; private set; }

        public MatchHandlerModel(GameObject robotPrefab, MapModel map, Canvas playerStatisticsPrefab)
        {
            _robotPrefab = robotPrefab;
            _map = map;
            _playerStatisticsPrefab = playerStatisticsPrefab;
        }

        public void OnStart()
        {
            _mapGameObject = new GameObject("Map");
            _playersGameObject = new GameObject("Players");
            _HUDCanvas = new GameObject("HUD");
            _HUDCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

            BeginMatch();
        }

        public void OnUpdate()
        {
            if (!IsMatchEnded)
            {
                if (_players != null)
                {
                    _players.ForEach(player => player.OnUpdate());
                }
            }
        }

        private void BeginMatch()
        {
            _map.Instantiate(_mapGameObject);
            _players = new List<IPlayer>();

            _players.Add(CreatePlayer(true));
            for (int i = 0; i < 10; i++)
            {
                _players.Add(CreatePlayer(false));
            }
            IsMatchEnded = false;
        }

        private IPlayer CreatePlayer(bool isPlayer)
        {
            var robot = SpawnRobot(isPlayer);
            var robotModel = robot.GetComponent<GlobalScripts.Robot>().RobotModel;
            robotModel.SetOnDeathListener(() => {/* IsMatchEnded = true;*/ });

            IPlayer player;
            if (isPlayer)
            {
                var hud = GameObject.Instantiate(_playerStatisticsPrefab, _HUDCanvas.transform);
                var statisticsHUD = hud.GetComponent<StatisticsHUD>().StatisticsHUDModel;
                player = new LocalPlayerModel((PlayerRobotModel)robotModel, statisticsHUD);
            }
            else
            {
                player = new AIPlayerModel((EnemyRobotModel)robotModel);
            }

            return player;
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