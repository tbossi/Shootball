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
        }

        public override void OnUpdate()
        {
            if (Inputs.Pause.Active)
            {
                MenuHandlerModel.OpenMenu(MenuHandlerModel.MenuType.MATCH_PAUSE);
            }
            
            if (MenuHandlerModel.IsMenuActive)
            {
                ShowCursor();
            }
            else
            {
                HideCursor();
                _matchHandlerModel.OnUpdate();
            }            
        }

        private void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}