using System;
using Shootball.GlobalScripts;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Shootball.Model.UI
{
    public abstract class MenuModel
    {
        public abstract void Initialize(MenuHandlerModel menuHandlerModel, object additionalData = null);

        protected void GoToGameStartMenu()
        {
            SceneManager.LoadScene(SceneHandler.GAME_START_SCENE);
        }
    }
}