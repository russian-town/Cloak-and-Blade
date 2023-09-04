using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(EnemySightHandler))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Cell _destination;
    [SerializeField] private Transform _transform;

    private Coroutine _moveCoroutine;
    private EnemySightHandler _sightHandler;
    private List<Cell> _cellsOnPath;
    private Cell _startCell;
    private Cell _currentCell;
    private Cell _firstStartCell;
    private Player _player;
    private Gameboard _gameBoard;
    private Cell _lastDestination;
    private int _currentIndex;
    private int _north = 0;
    private int _fakeNorth = 360;
    private int _east = 90;
    private int _south = 180;
    private int _west = 270;
    private bool _isPlayerDetected;

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
        _firstStartCell = _startCell;
        _player = player;
        _player.StepEnded += OnStepEnded;
        _gameBoard = gameboard;
        _sightHandler.Initialize();
    }

    public void SetDestination(Cell destination) 
    {
        if (destination == null)
            return;

        _destination = destination;
    }

s    private void GenerateSight(Cell currentCell)
    {
        if ((int)Mathf.Round(transform.rotation.eulerAngles.y) == _north || (int)Mathf.Round(transform.rotation.eulerAngles.y) == _fakeNorth)
            _sightHandler.GenerateSight(currentCell, Constants.North);

        else if ((int)Mathf.Round(transform.rotation.eulerAngles.y) == _east)
            _sightHandler.GenerateSight(currentCell, Constants.East);

        else if ((int)Mathf.Round(transform.rotation.eulerAngles.y) == _south)
            _sightHandler.GenerateSight(currentCell, Constants.South);

        else if ((int)Mathf.Round(transform.rotation.eulerAngles.y) == _west)
            _sightHandler.GenerateSight(currentCell, Constants.West);
    }

    public void ClearDestination() => _destination = null;

    private void OnStepEnded()
    {
        if (_destination == null)
            return;

        if (_moveCoroutine == null)
            _moveCoroutine = StartCoroutine(PerformMove());
    }

    private void CalculatePath()
    {
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

        _gameBoard.GeneratePath(out _cellsOnPath, _destination, _startCell);

        _lastDestination = _destination;
    }

    private void DefineChaseState()
    {
        if (_sightHandler.TryFindPlayer(_player) && !_isPlayerDetected)
        {
            print("What was that?");
            SetDestination(_player.CurrentCell);
            _isPlayerDetected = true;
            CalculatePath();
            return;
        }
        else if (!_sightHandler.TryFindPlayer(_player) && _isPlayerDetected)
        {
            print("Must've been the wind");
            SetDestination(_firstStartCell);
            _isPlayerDetected = false;
            CalculatePath();
            return;
        }
        else if (_sightHandler.TryFindPlayer(_player) && _isPlayerDetected)
        {
            print("Stop right there!");
            SetDestination(_player.CurrentCell);
            CalculatePath();
            return;
        }
    }

    private IEnumerator PerformMove()
    {
        CalculatePath();

        if (_cellsOnPath.Count == 0)
            yield break;

        #region RotateAndMove
        yield return RotateTowardsNextCell();

        yield return MoveToNextCell();
        #endregion

        #region Sideye
        if (_cellsOnPath.Count > 0)
            GenerateSight(_cellsOnPath[_currentIndex]);

        DefineChaseState();
        #endregion

        _moveCoroutine = null;
        _currentIndex++;
    }

    private IEnumerator RotateTowardsNextCell()
    {
        Vector3 rotationTarget = _cellsOnPath[_currentIndex].transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(rotationTarget, Vector3.up);

        if (transform.rotation != targetRotation)
            _sightHandler.ClearSight();

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator MoveToNextCell()
    {
        if (_cellsOnPath[_currentIndex] == null || _currentIndex == _cellsOnPath.Count)
            yield break;
     
        while (transform.localPosition != _cellsOnPath[_currentIndex].transform.localPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, _cellsOnPath[_currentIndex].transform.localPosition, Time.deltaTime * _movementSpeed);
            yield return null;
        }

        _currentCell = _cellsOnPath[_currentIndex];

        if (_player.AvailableCells.Count > 0 && _player.AvailableCells.Contains(_currentCell))
            Debug.Log("Game over!");
    }
}
