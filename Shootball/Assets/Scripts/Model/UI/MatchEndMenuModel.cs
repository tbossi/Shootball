using UnityEngine.UI;

namespace Shootball.Model.UI
{
    public class MatchEndMenuModel : MenuModel
    {
        private readonly Text _matchResult;
        private readonly Button _endButton;

        public MatchEndMenuModel(Text matchResult, Button endButton)
        {
            _matchResult = matchResult;
            _endButton = endButton;
        }

        public override void Initialize(MenuHandlerModel menuHandlerModel, object additionalData)
        {
            var matchStatus = (MatchStatusModel)additionalData;
            var message = matchStatus.LocalPlayerWon ? "You won!" : "You lost";
            _matchResult.text = message;
            _endButton.onClick.AddListener(GoToGameStartMenu);
        }
    }
}