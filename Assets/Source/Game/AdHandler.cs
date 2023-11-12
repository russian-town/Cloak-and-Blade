public class AdHandler
{
    private Game _game;
    private FocusHandler _focusHandler;
    private Audio _audio;

    public AdHandler(Game game, FocusHandler focusHandler, Audio audio)
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
