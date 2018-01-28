using Shootball.Model.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Shootball.GlobalScripts.UI
{
    public class MatchPauseMenu : Menu<MatchPauseMenuModel>
    {
        public CanvasRenderer ResumeCanvas;
        public CanvasRenderer EndMatchCanvas;

        void OnEnable()
        {
            var resumeButton = ResumeCanvas.GetComponent<Button>();
            var endMatchButton = EndMatchCanvas.GetComponent<Button>();
            MenuModel = new MatchPauseMenuModel(resumeButton, endMatchButton);
        }
    }
}
