using System;
using System.Collections.Generic;
using Source.InteractiveObjects.Objects.LevelExit;
using Source.Saves;
using Source.Shop;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.Game
{
    public class LevelFinisher : MonoBehaviour, ILevelFinisher, IDataReader, IDataWriter
    {
        private Gameboard.Gameboard _gameboard;
        private ScoreDefiner.ScoreDefiner _scoreDefiner;
        private StepCounter.StepCounter _stepCounter;
        private Wallet _wallet;
        private bool _levelPassed;
        private bool _gameOver;
        private List<string> _finishedLevelNames = new ();
        private List<int> _finishedLevelStarsCount = new ();
        private LevelExit _levelExit;
        private Player.Player _player;

        public event Action LevelPassed;

        public event Action LevelFailed;

        public void Write(PlayerData playerData)
        {
            playerData.FinishedLevelNames = _finishedLevelNames;
            playerData.FinishedLevelsStarsCount = _finishedLevelStarsCount;
        }

        public void Read(PlayerData playerData)
        {
            _finishedLevelNames = playerData.FinishedLevelNames;
            _finishedLevelStarsCount = playerData.FinishedLevelsStarsCount;
        }

        public void Unsubscribe()
        {
            _levelExit.ExitOpened -= OnExitOpened;
            _player.Died -= OnDied;
        }

        public void Initialize(LevelExit levelExit, Player.Player player)
        {
            _levelExit = levelExit;
            _player = player;
            _levelExit.ExitOpened += OnExitOpened;
            _player.Died += OnDied;
        }

        private void SaveStarsCount()
        {
            if (_finishedLevelNames.Contains(SceneManager.GetActiveScene().name))
            {
                int index = _finishedLevelNames.IndexOf(SceneManager.GetActiveScene().name);

                if (_scoreDefiner.StarsCount > _finishedLevelStarsCount[index])
                    _finishedLevelStarsCount[index] = _scoreDefiner.StarsCount;

                return;
            }

            _finishedLevelNames.Add(SceneManager.GetActiveScene().name);
            _finishedLevelStarsCount.Add(_scoreDefiner.StarsCount);
        }

        private void CompleteLevel()
        {
            if (_gameOver || _levelPassed)
                return;

            _levelPassed = true;
            _gameboard.gameObject.SetActive(false);
            _scoreDefiner.RecieveStars(_stepCounter.CurrentStepCount);
            _wallet.AddStars(_scoreDefiner.StarsCount);
            SaveStarsCount();
            LevelPassed?.Invoke();
        }

        private void GameOver()
        {
            if (_gameOver || _levelPassed)
                return;

            _gameOver = true;
            _gameboard.gameObject.SetActive(false);
            LevelFailed?.Invoke();
        }

        private void OnExitOpened()
            => CompleteLevel();

        private void OnDied()
            => GameOver();
    }
}
