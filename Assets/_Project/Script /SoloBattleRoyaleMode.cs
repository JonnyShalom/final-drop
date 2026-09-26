using System.Collections.Generic;

namespace FinalDrop.Core
{
    /// <summary>
    /// Solo battle royale ruleset. Win = last player alive.
    /// </summary>
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
            // TODO: lock loadouts, start countdown, spawn SafeZoneController
        }

        public void OnPlayerEliminated(string playerId, string killerId)
        {
            _alivePlayers.Remove(playerId);
            // TODO: update elimination counter UI, kill feed
        }

        public void OnZoneTick(int phaseIndex)
        {
            // TODO: broadcast zone phase warning to UI
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
            // TODO: show results screen, award XP
        }
    }
}
