using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera;
    private Gameboard _gameboard;
    private PlayerMover _mover;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("POop");
            Cell targetCell = _gameboard.GetCell(TouchRay);

            if(targetCell == null) 
            {
                print("No target cell");
                return;
            }

            _mover.Move(targetCell);
            print("Moving");
        }
    }

    public void Initialize(Gameboard gameboard, PlayerMover playerMover)
    {
        _camera = Camera.main;
        _gameboard = gameboard;
        _mover = playerMover;
    }
}
