using Source.Pause;
using Source.Player.PlayerUI;
using Source.Player.PlayerUI.LevelFinishScreen;
using Source.Yandex.Ads;
using UnityEngine;

namespace Source.Game
{
    public class Game : MonoBehaviour, IActiveScene
    {
        [SerializeField] private PauseView _pauseScreen;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private LevelFinishScreen _levelFinishScreen;

        private Pause.Pause _pause;
        private ILevelFinisher _levelFinisher;
        private LevelLoader.LevelLoader _levelLoader;

        public void Unsubscribe()
        {
            _playerView.PauseButtonClicked -= SetPause;
            _pauseScreen.ContionueButtonClicked -= Continue;
            _pauseScreen.RestartButtonClicked -= Restart;
            _pauseScreen.ExitButtonClicked -= Exit;
            _gameOverView.RestartButtonClicked -= Restart;
            _gameOverView.ExitButtonClicked -= Exit;
            _levelFinishScreen.ExitButtonClicked -= Exit;
            _levelFinishScreen.NextLevelButtonClicked -= OnNextLevelButtonClicked;
        }

        public void Initialize(
            Pause.Pause pause,
            ILevelFinisher levelFinisher,
            LevelLoader.LevelLoader levelLoader,
            YandexAds yandexAds)
        {
            _pause = pause;
            _pauseScreen.Subscribe();
            _gameOverView.Subscribe();
            _levelFinisher = levelFinisher;
            _pauseScreen.Initialize(_pause);
            _playerView.PauseButtonClicked += SetPause;
            _pauseScreen.ContionueButtonClicked += Continue;
            _pauseScreen.RestartButtonClicked += Restart;
            _pauseScreen.ExitButtonClicked += Exit;
            _gameOverView.RestartButtonClicked += Restart;
            _gameOverView.ExitButtonClicked += Exit;
            _levelFinishScreen.ExitButtonClicked += Exit;
            _levelFinishScreen.NextLevelButtonClicked += OnNextLevelButtonClicked;
            _levelFinishScreen.Initialize(yandexAds, _levelFinisher);
            _levelLoader = levelLoader;
        }

        public void SetPause()
            => _pause.Disable();

        public void Continue()
            => _pause.Disable();

        private void Restart()
            => _levelLoader.RestartLevel();

        private void Exit()
            => _levelLoader.BackToMainMenu();

        private void OnNextLevelButtonClicked()
            => _levelLoader.LoadNextLevel();
    }
}
