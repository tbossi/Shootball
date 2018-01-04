using System.Collections;
using System.Collections.Generic;
using Shootball.Model.UI;
using UnityEngine;

namespace Shootball.GlobalScripts.UI
{
    public class MenuHandler : MonoBehaviour
    {
        public Canvas GameStartCanvasPrefab;
        public Canvas MatchPauseCanvasPrefab;

        public MenuHandlerModel MenuHandlerModel { get; private set; }
        
        void OnEnable()
        {
            var dictionary = new Dictionary<MenuHandlerModel.MenuType, Canvas>
            {
                { MenuHandlerModel.MenuType.GAME_START, GameStartCanvasPrefab },
                { MenuHandlerModel.MenuType.MATCH_PAUSE, MatchPauseCanvasPrefab }
            };

            MenuHandlerModel = new MenuHandlerModel(dictionary);
        }
    }
}
