using Shootball.Model.UI;

namespace Shootball.Model.Scene
{
    public abstract class SceneModel
    {
        protected readonly MenuHandlerModel MenuHandlerModel;

        public SceneModel(MenuHandlerModel menuHandlerModel)
        {
            MenuHandlerModel = menuHandlerModel;
        }

        public abstract void OnStart();
        public abstract void OnUpdate();
    }
}