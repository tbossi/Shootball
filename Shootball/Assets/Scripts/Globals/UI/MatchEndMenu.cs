using Shootball.Model.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Shootball.GlobalScripts.UI
{
    public class MatchEndMenu : Menu<MatchEndMenuModel>
    {
        public Text MatchResult;
        public CanvasRenderer EndMatchCanvas;      

        void OnEnable()
        {
            var endMatchButton = EndMatchCanvas.GetComponent<Button>();
            MenuModel = new MatchEndMenuModel(MatchResult, endMatchButton);
        }
    }
}
