using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Project.Scripts.Enums;
using Project.Scripts.Interfaces;
using UnityEngine;

namespace Project.Scripts.Game.Models.Hunting
{
    public class TargetSpawnConfiguration : IDataModel<TargetSpawnConfiguration>
    {
        public string ToJson(TargetSpawnConfiguration model)
        {
            return JsonConvert.SerializeObject(this);
        }
        public TargetSpawnConfiguration FromJson(string json)
        {
            return JsonConvert.DeserializeObject<TargetSpawnConfiguration>(json);
        }

        [JsonConstructor]
        public TargetSpawnConfiguration(
            GameDifficulty Configurationdifficulty,
            int NumberOfTargets,
            float RadiusAwayFromCenter,
            int YLevelCount,
            float YOffset,
            Vector3 Center,
            Vector3 TargetMinSize,
            Vector3 TargetMaxSize
        )
        {
            this.Configurationdifficulty = Configurationdifficulty;
            this.NumberOfTargets = NumberOfTargets;
            this.RadiusAwayFromCenter = RadiusAwayFromCenter;
            this.YLevelCount = YLevelCount;
            this.YOffset = YOffset;
            this.Center = Center;
            this.TargetMinSize = TargetMinSize;
            this.TargetMaxSize = TargetMaxSize;
        }
        public TargetSpawnConfiguration(String json)
        {
            var tmp = FromJson(json);
            this.Configurationdifficulty = tmp.Configurationdifficulty;
            this.NumberOfTargets = tmp.NumberOfTargets;
            this.RadiusAwayFromCenter = tmp.RadiusAwayFromCenter;
            this.YLevelCount = tmp.YLevelCount;
            this.Center = tmp.Center;
            this.TargetMinSize = tmp.TargetMinSize;
            this.TargetMaxSize = tmp.TargetMaxSize;
        }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public GameDifficulty Configurationdifficulty { get; private set; }
        public int NumberOfTargets { get; private set; }
        public float RadiusAwayFromCenter { get; private set; }
        public int YLevelCount { get; private set; }
        public float YOffset { get; private set; }
        
        public Vector3 Center { get; private set; }
        public Vector3 TargetMinSize { get; private set; }
        public Vector3 TargetMaxSize { get; private set; }
    }
}