using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private List<Cell> _cellOnPath;
    [SerializeField] private List<Quaternion> _targetRotations;
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
            _startCell = _cellOnPath[_currentIndex];
            transform.localRotation = _targetRotations[_currentIndex];
            _currentIndex = 0;
        }

        if (_cellOnPath != null)
        {
            _cellOnPath.Clear();
            _targetRotations.Clear();
        }

        _gameboar.GeneratePath(out _cellOnPath, out _targetRotations, _destination, _startCell);

        if (_currentIndex == _cellOnPath.Count)
        {
            _destination = _startCell;
            _startCell = _cellOnPath[_currentIndex - 1];
            transform.localRotation = _targetRotations[_currentIndex - 1];
            _currentIndex = 0;
            _gameboar.GeneratePath(out _cellOnPath, out _targetRotations, _destination, _startCell);
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
            transform.localRotation = _targetRotations[_currentIndex];
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _cellOnPath[_currentIndex].transform.localPosition, Time.deltaTime * _moveSpeed);
            yield return null;
        }

        _currentIndex++;
    }
}
