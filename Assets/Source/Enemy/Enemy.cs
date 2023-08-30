using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private List<Cell> _cellOnPath;
    [SerializeField] private Cell _destination;
    private Cell _startCell;
    private Player _player;
    private Gameboard _gameboar;
    private Cell _lastDestination;
    private int _currentIndex;

    private void OnDisable()
    {
        _player.StepEnded -= OnStepEnded;
    }

    public void Initialize(Cell startCell, Player player, Gameboard gameboard)
    {
        //_destination = startCell;
        _startCell = startCell;
        _player = player;
        _player.StepEnded += OnStepEnded;
        _gameboar = gameboard;
    }

    private void OnStepEnded()
    {
        if (_destination == null)
            return;

        if (_destination != _lastDestination && _lastDestination != null)
        {
            print("Destination changed");
            _startCell = _cellOnPath[_currentIndex - 1];
            _currentIndex = 0;
            _gameboar.GeneratePath(out _cellOnPath, _destination, _startCell);
        }
        else if (_cellOnPath.Count > 0 && _currentIndex == _cellOnPath.Count)
        {
            print("Reached destination");
            _destination = _startCell;
            _startCell = _cellOnPath[_currentIndex - 1];
            _currentIndex = 0;
            _gameboar.GeneratePath(out _cellOnPath, _destination, _startCell);
        }
        else
        {
            print("Common move towards destination");
            _gameboar.GeneratePath(out _cellOnPath, _destination, _startCell);
        }

        if (_cellOnPath.Count > 0)
            StartCoroutine(StartMove());

        _lastDestination = _destination;
    }

    private IEnumerator StartMove()
    {
        if (_cellOnPath[_currentIndex] == null)
            yield break;

        while (transform.localPosition != _cellOnPath[_currentIndex].transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _cellOnPath[_currentIndex].transform.localPosition, Time.deltaTime * _speed);
            yield return null;
        }

        _currentIndex++;
    }
}
