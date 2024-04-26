using UnityEngine;

namespace Source.Game
{
    public class GameSpeedChanger : MonoBehaviour
    {
        [SerializeField] private float _speedMultiplier;

        private bool _isSpeedUp;

        private void OnDisable()
        {
            Time.timeScale = 1;
        }

        public void ChangeGameSpeed()
        {
            if (_isSpeedUp)
            {
                Time.timeScale = 1;
                _isSpeedUp = false;
            }
            else
            {
                Time.timeScale = _speedMultiplier;
                _isSpeedUp = true;
            }
        }
    }
}
