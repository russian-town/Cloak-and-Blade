using System;

public interface ILevelFinisher
{
    public event Action LevelPassed;

    public event Action LevelFailed;
}
