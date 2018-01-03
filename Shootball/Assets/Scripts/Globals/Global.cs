using System.Collections.Generic;
using Shootball.Model;
using Shootball.Model.Player;
using Shootball.Model.Robot;
using Shootball.Provider;
using Shootball.Utility;
using UnityEngine;

namespace Shootball.GlobalScripts
{
    public class Global : MonoBehaviour
    {
        public GameObject RobotPrefab;
        public GameObject MapBuilderScriptObject;

        private GameObject MapGameObject { get; set; }
        private GameObject PlayersGameObject { get; set; }        
        private MapModel Map => MapBuilderScriptObject.GetComponent<Map>().MapModel;
        private List<IPlayer> Players { get; set; }

        void Start()
        {
            MapGameObject = new GameObject("Map");
            PlayersGameObject = new GameObject("Players");
            StartMatch();

            //HideCursor();
        }

        void Update()
        {
            if (Inputs.Pause.Active) { ShowCursor(); }

            if (Players != null)
            {
                Players.ForEach(player => player.OnUpdate());
            }
        }

        private void StartMatch()
        {
            Map.Instantiate(MapGameObject);

            Players = new List<IPlayer>();
            var player = SpawnRobot(true).GetComponent<Robot>();  
            Players.Add(new LocalPlayerModel((PlayerRobotModel)player.RobotModel));

            for (int i = 0; i < 3; i++)
            {
                var enemy = SpawnRobot(false).GetComponent<Robot>();
                Players.Add(new AIPlayerModel((EnemyRobotModel)enemy.RobotModel));
            }
        }

        private GameObject SpawnRobot(bool isPlayer)
        {
            var spawnPoint = Map.GetSpawnPoint();
            var player = new GameObjectBuilder(RobotPrefab, spawnPoint.Position + Vector3.up * 3, new Quaternion(),
                    go => {
                        go.GetComponent<Robot>().IsPlayer = isPlayer;
                        go.GetComponent<Robot>().enabled = true;
                    });

            var spawn = spawnPoint.Instantiate(MapGameObject.transform);
            GameObject.Destroy(spawn, 10);
            return player.Instantiate(PlayersGameObject.transform);
        }

        private void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
