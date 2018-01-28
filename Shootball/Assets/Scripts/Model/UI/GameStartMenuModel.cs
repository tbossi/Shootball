using Shootball.GlobalScripts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Shootball.Model.UI
{
    public class GameStartMenuModel : MenuModel
    {
        private readonly Button _playButton;
        private readonly Button _exitButton;

        public GameStartMenuModel(Button playButton, Button exitButton)
        {
            _playButton = playButton;
            _exitButton = exitButton;
        }

        public override void Initialize(MenuHandlerModel menuHandlerModel, object additionalInfo = null)
        {
            _playButton.onClick.AddListener(OnPlayButton);
            _exitButton.onClick.AddListener(OnExitButton);
        }

        private void OnPlayButton()
        {
            SceneManager.LoadScene(SceneHandler.MATCH_SCENE);
        }

        private void OnExitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            UnityEngine.Application.Quit();
#endif
        }
    }
}