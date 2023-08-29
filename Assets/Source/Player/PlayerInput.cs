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
            Cell targetCell = _gameboard.GetCell(TouchRay);

            if(targetCell == null) 
            {
                return;
            }

            _mover.Move(targetCell);
        }
    }

    public void Initialize(Gameboard gameboard, PlayerMover playerMover)
    {
        _camera = Camera.main;
        _gameboard = gameboard;
        _mover = playerMover;
    }
}
