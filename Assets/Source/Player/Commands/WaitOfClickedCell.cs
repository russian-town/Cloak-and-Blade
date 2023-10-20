using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitOfClickedCell : CustomYieldInstruction
{
    private PlayerInput _input;
    private Gameboard _gameboard;
    private Camera _camera;
    private Player _player;
    private Navigator _navigator;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);
    public Cell Cell { get; private set; }

    public WaitOfClickedCell(PlayerInput input, Gameboard gameboard, Camera camera, Player player, Navigator navigator)
    {
        _input = input;
        _gameboard = gameboard;
        _camera = camera;
        _player = player;
        _navigator = navigator;
    }

    public override bool keepWaiting
    {
        get
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cell targetCell = _gameboard.GetCell(TouchRay);

                if (_navigator.CanMoveToCell(targetCell))
                {
                    Cell = targetCell;
                    return false;
                }
            }

            return true;
        }
    }
}
