using Shootball.Model;
using Shootball.Motion;
using Shootball.Provider;
using UnityEngine;

namespace Shootball
{
    public class Global : MonoBehaviour
    {
        public GameObject RobotPrefab;
        public GameObject PlayerRobotObject;
        public GameObject Arena;
        public GameObject ArenaBounds;

        private GameObject enemy;

        private PlayerRobotModel PlayerRobot => (PlayerRobotModel) PlayerRobotObject.GetComponent<Robot>().RobotModel;
        
        void Start()
        {
            BuildArena();
            HideCursor();
            enemy = Instantiate(RobotPrefab, new Vector3(10,3,10), new Quaternion()) as GameObject;
        }

        void Update()
        {
            if (Inputs.Pause.Active) { ShowCursor(); }

            if (Inputs.Shoot.Active) { PlayerRobot.Shoot(); }
            PlayerRobot.Target(Inputs.Target.Active);

            if (Inputs.RotateLeft.Active) { PlayerRobot.Turn(Spin.Left); }
            else if (Inputs.RotateRight.Active) { PlayerRobot.Turn(Spin.Right); }

            if (Inputs.RotateUp.Active) { PlayerRobot.Aim(Spin.Up); }
            else if (Inputs.RotateDown.Active) { PlayerRobot.Aim(Spin.Down); }

            if (Inputs.MoveForward.Active) { PlayerRobot.Move(Direction.Forward); }
            else if (Inputs.MoveBackward.Active) { PlayerRobot.Move(Direction.Backward); }

            if (Inputs.MoveLeft.Active) { PlayerRobot.Move(Direction.Left); }
            else if (Inputs.MoveRight.Active) { PlayerRobot.Move(Direction.Right); }

            //playEnemy();
        }

        private void BuildArena()
        {}

        private void playEnemy()
        {
            var enemyModel = (EnemyRobotModel) enemy.GetComponent<Robot>().RobotModel;
            if (Random.Range(-3,3) >= 1)
            {
                enemyModel.Shoot();
            }
            else
            {
                var i = Random.Range(0,3);
                if (i == 0) enemyModel.Move(Direction.Backward);
                else if (i == 1) enemyModel.Move(Direction.Forward);
                else if (i == 2) enemyModel.Move(Direction.Left);
                else if (i == 3) enemyModel.Move(Direction.Right);
            }
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
