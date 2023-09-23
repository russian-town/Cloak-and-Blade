using UnityEngine;

public class InputView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _mouseOverCell;
    [SerializeField] private Gameboard _gameboard;

    private bool _isInitialized;
    private Camera _camera;
    private Cell _lastCell;

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
    }

    public void Initialize()
    {
        _camera = Camera.main;
        _isInitialized = true;
    }
}
