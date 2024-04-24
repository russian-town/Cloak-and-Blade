using System;

public class GameOverView : ViewPanel
{
    private ILevelFinisher _levelFinisher;

    protected override void Unsubscribe()
        => _levelFinisher.LevelFailed -= OnLevelFailed;

    public void Initialize(ILevelFinisher levelFinisher)
    {
        _levelFinisher = levelFinisher;
        _levelFinisher.LevelFailed += OnLevelFailed;
    }

    private void OnLevelFailed()
        => Show();
}
