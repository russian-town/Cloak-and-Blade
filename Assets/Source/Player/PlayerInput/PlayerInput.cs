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

            if (targetCell == _player.CurrentCell.North.North && _player.CurrentCell.North.HasTrap)
                _player.ExecuteCurrentCommand(_player.CurrentCell.North);
            else if (targetCell == _player.CurrentCell.South.South && _player.CurrentCell.South.HasTrap)
                _player.ExecuteCurrentCommand(_player.CurrentCell.South);
            else if (targetCell == _player.CurrentCell.East.East && _player.CurrentCell.East.HasTrap)
                _player.ExecuteCurrentCommand(_player.CurrentCell.East);
            else if (targetCell == _player.CurrentCell.West.West && _player.CurrentCell.West.HasTrap)
                _player.ExecuteCurrentCommand(_player.CurrentCell.West);
            else
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
