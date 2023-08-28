using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Cell _destination;
    [SerializeField] private Queue<Cell> _searchFrontier = new Queue<Cell>();

    private Gameboard _board;
    private Cell _startCell;
    private Cell _wayPoint;
    private Player _player;
    private Coroutine _movingCoroutine;
    private Cell _currentDestination;

    private void OnDisable()
    {
        _player.StepEnded -= OnStepEnded;
    }

    public void Initialize(Cell startCell, Player player, Gameboard board)
    {
        _board = board; 
        _wayPoint = startCell;
        _startCell = startCell;
        _player = player;
        _player.StepEnded += OnStepEnded;
    }

    public bool FindPath()
    {
        if(_currentDestination == null && _destination != null)
        {
            _destination.Content.BecomeDestination();
            _currentDestination = _destination;
        }
        else if(_destination != null && _currentDestination != _destination)
        {
            _currentDestination.Content.BecomeEmpty();
            _destination.Content.BecomeDestination();
            _currentDestination = _destination;
        }

        foreach (var cell in _board.Cells)
        {
            if (cell.Content.Type == CellContentType.Destination)
            {
                cell.BecomeDestination();
                _searchFrontier.Enqueue(cell);
            }
            else
            {
                cell.ClearPath();
            }
        }

        while (_searchFrontier.Count > 0)
        {
            Cell cell = _searchFrontier.Dequeue();

            if (cell != null)
            {
                if (cell.IsAlternative)
                {
                    _searchFrontier.Enqueue(cell.GrowPathNorth());
                    _searchFrontier.Enqueue(cell.GrowPahtSouth());
                    _searchFrontier.Enqueue(cell.GrowPathEast());
                    _searchFrontier.Enqueue(cell.GrowPathWest());
                }
                else
                {
                    _searchFrontier.Enqueue(cell.GrowPathWest());
                    _searchFrontier.Enqueue(cell.GrowPathEast());
                    _searchFrontier.Enqueue(cell.GrowPahtSouth());
                    _searchFrontier.Enqueue(cell.GrowPathNorth());
                }
            }
        }

        foreach (var cell in _board.Cells)
        {
            cell.ShowPath();
        }

        return true;
    }

    private void OnStepEnded()
    {
        if (!FindPath())
            FindPath();

        if (_startCell.NextOnPath == null)
            _startCell = _wayPoint;

        if (_movingCoroutine == null && _destination != null)
            _movingCoroutine = StartCoroutine(StartMove());
    }

    private IEnumerator StartMove()
    {
        while(transform.localPosition != _startCell.NextOnPath.transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _startCell.NextOnPath.transform.localPosition, Time.deltaTime * _speed);
            yield return null;
        }

        _movingCoroutine = null;
        _startCell = _startCell.NextOnPath;
    }
}
