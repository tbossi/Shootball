using System;
using Shootball.GlobalScripts.UI;
using Shootball.Model;
using Shootball.Model.Scene;
using Shootball.Model.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shootball.GlobalScripts
{
    public class SceneHandler : MonoBehaviour
    {
        public static readonly string GAME_START_SCENE = "MenuScene";
        public static readonly string MATCH_SCENE = "MatchScene";

        public GameObject EventSystemPrefab;
        public GameObject MenuHandlerObject;
        public GameObject MatchHandlerObject;

        public MenuHandlerModel MenuHandlerModel => MenuHandlerObject.GetComponent<MenuHandler>().MenuHandlerModel;
        public MatchHandlerModel MatchHandlerModel => MatchHandlerObject.GetComponent<MatchHandler>().MatchHandlerModel;
        public SceneModel SceneModel { get; private set; }

        void Awake()
        {
            Instantiate(EventSystemPrefab);
        }

        void Start()
        {
            var scene = SceneManager.GetActiveScene().name;

            if (scene.Equals(GAME_START_SCENE))
            {
                SceneModel = new GameStartSceneModel(MenuHandlerModel);
            }
            else if (scene.Equals(MATCH_SCENE))
            {
                SceneModel = new MatchSceneModel(MenuHandlerModel, MatchHandlerModel);
            }
            else
            {
                throw new Exception(scene + " scene does not exist.");
            }

            SceneModel.OnStart();
        }

        void Update()
        {
            SceneModel.OnUpdate();
        }
    }
}
