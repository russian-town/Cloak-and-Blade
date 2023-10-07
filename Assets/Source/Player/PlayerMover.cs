using System.Collections;
using UnityEngine.Events;

public class PlayerMover : Mover
{
    private bool _timeFrize;

    public event UnityAction MoveEnded;

    protected override IEnumerator StartMoveTo(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        yield return base.StartMoveTo(targetCell, moveSpeed, rotationSpeed);
        MoveEnded?.Invoke();
    }
}
