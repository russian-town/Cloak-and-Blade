using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySightHandler))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform _transform;
    [SerializeField] private EnemyPhrasePlayer _phrasePlayer;
    
    private Coroutine _moveCoroutine;
    private EnemySightHandler _sightHandler;
    private List<Cell> _cellsOnPath;
    private Cell _startCell;
    private Player _player;
    private Gameboard _gameBoard;
    private MusicPlayer _musicPlayer;
    private Cell _currentDestination;
    private Cell[] _destinations;
    private int _currentIndex;
    private int _currentDestinationIndex;
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

    public void Initialize(Cell[] destinations, Player player, Gameboard gameboard, MusicPlayer musicPlayer)
    {
        _sightHandler = GetComponent<EnemySightHandler>();
        _cellsOnPath = new List<Cell>();
        _destinations = destinations;
        _currentDestination = _destinations[1];
        _currentDestinationIndex = 1;
        _startCell = _destinations[0];
        _player = player;
        _player.StepEnded += OnStepEnded;
        _gameBoard = gameboard;
        _sightHandler.Initialize();
        _musicPlayer = musicPlayer; 
    }

    public void SetDestination(Cell destination) 
    {
        if (destination == null)
            return;

        _currentDestination = destination;
    }

    public void ClearDestination() => _currentDestination = null;

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

    private void OnStepEnded()
    {
        if (_currentDestination == null)
            return;

        if (_moveCoroutine == null)
            _moveCoroutine = StartCoroutine(PerformMove());
    }

    private void ChangeDestination(Cell destination)
    {
        _currentIndex = 0;

        if (destination == null)
            return;

        _currentDestination = destination;
        print(_currentDestinationIndex);
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

    private IEnumerator PerformMove()
    {
        CalculatePath();

        if (_cellsOnPath.Count == 0)
            yield break;

        #region RotateAndMove
        yield return RotateTowardsNextCell();

        if (_cellsOnPath.Count > 0)
            GenerateSight(_cellsOnPath[_currentIndex]);

        if (_sightHandler.TryFindPlayer(_player))
        {
            Debug.Log("Game over!");
            _phrasePlayer.StopRightThere();
            _musicPlayer.SwitchMusic();
        }

        yield return MoveToNextCell();
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
    }
}
