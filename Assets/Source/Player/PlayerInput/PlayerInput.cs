using UnityEngine;

[RequireComponent (typeof(InputView))]
public class PlayerInput : MonoBehaviour
{
    private Camera _camera;
    private Gameboard _gameboard;
    private Player _player;
    private InputView _inputView;

    private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

    public void GameUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cell targetCell = _gameboard.GetCell(TouchRay);

            if (targetCell == null)
                return;

            _player.ExecuteCurrentCommand(targetCell);
        }
    }

    public void Initialize(Camera camera, Gameboard gameboard, Player player)
    {
        _inputView = GetComponent<InputView>();
        _camera = camera;
        _gameboard = gameboard;
        _player = player;
        _inputView.Initialize();
    }
}
