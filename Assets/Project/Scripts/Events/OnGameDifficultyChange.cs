using Project.Scripts.Enums;

namespace Project.Scripts.Events
{
    public class OnGameDifficultyChange
    {
        public OnGameDifficultyChange(GameDifficulty oldDifficulty, GameDifficulty newDifficulty)
        {
            this.oldDifficulty = oldDifficulty;
            this.newDifficulty = newDifficulty;
        }
        public GameDifficulty oldDifficulty { get; private set; }
        public GameDifficulty newDifficulty { get; private set; }
    }
}