using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Coroutine _startMove;

    public Cell CurrentCell {  get; private set; }

    public void Move(Cell targetCell, float duration = 1)
    {
        if (_startMove == null)
            _startMove = StartCoroutine(StartMove(targetCell, duration));
    }

    private IEnumerator StartMove(Cell targetCell, float duration)
    {
        float progress = 0f;

        while (progress < 1)
        {
            transform.localPosition = Vector3.LerpUnclamped(transform.localPosition, targetCell.transform.localPosition, progress);
            progress += Time.deltaTime / duration;
            yield return null;
        }

        CurrentCell = targetCell;
        _startMove = null;
    }
}
