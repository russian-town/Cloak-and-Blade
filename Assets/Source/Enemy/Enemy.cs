using System.Collections;
using Source.Gameboard.Cell;
using Source.Pause;
using Source.Root;
using UnityEngine;

namespace Source.Enemy
{
    [RequireComponent(typeof(EnemySightHandler), typeof(EnemyMover), typeof(EnemyAnimationHandler))]
    public class Enemy : Ghost.Ghost, IPauseHandler
    {
        private readonly int _north = 0;
        private readonly int _fakeNorth = 360;
        private readonly int _east = 90;
        private readonly int _south = 180;
        private readonly int _west = 270;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private Transform _transform;
        [SerializeField] private ParticleSystem _freezeEffect;
        [SerializeField] private Transform _announcer;
        [SerializeField] private ParticleSystem _announcerNorth;
        [SerializeField] private ParticleSystem _announcerSouth;
        [SerializeField] private ParticleSystem _announcerEast;
        [SerializeField] private ParticleSystem _announcerWest;

        private EnemySightHandler _sightHandler;
        private EnemyZoneDrawer _zoneDrawer;
        private EnemyMover _mover;
        private Cell _startCell;
        private Cell _nextCell;
        private Cell _previousCell;
        private Cell _nextDeclaredCell;
        private Cell _previousDestination;
        private Player.Player _player;
        private EnemyAnimationHandler _animationHandler;
        private Gameboard.Gameboard _gameBoard;
        private Cell _currentDestination;
        private Cell[] _destinations;
        private ParticleSystem _previousAnnouncer;
        private int _currentDestinationIndex;
        private bool _isBlind;

        public bool IsFrozen { get; private set; }

        public void Initialize(Cell[] destinations, Player.Player player, Gameboard.Gameboard gameboard, EnemyZoneDrawer enemyZoneDrawer)
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
            AnnouncerDerection();
        }

        public Cell DeclareNextCell()
        {
            CalculatePath();
            return _nextDeclaredCell;
        }

        public void Freeze()
        {
            IsFrozen = true;
            _freezeEffect.Play();
        }

        public void UnFreeze()
            => IsFrozen = false;

        public void Blind()
            => _isBlind = true;

        public void UnBlind()
            => _isBlind = false;

        public Coroutine StartPerformMove()
        {
            return StartCoroutine(PerformMove());
        }

        public void Die()
        {
            _sightHandler.ClearSight();
            Destroy(gameObject);
        }

        public void Unpause()
        {
            _mover.Unpause();
            _animationHandler.StartAnimation();
        }

        public void Pause()
        {
            _mover.Pause();
            _animationHandler.StopAnimation();
        }

        private bool CalculatePath()
        {
            if (_gameBoard.FindPath(_currentDestination, ref _nextDeclaredCell, _mover.CurrentCell))
            {
                return true;
            }
            else
            {
                ChangeDestination();
                return false;
            }
        }

        private IEnumerator PerformMove()
        {
            _sightHandler.ClearSight();
            _nextDeclaredCell.BecomeOccupied();

            if (_previousDestination != null)
            {
                _previousDestination.BecomeUnoccupied();
                _previousDestination = null;
            }

            if (_previousAnnouncer != null)
            {
                _previousAnnouncer.Stop();
                _previousAnnouncer.gameObject.SetActive(false);
                _previousAnnouncer = null;
            }

            _previousCell = _mover.CurrentCell;
            _nextCell = _nextDeclaredCell;

            if (_nextCell == _player.CurrentCell && _isBlind == true)
            {
                DeclareNextCell();
                _nextCell = _nextDeclaredCell;
            }

            if (_nextCell == _player.CurrentCell && _isBlind == false)
            {
                yield return _mover.StartRotate(_nextCell, _rotationSpeed);
                _player.Die();
                yield break;
            }

            if (_nextDeclaredCell == _destinations[_currentDestinationIndex])
            {
                _nextDeclaredCell.BecomeOccupied();
                _previousDestination = _destinations[_currentDestinationIndex];
            }

            yield return _mover.StartMoveTo(_nextCell, _moveSpeed, _rotationSpeed);

            if (_nextCell != null)
                GenerateSight(_nextCell);

            if (_sightHandler.TryFindPlayer(_player) && _isBlind == false)
                _player.Die();

            if (_previousCell != null)
                _previousCell.BecomeUnoccupied();

            AnnouncerDerection();
        }

        private void AnnouncerDerection()
        {
            Cell cell = DeclareNextCell();
            _announcer.rotation = Quaternion.Euler(Vector3.zero);

            if (cell == _mover.CurrentCell.North)
            {
                _announcerNorth.gameObject.SetActive(true);
                _announcerNorth.Play();
                _previousAnnouncer = _announcerNorth;
            }

            if (cell == _mover.CurrentCell.South)
            {
                _announcerSouth.gameObject.SetActive(true);
                _announcerSouth.Play();
                _previousAnnouncer = _announcerSouth;
            }

            if (cell == _mover.CurrentCell.West)
            {
                _announcerWest.gameObject.SetActive(true);
                _announcerWest.Play();
                _previousAnnouncer = _announcerWest;
            }

            if (cell == _mover.CurrentCell.East)
            {
                _announcerEast.gameObject.SetActive(true);
                _announcerEast.Play();
                _previousAnnouncer = _announcerEast;
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

        private void ChangeDestination()
        {
            _currentDestinationIndex++;

            if (_currentDestinationIndex > _destinations.Length - 1)
                _currentDestinationIndex = 0;

            _currentDestination = _destinations[_currentDestinationIndex];
            DeclareNextCell();
        }
    }
}
