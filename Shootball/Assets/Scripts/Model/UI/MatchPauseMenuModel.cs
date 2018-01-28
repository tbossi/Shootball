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

        public override void Initialize(MenuHandlerModel menuHandlerModel, object additionalData = null)
        {
            _resumeButton.onClick.AddListener(() => OnResumeMatch(menuHandlerModel));
            _endMatchButton.onClick.AddListener(GoToGameStartMenu);
        }

        private void OnResumeMatch(MenuHandlerModel menuHandlerModel)
        {
            menuHandlerModel.CloseMenu();
        }
    }
}