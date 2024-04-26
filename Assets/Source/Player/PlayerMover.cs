using System.Collections;
using Source.Gameboard.Cell;
using Source.Ghost.Mover;
using UnityEngine.Events;

namespace Source.Player
{
    public class PlayerMover : Mover
    {
        public event UnityAction MoveEnded;

        protected override IEnumerator MoveTo(Cell targetCell, float moveSpeed, float rotationSpeed)
        {
            yield return base.MoveTo(targetCell, moveSpeed, rotationSpeed);
            MoveEnded?.Invoke();
        }
    }
}
