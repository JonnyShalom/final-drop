using System.Collections.Generic;

namespace FinalDrop.Core
{
    public class SoloBattleRoyaleMode : IGameMode
    {
        public string ModeName => "Solo";

        private readonly HashSet<string> _alivePlayers = new HashSet<string>();

        public void RegisterPlayer(string playerId)
        {
            _alivePlayers.Add(playerId);
        }

        public void OnMatchStart()
        {
        }

        public void OnPlayerEliminated(string playerId, string killerId)
        {
            _alivePlayers.Remove(playerId);
        }

        public void OnZoneTick(int phaseIndex)
        {
        }

        public bool CheckWinCondition(out string winnerId)
        {
            if (_alivePlayers.Count == 1)
            {
                foreach (var id in _alivePlayers)
                {
                    winnerId = id;
                    return true;
                }
            }

            winnerId = null;
            return false;
        }

        public void OnMatchEnd()
        {
        }
    }
}
