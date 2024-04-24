using UnityEngine;
using UnityEngine.EventSystems;

public class WaitOfClickedCell : CustomYieldInstruction
{
    private readonly Gameboard _gameboard;
    private readonly Camera _camera;
    private readonly Navigator _navigator;

    public WaitOfClickedCell(Gameboard gameboard, Camera camera, Navigator navigator)
    {
        _gameboard = gameboard;
        _camera = camera;
        _navigator = navigator;
    }

    public Cell Cell { get; private set; }

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);
    
    public override bool keepWaiting
    {
        get
        {
            return TryGetTargetCell();
        }
    }

    private bool TryGetTargetCell()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Cell targetCell = _gameboard.GetCell(TouchRay);

            if (_navigator.CanMoveToCell(ref targetCell))
            {
                Cell = targetCell;
                return false;
            }
        }

        return true;
    }
}
