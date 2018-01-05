using UnityEngine;
using UnityEngine.UI;

namespace Shootball.Model.UI
{
    public class StatisticsHUDModel
    {
        private readonly Image _lifeBar;
        private readonly Image _shotsBar;
        private readonly Text _score;

        public StatisticsHUDModel(Image lifebar, Image shotsbar, Text score)
        {
            _lifeBar = lifebar;
            _shotsBar = shotsbar;
            _score = score;
        }

        public void SetScore(int value)
        {
            _score.text = value.ToString();
        }

        public void SetLife(float value, float max)
        {
            var amount = value / max;
            _lifeBar.fillAmount = amount;
            _lifeBar.color = Color.Lerp(Color.red, Color.green, amount);
        }

        public void SetShots(float value, float max)
        {
            _shotsBar.fillAmount = value / max;
        }
    }
}