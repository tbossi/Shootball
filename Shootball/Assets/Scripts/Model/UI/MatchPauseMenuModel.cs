using Shootball.GlobalScripts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Shootball.Model.UI
{
    public class MatchPauseMenuModel : MenuModel
    {
        private readonly Button _resumeButton;
        private readonly Button _endMatchButton;

        public MatchPauseMenuModel(Button resumeButton, Button endMatchButton)
        {
            _resumeButton = resumeButton;
            _endMatchButton = endMatchButton;
        }

        public override void InitializeButtons(MenuHandlerModel menuHandlerModel)
        {
            _resumeButton.onClick.AddListener(() => OnResumeMatch(menuHandlerModel));
            _endMatchButton.onClick.AddListener(OnExitMatch);
        }

        private void OnResumeMatch(MenuHandlerModel menuHandlerModel)
        {
            menuHandlerModel.CloseMenu();
        }

        private void OnExitMatch()
        {
            SceneManager.LoadScene(SceneHandler.GAME_START_SCENE);
        }
    }
}