using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitOfClickedCell : CustomYieldInstruction
{
    private Gameboard _gameboard;
    private Camera _camera;
    private Navigator _navigator;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);
    public Cell Cell { get; private set; }

    public WaitOfClickedCell(Gameboard gameboard, Camera camera, Navigator navigator)
    {
        _gameboard = gameboard;
        _camera = camera;
        _navigator = navigator;
    }

    public override bool keepWaiting
    {
        get
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cell targetCell = _gameboard.GetCell(TouchRay);
                Debug.Log("Click");

                if (_navigator.CanMoveToCell(ref targetCell))
                {
                    Debug.Log("Can move.");
                    Cell = targetCell;
                    return false;
                }
            }

            return true;
        }
    }
}
