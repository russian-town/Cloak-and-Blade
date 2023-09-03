using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover), typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    private PlayerMover _mover;
    private PlayerInput _input;
    private Gameboard _gameboard;
    private Cell _startCell;
    private ParticleSystem _mouseOverCell;

    public event UnityAction StepEnded;

    private void OnDisable()
    {
        _mover.MoveEnded -= OnMoveEnded;
    }

    public void Initialize(Gameboard gameboard, Cell startCell, ParticleSystem mouseOverCell)
    {
        _mouseOverCell = mouseOverCell;
        _gameboard = gameboard;
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _input = GetComponent<PlayerInput>();
        _mover.Initialize(_startCell, _gameboard);
        _input.Initialize(_gameboard, _mover, _mouseOverCell);
        _mover.MoveEnded += OnMoveEnded;
    }

    private void OnMoveEnded()
    {
        StepEnded?.Invoke();
    }
}
