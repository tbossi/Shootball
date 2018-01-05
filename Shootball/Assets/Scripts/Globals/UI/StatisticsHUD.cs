using Shootball.Model.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Shootball.GlobalScripts.UI
{
    [RequireComponent(typeof(Canvas))]
    public class StatisticsHUD : MonoBehaviour
    {
        public Image LifeBar;
        public Image ShotsBar;
        public Text Score;

        [HideInInspector]
        public StatisticsHUDModel StatisticsHUDModel { get; private set; }

        public void OnEnable()
        {
            StatisticsHUDModel = new StatisticsHUDModel(LifeBar, ShotsBar, Score);
        }
    }
}
