using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera;
    private Gameboard _gameboard;
    private Cell _lastCell;
    private ParticleSystem _mouseOverCell;
    private Player _player;
    private bool _isInitialized;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private void Update()
    {
        if (_isInitialized == false)
            return;

        Cell cell = _gameboard.GetCell(TouchRay);

        if (_lastCell != null && cell != _lastCell)
            _mouseOverCell.Stop();

        if (cell != null && cell.Content.Type != CellContentType.Wall)
        {
            _mouseOverCell.transform.position = cell.transform.position;
            _mouseOverCell.Play();
            _lastCell = cell;
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Cell targetCell = _gameboard.GetCell(TouchRay);

        //    if (targetCell == null)
        //        return;

        //    _player.ExecuteCurrentCommand(targetCell);
        //}
    }

    public void Initialize(Camera camera, Gameboard gameboard, ParticleSystem mouseOverCell, Player player)
    {
        _camera = camera;
        _mouseOverCell = mouseOverCell;
        _gameboard = gameboard;
        _player = player;
        _isInitialized = true;
    }
}
