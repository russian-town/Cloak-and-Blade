using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySightHandler))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Cell _destination;
    [SerializeField] private Transform _transform;

    private EnemySightHandler _sightHandler;
    private List<Cell> _cellsOnPath;
    private Cell _startCell;
    private Player _player;
    private Gameboard _gameBoard;
    private Cell _lastDestination;
    private int _currentIndex;
    private int _north = 0;
    private int _fakeNorth = 360;
    private int _east = 90;
    private int _south = 180;
    private int _west = 270;

    public Gameboard Gameboard => _gameBoard;

    private void OnDisable()
    {
        _player.StepEnded -= OnStepEnded;
    }

    public void Initialize(Cell startCell, Player player, Gameboard gameboard)
    {
        _sightHandler = GetComponent<EnemySightHandler>();
        _cellsOnPath = new List<Cell>();
        _startCell = startCell;
        _player = player;
        _player.StepEnded += OnStepEnded;
        _gameBoard = gameboard;
        _sightHandler.Initialize();
    }

    private void GenerateSight(Cell currentCell)
    {
        if ((int)Mathf.Round(transform.rotation.eulerAngles.y) == _north || (int)Mathf.Round(transform.rotation.eulerAngles.y) == _fakeNorth)
        {
            _sightHandler.GenerateSight(currentCell, Constants.North);
        }
        else if ((int)Mathf.Round(transform.rotation.eulerAngles.y) == _east)
        {
            _sightHandler.GenerateSight(currentCell, Constants.East);
        }
        else if ((int)Mathf.Round(transform.rotation.eulerAngles.y) == _south)
        {
            _sightHandler.GenerateSight(currentCell, Constants.South);
        }
        else if ((int)Mathf.Round(transform.rotation.eulerAngles.y) == _west)
        {
            _sightHandler.GenerateSight(currentCell, Constants.West);
        }
    }

    public void ClearDestination() => _destination = null;

    private void OnStepEnded()
    {
        if (_destination == null)
            return;

        if (_destination != _lastDestination && _lastDestination != null && _cellsOnPath.Count > 0)
        {
            _startCell = _cellsOnPath[_currentIndex - 1];
            _currentIndex = 0;
        }
        else if (_cellsOnPath.Count > 0 && _currentIndex == _cellsOnPath.Count)
        {
            _destination = _startCell;
            _startCell = _cellsOnPath[_currentIndex - 1];
            _currentIndex = 0;
        }

        _gameBoard.GeneratePath(out _cellsOnPath,  _destination, _startCell);

        if (_cellsOnPath.Count > 0)
        {
            StartCoroutine(StartMove());
        }

        _lastDestination = _destination;
    }

    private IEnumerator StartMove()
    {
        if (_cellsOnPath[_currentIndex] == null)
            yield break;

        Vector3 rotationTarget = _cellsOnPath[_currentIndex ].transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationTarget, Vector3.up);

        if(transform.rotation != targetRotation)
            _sightHandler.ClearSight();

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            yield return null;
        }

        print(_cellsOnPath[_currentIndex]);

        if (_cellsOnPath.Count > 0)
            GenerateSight(_cellsOnPath[_currentIndex]);

        while (transform.localPosition != _cellsOnPath[_currentIndex].transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _cellsOnPath[_currentIndex].transform.localPosition, Time.deltaTime * _movementSpeed);
            yield return null;
        }

        _currentIndex++;
    }
}
