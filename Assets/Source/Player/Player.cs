using System;
using System.Collections;
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

    public event UnityAction StepEnded;

    private void OnDisable()
    {
        _mover.MoveEnded -= OnMoveEnded;
    }

    public void Initialize(Gameboard gameboard, Cell startCell)
    {
        _gameboard = gameboard;
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _input = GetComponent<PlayerInput>();
        _mover.Initialize(_startCell, _gameboard);
        _input.Initialize(_gameboard, _mover);
        _mover.MoveEnded += OnMoveEnded;
    }

    private void OnMoveEnded()
    {
        StepEnded?.Invoke();
    }
}
