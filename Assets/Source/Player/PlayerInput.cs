using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera;
    private Gameboard _gameboard;
    private Cell _startCell;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private void HandleTouch()
    {
        Cell targetCell = _gameboard.GetCell(TouchRay);

        if (targetCell == _startCell.North || targetCell == _startCell.South || targetCell == _startCell.East || targetCell == _startCell.West)
        {

        }
    }
}
