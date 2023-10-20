using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMover : Mover
{
    public event UnityAction MoveEnded;

    public override IEnumerator MoveTo(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        Debug.Log(moveSpeed);
        yield return base.MoveTo(targetCell, moveSpeed, rotationSpeed);
        MoveEnded?.Invoke();
    }
}
