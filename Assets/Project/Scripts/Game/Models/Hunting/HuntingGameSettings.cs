using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Project.Scripts.Enums;
using Project.Scripts.Interfaces;

namespace Project.Scripts.Game.Models.Hunting
{
    public class HuntingGameSettings : IDataModel<HuntingGameSettings>
    {


        [JsonConstructor]
        public HuntingGameSettings(
            GameDifficulty gamemodedifficulty,
            float gamelifetime,
            float targetSwitchTime
        )
        {
            this.gamemodedifficulty = gamemodedifficulty;
            this.gamelifetime = gamelifetime;
            this.targetSwitchTime = targetSwitchTime;
        }
        
        public HuntingGameSettings(string json)
        {
            var temp = FromJson(json);
            this.gamemodedifficulty = temp.gamemodedifficulty;
            this.gamelifetime = temp.gamelifetime;
            this.targetSwitchTime = temp.targetSwitchTime;

        }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public GameDifficulty gamemodedifficulty { get; private set; }
        public float gamelifetime { get; private set; }
        public float targetSwitchTime { get; private set; }

        public string ToJson(HuntingGameSettings model)
        {
            return JsonConvert.SerializeObject(this);
        }

        public HuntingGameSettings FromJson(string json)
        {
            return JsonConvert.DeserializeObject<HuntingGameSettings>(json);
        }
    }
}