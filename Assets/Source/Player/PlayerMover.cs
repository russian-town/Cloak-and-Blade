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
    private Gameboard _gameboard;

    public event UnityAction MoveEnded;

    public void Initialize(Cell startCell, Gameboard gameboard)
    {
        _startCell = startCell;
        _gameboard = gameboard;
        AddAvailableCells();
    }

    private void AddAvailableCells()
    {
        if (_availableCells.Count > 0)
            _availableCells.Clear();

        _availableCells.AddRange(new[] { _startCell.South, _startCell.North, _startCell.West, _startCell.East });
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

    private IEnumerator StartMoveTo(Cell targetCell)
    {
        while(transform.localPosition != targetCell.transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetCell.transform.localPosition, Time.deltaTime * _speed);
            yield return null;
        }

        _startCell = targetCell;
        AddAvailableCells();
        _startMoveCoroutine = null;
        MoveEnded?.Invoke();
    }
}
