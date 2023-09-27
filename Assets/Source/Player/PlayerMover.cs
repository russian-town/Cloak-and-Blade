using System.Collections;
using UnityEngine.Events;

public class PlayerMover : Mover
{
    public event UnityAction MoveEnded;

    protected override IEnumerator StartMoveTo(Cell targetCell)
    {
        yield return base.StartMoveTo(targetCell);
        MoveEnded?.Invoke();
    }
}
