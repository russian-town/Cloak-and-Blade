using System;

namespace Source.Game
{
    public interface ILevelFinisher
    {
        public event Action LevelPassed;

        public event Action LevelFailed;
    }
}
