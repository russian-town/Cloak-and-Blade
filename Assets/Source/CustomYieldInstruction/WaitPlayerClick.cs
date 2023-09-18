using UnityEngine;

public class WaitPlayerClick : CustomYieldInstruction
{
    private Gameboard _gameboard;
    private Camera _camera;

    public Cell Cell { get; private set; }
    private Ray Ray => _camera.ScreenPointToRay(Input.mousePosition);

    public WaitPlayerClick(Gameboard gameboard)
    {
        _gameboard = gameboard;
        _camera = Camera.main;
    }

    public override bool keepWaiting
    {
        get
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cell targetCell = _gameboard.GetCell(Ray);;

                if (targetCell != null)
                {
                    Cell = targetCell;
                    return false;
                }
            }

            return true;
        }
    }
}
