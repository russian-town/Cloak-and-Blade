using Source.Sound_and_music;

namespace Source.Game
{
    public class AdHandler
    {
        private readonly Game _game;
        private readonly FocusHandler.FocusHandler _focusHandler;
        private readonly Audio _audio;

        public AdHandler(Game game, FocusHandler.FocusHandler focusHandler, Audio audio)
        {
            _game = game;
            _focusHandler = focusHandler;
            _audio = audio;
        }

        public void OpenAd()
        {
            _focusHandler.enabled = false;
            _game.SetPause();
            _audio.Mute();
        }

        public void CloseAd()
        {
            _audio.UnMute();
            _focusHandler.enabled = true;
        }
    }
}
