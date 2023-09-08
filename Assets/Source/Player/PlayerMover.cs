using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Cell _startCell;
    private List<Cell> _availableCells = new List<Cell>();
    private Coroutine _startMoveCoroutine;

    public Cell CurrentCell { get; private set; }
    public IReadOnlyList<Cell> AvailableCells => _availableCells;

    public event UnityAction MoveEnded;

    public void Initialize(Cell startCell)
    {
        _startCell = startCell;
        AddAvailableCells();
    }

    public void Move(Cell targetCell)
    {
        if (_availableCells.Contains(targetCell))
        {
            if (_startMoveCoroutine == null && targetCell.Content.Type != CellContentType.Wall && targetCell.Content != null)
            {
                _startMoveCoroutine = StartCoroutine(StartMoveTo(targetCell));
            }
        }
    }

    private void AddAvailableCells()
    {
        if (_availableCells.Count > 0)
            _availableCells.Clear();

        _availableCells.AddRange(new[] { _startCell.South, _startCell.North, _startCell.West, _startCell.East });
    }

    private IEnumerator StartMoveTo(Cell targetCell)
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
        AddAvailableCells();
        _startMoveCoroutine = null;
        MoveEnded?.Invoke();
    }
}
