using UnityEngine;

namespace Source.Player.PlayerUI
{
    [RequireComponent(typeof(MainMenuVeiw))]
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private LevelsViewScroll _levelsViewScroll;

        private MainMenuVeiw _mainMenuVeiw;

        private void OnDisable() => _mainMenuVeiw.PlayButtonClicked -= StartGame;

        public void Initialize()
        {
            _mainMenuVeiw = GetComponent<MainMenuVeiw>();
            _mainMenuVeiw.PlayButtonClicked += StartGame;
            _levelsViewScroll.Hide();
        }

        private void StartGame() => _levelsViewScroll.Show();
    }
}
