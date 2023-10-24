using System.Collections;
using UnityEngine.Events;

public class PlayerMover : Mover
{
    public event UnityAction MoveEnded;

    protected override IEnumerator MoveTo(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        yield return base.MoveTo(targetCell, moveSpeed, rotationSpeed);
        MoveEnded?.Invoke();
    }
}
