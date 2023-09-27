using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySightHandler), typeof(EnemyMover), typeof(EnemyAnimationHandler))]
public class Enemy : Ghost, IPauseHandler
{
    [SerializeField] private Transform _transform;
    [SerializeField] private EnemyPhrasePlayer _phrasePlayer;

    private EnemySightHandler _sightHandler;
    private EnemyZoneDrawer _zoneDrawer;
    private EnemyMover _mover;
    private List<Cell> _cellsOnPath;
    private Cell _startCell;
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
    private bool _isFreeze;
    private TheWorld _theWorld;

    private void OnDisable() => _player.StepEnded -= UpdatePlayerStepCount;

    public void Initialize(Cell[] destinations, Player player, Gameboard gameboard, EnemyZoneDrawer enemyZoneDrawer)
    {
        _sightHandler = GetComponent<EnemySightHandler>();
        _animationHandler = GetComponent<EnemyAnimationHandler>();
        _mover = GetComponent<EnemyMover>();
        _mover.Initialize(_startCell, _animationHandler);
        _cellsOnPath = new List<Cell>();
        _destinations = destinations;
        _currentDestination = _destinations[1];
        _currentDestinationIndex = 1;
        _startCell = _destinations[0];
        _player = player;
        _gameBoard = gameboard;
        _zoneDrawer = enemyZoneDrawer;
        _sightHandler.Initialize(_zoneDrawer);
    }

    public void SetPause(bool isPause)
    {
        _mover.SetPause(isPause);

        if (isPause == true)
        {
            _animationHandler.StopAnimation();
        }
        else
        {
            _animationHandler.StartAnimation();
        }
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

    private void ChangeDestination(Cell destination)
    {
        _mover.CurrentCell.BecomeUnoccupied();
        _currentIndex = 0;

        if (destination == null)
            return;

        _currentDestination = destination;
    }

    private void CalculatePath()
    {
        if (_cellsOnPath.Count > 0 && _currentIndex == _cellsOnPath.Count)
        {
            if (_currentDestinationIndex < _destinations.Length - 1)
            {
                _currentDestinationIndex++;
                _startCell = _cellsOnPath[_currentIndex - 1];
                _currentIndex = 0;
            }
            else
            {
                _currentDestinationIndex = 0;
                _startCell = _cellsOnPath[_currentIndex - 1];
                _currentIndex = 0;
            }

            ChangeDestination(_destinations[_currentDestinationIndex]);
        }

        _gameBoard.GeneratePath(out _cellsOnPath, _currentDestination, _startCell);
    }

    public void TakeAbility(Ability ability)
    {
        if (ability == null)
            return;

        if (ability is TheWorld theWorld)
        {
            _isFreeze = true;
            _theWorld = theWorld;
        }

        _player.StepEnded += UpdatePlayerStepCount;
        Debug.Log("Attack");
    }

    public void UpdatePlayerStepCount()
    {
        Debug.Log(_theWorld.CurrentStepCount);

        if (_theWorld != null && _theWorld.CurrentStepCount >= _theWorld.MaxStepCount)
        {
            _isFreeze = false;
            _player.StepEnded -= UpdatePlayerStepCount;
            _theWorld = null;
        }
    }

    public IEnumerator PerformMove()
    {
        if (_isFreeze)
            yield break;

        _sightHandler.ClearSight();
        CalculatePath();

        if (_cellsOnPath.Count == 0)
            yield break;

        if (_cellsOnPath[_currentIndex] == null || _currentIndex == _cellsOnPath.Count)
            yield break;

        _mover.Move(_cellsOnPath[_currentIndex]);
        yield return _mover.StartMoveCoroutine;

        if (_cellsOnPath.Count > 0)
            GenerateSight(_cellsOnPath[_currentIndex]);

        if (_sightHandler.TryFindPlayer(_player))
        {
            _player.Die();
            _phrasePlayer.StopRightThere();
        }

        if (_currentIndex - 1 >= 0)
            _cellsOnPath[_currentIndex - 1].BecomeUnoccupied();

        _mover.CurrentCell.BecomeOccupied();
        _currentIndex++;
    }
}
