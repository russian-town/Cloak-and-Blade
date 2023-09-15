using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Cell _startCell;

    public Coroutine StartMoveCoroutine { get; private set; }
    public Cell CurrentCell { get; private set; }

    public event UnityAction MoveEnded;

    public void Initialize(Cell startCell)
    {
        _startCell = startCell;
        CurrentCell = _startCell;
    }

    public void Move(Cell targetCell)
    {
        if (StartMoveCoroutine == null)
            StartMoveCoroutine = StartCoroutine(StartMoveTo(targetCell));
    }

    public IEnumerator StartMoveTo(Cell targetCell)
    {
        while (transform.localPosition != targetCell.transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetCell.transform.localPosition, Time.deltaTime * _speed);
            yield return null;
        }

        CurrentCell = targetCell;
        _startCell = targetCell;
        StartMoveCoroutine = null;
        MoveEnded?.Invoke();
    }
}
