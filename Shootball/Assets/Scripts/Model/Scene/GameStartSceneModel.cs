using Shootball.Model.UI;
using UnityEngine;

namespace Shootball.Model.Scene
{
    public class GameStartSceneModel : SceneModel
    {
        public GameStartSceneModel(MenuHandlerModel menuHandlerModel) : base(menuHandlerModel)
        { }

        public override void OnStart()
        {
            MenuHandlerModel.OpenMenu(MenuHandlerModel.MenuType.GAME_START);
        }

        public override void OnUpdate()
        { }
    }
}