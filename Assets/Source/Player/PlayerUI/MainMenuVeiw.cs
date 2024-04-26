using System;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Player.PlayerUI
{
    public class MainMenuVeiw : MonoBehaviour
    {
        [SerializeField] private Button _play;
        [SerializeField] private Button _leaderboard;
        [SerializeField] private Button _settings;

        public event Action PlayButtonClicked;

        public event Action LeaderboardButtonClicked;

        public event Action SettingsButtonClicked;

        private void OnEnable()
        {
            _play.onClick.AddListener(() => PlayButtonClicked?.Invoke());
            _leaderboard.onClick.AddListener(() => LeaderboardButtonClicked?.Invoke());
            _settings.onClick.AddListener(() => SettingsButtonClicked?.Invoke());
        }

        private void OnDisable()
        {
            _play.onClick.RemoveListener(() => PlayButtonClicked?.Invoke());
            _leaderboard.onClick.RemoveListener(() => LeaderboardButtonClicked?.Invoke());
            _settings.onClick.RemoveListener(() => SettingsButtonClicked?.Invoke());
        }
    }
}
