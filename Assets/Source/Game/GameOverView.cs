using Source.Player.PlayerUI;

namespace Source.Game
{
    public class GameOverView : ViewPanel
    {
        private ILevelFinisher _levelFinisher;

        public void Initialize(ILevelFinisher levelFinisher)
        {
            _levelFinisher = levelFinisher;
            _levelFinisher.LevelFailed += OnLevelFailed;
        }

        protected override void Unsubscribe()
            => _levelFinisher.LevelFailed -= OnLevelFailed;

        private void OnLevelFailed()
            => Show();
    }
}
