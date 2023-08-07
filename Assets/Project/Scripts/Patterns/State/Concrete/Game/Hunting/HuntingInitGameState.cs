using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Scripts.Enums;
using Project.Scripts.Game.Models.Hunting;
using Project.Scripts.Patterns.State.Concrete.Game.Hunting;
using UnityEngine;
using UnityEngine.VFX;

namespace Project.Scripts.Patterns.State.Concrete.Game.Hunting
{
    public class HuntingInitGameState : HuntingGameBaseState
    {
        private List<TargetSpawnConfiguration> _targetSpawnConfigurations;
        private VisualEffect _gunSpawnVFX;

        public HuntingInitGameState(HuntingStateMachine stateMachine, JObject jsonSettings, VisualEffect gunSpawnVFX) : base(stateMachine)
        {
            _targetSpawnConfigurations = new List<TargetSpawnConfiguration>();
            _gunSpawnVFX = gunSpawnVFX;
            IList<JToken> spawnConfigs = jsonSettings["HuntingGameSpawningConfiguration"].Children().ToList();
            foreach (JToken spawnConfig in spawnConfigs)
            {
                _targetSpawnConfigurations.Add(spawnConfig.ToObject<TargetSpawnConfiguration>());
            }
        }

        public override void OnStateStart()
        {
            Debug.Log("HuntingInitGameState, Started");
            SpawnTargets();
            _gunSpawnVFX.Play();
            SelectRandomTargets();
        }

        public override void OnStateUpdate()
        {
            Debug.Log("HuntingInitGameState, Update");
        }

        public override void OnStateExit()
        {
            Debug.Log("HuntingInitGameState, Exit");
            HuntingStateMachine.HasGameStarted = true;
        }

        public override void OnCollision()
        {
            Debug.Log("HuntingInitGameState, Collided");
        }

        private void SpawnTargets()
        {
            TargetSpawnConfiguration correctConfiguration =
                _targetSpawnConfigurations.First(x => x.Configurationdifficulty == _stateMachine.CurrentGameDifficulty);
            Transform center = new GameObject("Centerhelper").transform;
            center.transform.position = correctConfiguration.Center;
            bool CanEvenlyFitTargetCountToLayers =
                (correctConfiguration.NumberOfTargets % correctConfiguration.YLevelCount) == 0;
            int remainder = CanEvenlyFitTargetCountToLayers
                ? 0
                : correctConfiguration.NumberOfTargets % correctConfiguration.YLevelCount;
            int targetCountMinusRemainder = correctConfiguration.NumberOfTargets - remainder;
            for (int i = 0; i < correctConfiguration.YLevelCount; i++)
            {
                float SpawnLevelY = center.position.y;
                int TargetCountForLevel = ((correctConfiguration.YLevelCount - 1) == i)
                    ? (targetCountMinusRemainder / correctConfiguration.YLevelCount) + remainder
                    : (targetCountMinusRemainder / correctConfiguration.YLevelCount);
                float angleStep = 360f / TargetCountForLevel;
                if ((correctConfiguration.YLevelCount % 2) == 0)
                {
                    int pairlevel = Mathf.FloorToInt(i / 2.0f);
                    if ((i % 2) == 0)
                        SpawnLevelY = calculateYOffset(SpawnLevelY,
                            (correctConfiguration.YOffset * (pairlevel + 0.5f)), true);
                    else
                        SpawnLevelY = calculateYOffset(SpawnLevelY,
                            (correctConfiguration.YOffset * (pairlevel + 0.5f)), false);
                }
                else
                {
                    if (i != 0)
                    {
                        int pairlevel = Mathf.FloorToInt((i - 1) / 2.0f);
                        if ((i % 2) == 0)
                            SpawnLevelY = calculateYOffset(SpawnLevelY,
                                (correctConfiguration.YOffset * (pairlevel + 1.0f)), true);
                        else
                            SpawnLevelY = calculateYOffset(SpawnLevelY,
                                (correctConfiguration.YOffset * (pairlevel + 1.0f)), false);
                    }
                }

                for (int j = 0; j < TargetCountForLevel; j++)
                {
                    float angle = j * angleStep;
                    float x = Mathf.Cos(Mathf.Deg2Rad * angle) * correctConfiguration.RadiusAwayFromCenter;
                    float z = Mathf.Sin(Mathf.Deg2Rad * angle) * correctConfiguration.RadiusAwayFromCenter;
                    Vector3 spawnPosition = new Vector3(x, SpawnLevelY, z) + center.position;

                    Target.Target target = _stateMachine.TargetPool.Get();
                    target.HelperTarget = center;
                    target.gameObject.transform.position = spawnPosition;
                    target.gameObject.transform.rotation = Quaternion.identity;
                    target.transform.SetParent(center);
                    _stateMachine._roamingTargets.Add(target);
                }

            }
        }

        private float calculateYOffset(float n, float yoffset, bool positiveDirection)
        {
            int sign = positiveDirection ? 1 : -1;
            float yOffsetPosition = n + (yoffset * sign);
            return yOffsetPosition;
        }

        private void SelectRandomTargets()
        {
            switch (_stateMachine.CurrentGameDifficulty)
            {
                case GameDifficulty.EASY:
                    SelectEasyTargets();
                    break;
                case GameDifficulty.MEDIUM:
                    SelectMediumTargets();
                    break;
            }
        }

        private void SelectEasyTargets()
        {
            _stateMachine._roamingTargets.Shuffle();
            _stateMachine._roamingTargets.ElementAt(Random.Range(0, _stateMachine._roamingTargets.Count)).Modifier =
                new GreenModifier();
        }

        private void SelectMediumTargets()
        {
            _stateMachine._roamingTargets.Shuffle();
            var selectedTargets = _stateMachine._roamingTargets.Take(4).ToList();
            selectedTargets[0].Modifier = new GreenModifier();
            selectedTargets[1].Modifier = new OrangeModifier();
            selectedTargets[2].Modifier = new OrangeModifier();
            selectedTargets[3].Modifier = new OrangeModifier();
        }
    }
}