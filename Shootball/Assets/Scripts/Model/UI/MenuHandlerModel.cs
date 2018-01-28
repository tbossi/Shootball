using System;
using System.Collections.Generic;
using Shootball.GlobalScripts.UI;
using UnityEngine;

namespace Shootball.Model.UI
{
    public class MenuHandlerModel
    {
        public enum MenuType
        {
            GAME_START, MATCH_PAUSE, MATCH_END
        }

        private readonly Dictionary<MenuType, Canvas> _menus;
        private List<Action> _onOpenCallbacks;
        private List<Action> _onCloseCallbacks;

        private GameObject ActiveMenu { get; set; }
        public bool IsMenuActive => ActiveMenu != null;

        public MenuHandlerModel(Dictionary<MenuType, Canvas> menus)
        {
            _menus = menus;
            _onOpenCallbacks = new List<Action>();
            _onCloseCallbacks = new List<Action>();
        }

        public void SetOnOpenListener(Action action)
        {
            _onOpenCallbacks.Add(action);
        }

        public void SetOnCloseListener(Action action)
        {
            _onCloseCallbacks.Add(action);
        }

        public void OpenMenu(MenuType type, object additionalData = null)
        {
            if (IsMenuActive) { CloseMenu(); }

            _onOpenCallbacks.ForEach(c => c.Invoke());
            ActiveMenu = new GameObject("Menu");
            var menuCanvas = GameObject.Instantiate(_menus[type], ActiveMenu.transform);
            var menuModel = menuCanvas.GetComponent<Menu>().GetMenuModel();
            menuModel.Initialize(this, additionalData);
        }

        public void CloseMenu()
        {
            _onCloseCallbacks.ForEach(c => c.Invoke());
            GameObject.Destroy(ActiveMenu);
            ActiveMenu = null;
        }
    }
}