using System.Collections.Generic;
using System.Linq;
using Shootball.Model.Player;

namespace Shootball.Model
{
    public class MatchStatusModel
    {
        public readonly List<IPlayer> Players;

        private LocalPlayerModel LocalPlayer =>
            (LocalPlayerModel)Players.Where(p => p.GetType() == typeof(LocalPlayerModel)).First();

        private IEnumerable<AIPlayerModel> EnemyPlayers =>
            Players.Where(p => p.GetType() == typeof(AIPlayerModel)).Select(p => (AIPlayerModel)p);

        public bool LocalPlayerWon =>
            LocalPlayer.Robot.Statistics.IsAlive && EnemyPlayers.All(p => !p.Robot.Statistics.IsAlive);

        public bool IsMatchEnded =>
            !LocalPlayer.Robot.Statistics.IsAlive || EnemyPlayers.All(p => !p.Robot.Statistics.IsAlive);

        public MatchStatusModel(List<IPlayer> players)
        {
            Players = players;
            Players.ForEach(p => p.SetMatchStatus(this));
        }
    }
}