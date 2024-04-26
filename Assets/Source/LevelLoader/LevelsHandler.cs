using System.Collections.Generic;
using Source.Player.PlayerUI.Level;
using Source.Root;
using Source.Saves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Source.LevelLoader
{
    public class LevelsHandler : MonoBehaviour, IDataReader
    {
        [SerializeField] private List<Level> _levels = new ();

        private bool _tutorialCompleted;

        public Level GetNextLevel()
        {
            for (int i = 0; i < _levels.Count; i++)
            {
                if (_levels[i].Name == SceneManager.GetActiveScene().name)
                {
                    if (i + 1 < _levels.Count)
                        return _levels[i + 1];
                    else
                        return _levels[0];
                }
            }

            return null;
        }

        public bool TryLoadTutorial()
        {
            if (_tutorialCompleted == false)
            {
                SceneManager.LoadScene(Constants.Tutorial);
                return true;
            }
            else
            {
                SceneManager.LoadScene(Constants.MainMenu);
                return false;
            }
        }

        public void Read(PlayerData playerData)
            => _tutorialCompleted = playerData.IsTutorialCompleted;
    }
}
