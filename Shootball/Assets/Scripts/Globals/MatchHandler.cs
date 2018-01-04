using System.Collections.Generic;
using Shootball.Model;
using Shootball.Model.Player;
using Shootball.Model.Robot;
using Shootball.Provider;
using Shootball.Utility;
using UnityEngine;

namespace Shootball.GlobalScripts
{
    public class MatchHandler : MonoBehaviour
    {
        public GameObject RobotPrefab;
        public GameObject MapBuilderScriptObject;

        public MatchHandlerModel MatchHandlerModel { get; private set; }

        void OnEnable()
        {
            var map = MapBuilderScriptObject.GetComponent<Map>().MapModel;
            MatchHandlerModel = new MatchHandlerModel(RobotPrefab, map);
        }
    }
}
