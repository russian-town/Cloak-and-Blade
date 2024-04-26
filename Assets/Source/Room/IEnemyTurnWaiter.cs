using UnityEngine;

namespace Source.Room
{
    public interface IEnemyTurnWaiter
    {
        public Coroutine WaitForEnemies();
    }
}
