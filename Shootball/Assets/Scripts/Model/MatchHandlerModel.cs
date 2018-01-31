using System;
using System.Collections.Generic;
using Shootball.GlobalScripts;
using Shootball.GlobalScripts.UI;
using Shootball.Model.Player;
using Shootball.Model.Robot;
using Shootball.Utility;
using Shootball.Utility.Navigation;
using UnityEngine;
using UnityEngine.UI;

namespace Shootball.Model
{
    public class MatchHandlerModel
    {
        private readonly MatchHandler _matchHandler;
        private readonly GameObject _robotPrefab;
        private readonly MapModel _map;
        private readonly Canvas _playerStatisticsPrefab;
        private readonly Camera _minimapCamera;
        private readonly CanvasRenderer _minimapPrefab;
        private readonly Image _cursor;
        private GameObject _mapGameObject;
        private GameObject _playersGameObject;
        private GameObject _HUDCanvas;
        private int _totalEnemies;

        public MatchStatusModel MatchStatusModel { get; private set; }

        public MatchHandlerModel(MatchHandler matchHandler, GameObject robotPrefab, MapModel map,
            Canvas playerStatisticsPrefab, Camera minimapCamera, CanvasRenderer minimapPrefab, Image cursor)
        {
            _matchHandler = matchHandler;
            _robotPrefab = robotPrefab;
            _map = map;
            _playerStatisticsPrefab = playerStatisticsPrefab;
            _minimapCamera = minimapCamera;
            _minimapPrefab = minimapPrefab;
            _cursor = cursor;
        }

        public void OnStart()
        {
            _mapGameObject = new GameObject("Map");
            _playersGameObject = new GameObject("Players");
            _HUDCanvas = new GameObject("HUD");
            _HUDCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            _totalEnemies = 5;

            BeginMatch(_totalEnemies);
        }

        public void OnUpdate()
        {
            if (MatchStatusModel != null)
            {
                if (!MatchStatusModel.IsMatchEnded)
                {
                    MatchStatusModel.Players.ForEach(player => player.OnUpdate());
                }
            }
        }

        private void BeginMatch(int totalEnemies)
        {
            _map.InstantiateObjects(_mapGameObject);

            _matchHandler.StartCoroutine(_map.GenerateNavPoints(navGraph =>
            {
                var players = new List<IPlayer>();
                players.Add(CreatePlayer(true, "LocalPlayer", navGraph));
                for (int i = 0; i < totalEnemies; i++)
                {
                    players.Add(CreatePlayer(false, $"E{i}-BX", navGraph));
                }

                MatchStatusModel = new MatchStatusModel(players);
            }));
        }

        private IPlayer CreatePlayer(bool isPlayer, string name, Graph<Vector3> navGraph)
        {
            var robot = SpawnRobot(isPlayer);
            var robotModel = robot.GetComponent<GlobalScripts.Robot>().RobotModel;

            IPlayer player;
            if (isPlayer)
            {
                var hud = GameObject.Instantiate(_playerStatisticsPrefab, _HUDCanvas.transform);
                GameObject.Instantiate(_minimapPrefab, _HUDCanvas.transform);
                GameObject.Instantiate(_cursor, _HUDCanvas.transform);
                var statisticsHUD = hud.GetComponent<StatisticsHUD>().StatisticsHUDModel;
                var minimapCamera = GameObject.Instantiate(_minimapCamera, _playersGameObject.transform);
                player = new LocalPlayerModel(name, (PlayerRobotModel)robotModel, statisticsHUD, minimapCamera);
            }
            else
            {
                player = new AIPlayerModel(name, (EnemyRobotModel)robotModel, navGraph);
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