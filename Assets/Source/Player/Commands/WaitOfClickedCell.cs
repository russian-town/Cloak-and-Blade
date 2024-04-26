using Source.Gameboard.Cell;
using Source.Gameboard.Cell.CellContent;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Source.Player.Commands
{
    public class WaitOfClickedCell : CustomYieldInstruction
    {
        private readonly Gameboard.Gameboard _gameboard;
        private readonly UnityEngine.Camera _camera;
        private readonly Navigator.Navigator _navigator;

        public WaitOfClickedCell(Gameboard.Gameboard gameboard, UnityEngine.Camera camera, Navigator.Navigator navigator)
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

                if (targetCell == null || targetCell.Content.Type == CellContentType.Wall)
                    return true;

                if (_navigator.CanMoveToCell(ref targetCell))
                {
                    Cell = targetCell;
                    return false;
                }
            }

            return true;
        }
    }
}
