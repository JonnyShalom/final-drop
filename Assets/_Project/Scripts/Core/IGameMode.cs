namespace FinalDrop.Core
{
    public interface IGameMode
    {
        string ModeName { get; }
        void OnMatchStart();
        void OnPlayerEliminated(string playerId, string killerId);
        void OnZoneTick(int phaseIndex);
        bool CheckWinCondition(out string winnerId);
        void OnMatchEnd();
    }
}
