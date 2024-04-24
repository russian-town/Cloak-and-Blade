using UnityEngine;
using UnityEngine.EventSystems;

public class InputView : MonoBehaviour, IPauseHandler
{
    [SerializeField] private ParticleSystem _mouseOverCell;
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private AudioSource _source;

    private bool _isInitialized;
    private Camera _camera;
    private Cell _lastCell;
    private bool _isPause;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    private void Update()
    {
        if (_isInitialized == false || _isPause)
            return;

        Cell cell = _gameboard.GetCell(TouchRay);

        if (_lastCell != null && cell != _lastCell)
            _mouseOverCell.Stop();

        if (cell != null && cell.Content.Type != CellContentType.Wall)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                _source.Play();

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

    public void Unpause()
        => _isPause = false;

    public void Pause()
        => _isPause = true;
}
