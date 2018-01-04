using System;
using Shootball.Model.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shootball.GlobalScripts.UI
{
    [RequireComponent(typeof(Canvas))]
    public abstract class Menu : MonoBehaviour
    {
        protected Canvas MenuCanvas => GetComponent<Canvas>();
        public abstract MenuModel GetMenuModel();
    }

    public abstract class Menu<M> : Menu where M : MenuModel
    {
        protected M MenuModel { get; set; }

        public override MenuModel GetMenuModel()
        {
            return MenuModel;
        }
    }
}
