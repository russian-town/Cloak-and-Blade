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
        float progress = 0f;

        while(progress < 1)
        {
            transform.localPosition = Vector3.LerpUnclamped(transform.localPosition, targetCell.transform.localPosition, progress);
            progress += Time.deltaTime / 2;
            yield return null;
        }

        CurrentCell = targetCell;
        _startCell = targetCell;
        StartMoveCoroutine = null;
        Debug.Log("Move ended!");
        MoveEnded?.Invoke();
    }
}
