using System.Collections.Generic;
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

    public Cell CurrentCell => _mover.CurrentCell;

    public PlayerInput Input => _input;

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
        _mover.Initialize(_startCell);
        _input.Initialize(_gameboard, _mover, _mouseOverCell);
        _input.Disable();
        _mover.MoveEnded += OnMoveEnded;
    }

    public void EnableMove()
    {
        if (_input.enabled)
            _input.Disable();
        else
            _input.Enable();
    }

    public void PrepareAbility()
    {
        print("Casting");
    }

    public void SkipTurn() => OnMoveEnded();

    private void OnMoveEnded()
    {
        StepEnded?.Invoke();
    }
}
