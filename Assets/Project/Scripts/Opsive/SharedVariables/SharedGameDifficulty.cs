using BehaviorDesigner.Runtime;
using Project.Scripts.Enums;

namespace Project.Scripts.Opsive.SharedVariables
{
    [System.Serializable]
    public class SharedGameDifficulty : SharedVariable<GameDifficulty>
    {
        public static implicit operator SharedGameDifficulty(GameDifficulty value)
        {
            return new SharedGameDifficulty {Value = value};
        }
    }
}