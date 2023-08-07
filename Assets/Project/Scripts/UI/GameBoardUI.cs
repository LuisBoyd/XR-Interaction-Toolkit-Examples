using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project.Scripts.Enums;
using Project.Scripts.Events;
using Project.Scripts.Game;
using Project.Scripts.Game.Models;
using Project.Scripts.Patterns.Observer;
using Project.Scripts.Patterns.State.Concrete.Game.Hunting;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.UI
{
    public class GameBoardUI : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI textcontainer; //TextContainer
        [SerializeField] protected Button playlvlBtn;
        private List<GameModeDescription> _gameModeDescriptions;
        private GameDifficulty _currentDifficulty;

        //private Animator _uiAnimator;
        private void Awake()
        {
            _currentDifficulty = GameDifficulty.NONE;
            EventBus.Instance.Subscribe<OnGameDifficultyChange>(On_GameDifficultyChange);
            // EventBus.Instance.Subscribe<GameStartEvent>(On_GameStart);
            // EventBus.Instance.Subscribe<GameEndEvent>(On_GameEnd);
            _gameModeDescriptions = new List<GameModeDescription>();
            JsonSerializer serializer = new JsonSerializer(); //GET ALL THE JSON SETTINGS
            using (StreamReader streamReader = new StreamReader(string.Join('/', new string[]
                   {
                       Application.dataPath,
                       Constants.HuntingJsonConfigLocation
                   })))
            {
               JObject HuntingGameJsonSettings = JObject.Parse(streamReader.ReadToEnd());
               foreach (JToken Description in HuntingGameJsonSettings["HuntingGameModeDescriptions"].Children())
               {
                   _gameModeDescriptions.Add(Description.ToObject<GameModeDescription>());
               }
            }

            playlvlBtn.onClick.AddListener(() => EventBus.Instance.Trigger<GameStartEvent>(new GameStartEvent())); //should clean up the listeners when destroyed by itself
        }
        private void Start()
        {
            textcontainer.text = _gameModeDescriptions.First(x => x.gamemodedifficulty == _currentDifficulty)
                .description;
            playlvlBtn.interactable = false;
        }

        public void ChangeDifficulty(string difficulty)
        {
            try
            {
                GameDifficulty selectedDifficulty = Enum.Parse<GameDifficulty>(difficulty, true);
                if(_currentDifficulty == selectedDifficulty) return;
                EventBus.Instance.Trigger<OnGameDifficultyChange>(new OnGameDifficultyChange(_currentDifficulty, selectedDifficulty));
                _currentDifficulty = selectedDifficulty;
            }
            catch (Exception e)
            {
                Debug.LogWarning($"Failed to convert {difficulty} to type GameDifficulty Enum");
            }
        }
        
        private void On_GameDifficultyChange(OnGameDifficultyChange evnt)
        {
            //if we have the textcontainer
            textcontainer.text = _gameModeDescriptions.First(x => x.gamemodedifficulty == evnt.newDifficulty)
                .description;
            playlvlBtn.interactable = evnt.newDifficulty != GameDifficulty.NONE;
        }

        // private void On_GameStart(GameStartEvent evnt)
        // {
        //     EventBus.Instance.Unsubscribe<OnGameDifficultyChange>(On_GameDifficultyChange);
        //     _uiAnimator.ResetTrigger("Show");
        //     _uiAnimator.SetTrigger("Hide");
        // }
        //
        // private void On_GameEnd(GameEndEvent evnt)
        // {
        //     EventBus.Instance.Subscribe<OnGameDifficultyChange>(On_GameDifficultyChange);
        //     _uiAnimator.ResetTrigger("Hide");
        //     _uiAnimator.SetTrigger("Show");
        // }
        
    }
}