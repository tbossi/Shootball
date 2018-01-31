using Shootball.Model.UI;
using Shootball.Provider;
using UnityEngine;

namespace Shootball.Model.Scene
{
    public class MatchSceneModel : SceneModel
    {
        private readonly MatchHandlerModel _matchHandlerModel;

        public MatchSceneModel(MenuHandlerModel menuHandlerModel, MatchHandlerModel matchHandlerModel)
                : base(menuHandlerModel)
        {
            _matchHandlerModel = matchHandlerModel;
        }

        public override void OnStart()
        {
            _matchHandlerModel.OnStart();
            MenuHandlerModel.SetOnOpenListener(ShowCursor);
            MenuHandlerModel.SetOnCloseListener(LockCursor);
            LockCursor();
        }

        public override void OnUpdate()
        {
            if (Inputs.Pause.Value)
            {
                MenuHandlerModel.OpenMenu(MenuHandlerModel.MenuType.MATCH_PAUSE);
            }

            if (!MenuHandlerModel.IsMenuActive)
            {
                if (_matchHandlerModel.MatchStatusModel != null
                    && _matchHandlerModel.MatchStatusModel.IsMatchEnded)
                {
                    MenuHandlerModel.OpenMenu(MenuHandlerModel.MenuType.MATCH_END,
                        _matchHandlerModel.MatchStatusModel);
                }
                else
                {
                    _matchHandlerModel.OnUpdate();
                }
            }
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}