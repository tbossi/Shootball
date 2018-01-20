using System.Collections.Generic;
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
        private readonly GameObject _robotPrefab;
        private readonly MapModel _map;
        private readonly Canvas _playerStatisticsPrefab;
        private readonly Camera _minimapCamera;
        private readonly CanvasRenderer _minimapPrefab;        
        private readonly Image _cursor;    
        private List<IPlayer> _players;
        private GameObject _mapGameObject;
        private GameObject _playersGameObject;
        private GameObject _HUDCanvas;

        public bool IsMatchEnded { get; private set; }

        public MatchHandlerModel(GameObject robotPrefab, MapModel map, Canvas playerStatisticsPrefab,
                Camera minimapCamera, CanvasRenderer minimapPrefab, Image cursor)
        {
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
            var navGraph = _map.Instantiate(_mapGameObject);

            /*
            //Debug: shows navGraph
            foreach (var node in navGraph.GetNodes())
            {
                var neighbors = navGraph.GetNeighbors(node);
                foreach (var neighbor in neighbors)
                {
                    Debug.DrawLine(node, neighbor.Key, Color.cyan, 300);
                }
            }
            */            

            var enemies = 1;
            _players = new List<IPlayer>();

            _players.Add(CreatePlayer(true, navGraph));
            for (int i = 0; i < enemies; i++)
            {
                _players.Add(CreatePlayer(false, navGraph));
            }

            _players.ForEach(p => p.SetPlayersList(_players));
            IsMatchEnded = false;
        }

        private IPlayer CreatePlayer(bool isPlayer, Graph<Vector3> navGraph)
        {
            var robot = SpawnRobot(isPlayer);
            var robotModel = robot.GetComponent<GlobalScripts.Robot>().RobotModel;
            robotModel.SetOnDeathListener(() => {/* IsMatchEnded = true;*/ });

            IPlayer player;
            if (isPlayer)
            {
                var hud = GameObject.Instantiate(_playerStatisticsPrefab, _HUDCanvas.transform);
                GameObject.Instantiate(_minimapPrefab, _HUDCanvas.transform);                
                GameObject.Instantiate(_cursor, _HUDCanvas.transform);
                var statisticsHUD = hud.GetComponent<StatisticsHUD>().StatisticsHUDModel;
                var minimapCamera = GameObject.Instantiate(_minimapCamera, _playersGameObject.transform);
                player = new LocalPlayerModel((PlayerRobotModel)robotModel, statisticsHUD, minimapCamera);
            }
            else
            {
                player = new AIPlayerModel((EnemyRobotModel)robotModel, navGraph);
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