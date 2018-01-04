using Shootball.Model.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Shootball.GlobalScripts.UI
{
    public class GameStartMenu : Menu<GameStartMenuModel>
    {
        public CanvasRenderer PlayCanvas;
        public CanvasRenderer ExitCanvas;

        void OnEnable()
        {
            var playButton = PlayCanvas.GetComponent<Button>();
            var exitButton = ExitCanvas.GetComponent<Button>();
            
            MenuModel = new GameStartMenuModel(playButton, exitButton);
        }
    }
}
