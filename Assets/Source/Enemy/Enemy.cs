using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySightHandler), typeof(EnemyMover), typeof(EnemyAnimationHandler))]
public class Enemy : Ghost, IPauseHandler
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _transform;
    [SerializeField] private ParticleSystem _freezeEffect;

    private EnemySightHandler _sightHandler;
    private EnemyZoneDrawer _zoneDrawer;
    private EnemyMover _mover;
    private Cell _startCell;
    private Cell _nextCell;
    private Cell _previousCell;
    private Player _player;
    private EnemyAnimationHandler _animationHandler;
    private Gameboard _gameBoard;
    private Cell _currentDestination;
    private Cell[] _destinations;
    private int _currentIndex;
    private int _currentDestinationIndex;
    private int _north = 0;
    private int _fakeNorth = 360;
    private int _east = 90;
    private int _south = 180;
    private int _west = 270;
    private bool _isBlind;

    public bool IsFreeze { get; private set; }

    public void Initialize(Cell[] destinations, Player player, Gameboard gameboard, EnemyZoneDrawer enemyZoneDrawer)
    {
        _sightHandler = GetComponent<EnemySightHandler>();
        _animationHandler = GetComponent<EnemyAnimationHandler>();
        _mover = GetComponent<EnemyMover>();
        _destinations = destinations;
        _currentDestination = _destinations[1];
        _currentDestinationIndex = 1;
        _startCell = _destinations[0];
        _mover.Initialize(_startCell, _animationHandler);
        _player = player;
        _gameBoard = gameboard;
        _zoneDrawer = enemyZoneDrawer;
        _sightHandler.Initialize(_zoneDrawer);
    }

    public void SetPause(bool isPause)
    {
        _mover.SetPause(isPause);

        if (isPause == true)
            _animationHandler.StopAnimation();
        else
            _animationHandler.StartAnimation();
    }

    public void Freeze()
    {
        IsFreeze = true;
        _freezeEffect.Play();
    }

    public void UnFreeze() => IsFreeze = false;

    public void Blind() => _isBlind = true;

    public void UnBlind() => _isBlind = false;

    public Coroutine StartPerformMove()
    {
        return StartCoroutine(PerformMove());
    }

    private IEnumerator PerformMove()
    {
        _sightHandler.ClearSight();
        _previousCell = _mover.CurrentCell;

        if (!CalculatePath())
            yield return new WaitUntil(() => CalculatePath());

        if (_nextCell == _player.CurrentCell && _isBlind == false)
        {
            yield return _mover.StartRotate(_nextCell, _rotationSpeed);
            _player.Die();
            yield break;
        }

        yield return _mover.StartMoveTo(_nextCell, _moveSpeed, _rotationSpeed);

        if (_nextCell != null)
            GenerateSight(_nextCell);

        if (_sightHandler.TryFindPlayer(_player) && _isBlind == false)
        {
            _player.Die();
        }

        if (_previousCell != null)
            _previousCell.BecomeUnoccupied();

        _mover.CurrentCell.BecomeOccupied();
        _currentIndex++;
    }

    private void GenerateSight(Cell currentCell)
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

    private void ChangeDestination()
    {
        _mover.CurrentCell.BecomeUnoccupied();
        _currentDestinationIndex++;

        if(_currentDestinationIndex > _destinations.Length - 1)
            _currentDestinationIndex = 0;

        _currentDestination = _destinations[_currentDestinationIndex];
    }

    public bool CalculatePath()
    {
        if (_gameBoard.FindPath(_currentDestination, ref _nextCell, _mover.CurrentCell))
        {
            return true;
        }
        else
        {
            ChangeDestination();
            return false;
        }
    }
}
