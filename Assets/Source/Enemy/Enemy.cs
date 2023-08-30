using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private List<Cell> _cellsOnPath;
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
            _startCell = _cellsOnPath[_currentIndex - 1];
            _currentIndex = 0;
        }
        else if (_cellsOnPath.Count > 0 && _currentIndex == _cellsOnPath.Count)
        {
            print("Reached destination");
            _destination = _startCell;
            _startCell = _cellsOnPath[_currentIndex - 1];
            _currentIndex = 0;
        }

        _gameboar.GeneratePath(out _cellsOnPath,  _destination, _startCell);

        if (_cellsOnPath.Count > 0)
        {
            print("Startingmove");
            StartCoroutine(StartMove());
        }

        _lastDestination = _destination;
    }

    private IEnumerator StartMove()
    {
        if (_cellsOnPath[_currentIndex] == null)
            yield break;

        Vector3 rotationTarget = _cellsOnPath[_currentIndex].transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationTarget, Vector3.up);

        while (transform.rotation != targetRotation)
        {
            print("Rotating");
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            yield return null;
        }

        while (transform.localPosition != _cellsOnPath[_currentIndex].transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _cellsOnPath[_currentIndex].transform.localPosition, Time.deltaTime * _movementSpeed);
            yield return null;
        }

        _currentIndex++;
    }
}
