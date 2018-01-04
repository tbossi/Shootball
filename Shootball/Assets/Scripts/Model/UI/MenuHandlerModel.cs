using System.Collections.Generic;
using Shootball.GlobalScripts.UI;
using UnityEngine;

namespace Shootball.Model.UI
{
    public class MenuHandlerModel
    {
        public enum MenuType
        {
            GAME_START, MATCH_PAUSE
        }

        private readonly Dictionary<MenuType, Canvas> _menus;

        private GameObject ActiveMenu { get; set; }
        public bool IsMenuActive => ActiveMenu != null;

        public MenuHandlerModel(Dictionary<MenuType, Canvas> menus)
        {
            _menus = menus;
        }

        public void OpenMenu(MenuType type)
        {
            ActiveMenu = new GameObject("Menu");
            var menuCanvas = GameObject.Instantiate(_menus[type], ActiveMenu.transform);
            var menuModel = menuCanvas.GetComponent<Menu>().GetMenuModel();
            menuModel.InitializeButtons(this);
        }

        public void CloseMenu()
        {
            GameObject.Destroy(ActiveMenu);
            ActiveMenu = null;
        }
    }
}