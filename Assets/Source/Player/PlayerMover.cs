using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Cell _startCell;
    private Coroutine _startMoveCoroutine;

    public Coroutine StartMoveCoroutine => _startMoveCoroutine;

    public Cell CurrentCell { get; private set; }

    public event UnityAction MoveEnded;

    public void Initialize(Cell startCell)
    {
        _startCell = startCell;
        CurrentCell = _startCell;
    }

    public void Move(Cell targetCell)
    {
        if (_startMoveCoroutine == null)
            _startMoveCoroutine = StartCoroutine(StartMoveTo(targetCell));
    }

    public IEnumerator StartMoveTo(Cell targetCell)
    {
        float progress = 0f;

        while(progress < 1)
        {
            transform.localPosition = Vector3.LerpUnclamped(transform.localPosition, targetCell.transform.localPosition, progress);
            progress += Time.deltaTime;
            yield return null;
        }

        _startCell = targetCell;
        CurrentCell = targetCell;
        _startMoveCoroutine = null;
        MoveEnded?.Invoke();
    }
}
