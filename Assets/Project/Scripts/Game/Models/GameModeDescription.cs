using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Project.Scripts.Enums;
using Project.Scripts.Interfaces;

namespace Project.Scripts.Game.Models
{
    public class GameModeDescription: IDataModel<GameModeDescription>
    {
        [JsonConstructor]
        public GameModeDescription(GameDifficulty gamemodedifficulty,String description)
        {
            this.gamemodedifficulty = gamemodedifficulty;
            this.description = description;
        }
        public GameModeDescription(String json)
        {
            var tmp = FromJson(json);
            this.gamemodedifficulty = tmp.gamemodedifficulty;
            this.description = tmp.description;
        }


        [JsonConverter(typeof(StringEnumConverter))]
        public GameDifficulty gamemodedifficulty { get; private set; }
        public string description { get; private set; }
        public string ToJson(GameModeDescription model)
        {
            return JsonConvert.SerializeObject(this);
        }

        public GameModeDescription FromJson(string json)
        {
           return JsonConvert.DeserializeObject<GameModeDescription>(json);
        }
    }
}