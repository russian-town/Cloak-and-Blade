using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera;
    private Gameboard _gameboard;
    private PlayerMover _mover;
    private Cell _lastCell;
    private ParticleSystem _mouseOverCell;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    public event UnityAction<Cell> CellClicked;

    private void Update()
    {
        Cell cell = _gameboard.GetCell(TouchRay);

        if (_lastCell != null && cell != _lastCell)
        {
            _mouseOverCell.Stop();
        }

        if (cell != null && cell.Content.Type != CellContentType.Wall)
        {
            _mouseOverCell.transform.position = cell.transform.position;
            _mouseOverCell.Play();
            _lastCell = cell;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cell targetCell = _gameboard.GetCell(TouchRay);
            CellClicked?.Invoke(targetCell);

            if(targetCell == null) 
                return;

            _mover.Move(targetCell);
        }
    }

    public void Initialize(Gameboard gameboard, PlayerMover playerMover, ParticleSystem mouseOverCell)
    {
        _camera = Camera.main;
        _mouseOverCell = mouseOverCell;
        _gameboard = gameboard;
        _mover = playerMover;
    }

    public void Enable()
    {
        enabled = true;
        Debug.Log("Enable");
    }

    public void Disable()
    {
        enabled = false;
        Debug.Log("Disable");
    }
}
