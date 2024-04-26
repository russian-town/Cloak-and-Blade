using System.Collections.Generic;
using System.Linq;
using Source.Gameboard.Cell.CellContent;
using UnityEngine;

namespace Source.Gameboard
{
    public class Gameboard : MonoBehaviour
    {
        private readonly float _divider = 2f;
        private readonly float _mapOffsetSizeRatio = 1f;

        [SerializeField] private Transform _ground;
        [SerializeField] private Cell.Cell _cellTemplate;
        [SerializeField] private Vector2Int _size;
        [SerializeField] private List<Cell.Cell> _cells;
        [SerializeField] private Queue<Cell.Cell> _searchFrontier = new ();

        public IReadOnlyList<Cell.Cell> Cells => _cells;

        public void Enable()
            => gameObject.SetActive(true);

        public void Disable()
            => gameObject.SetActive(false);

        public void HideGrid()
        {
            foreach (var cell in _cells)
                cell.View.Hide();
        }

        public void ShowGrid()
        {
            foreach (var cell in _cells)
                cell.View.Show();
        }

        public bool FindPath(Cell.Cell destination, ref Cell.Cell nextCell, Cell.Cell startCell)
        {
            foreach (Cell.Cell cell in _cells)
            {
                if (cell != destination)
                    cell.ClearPath();

                if (destination.Content.Type == CellContentType.Wall)
                {
                    if (destination.South != null && destination.South.Content.Type == CellContentType.Empty)
                        destination = destination.South;
                    else if (destination.West != null && destination.West.Content.Type == CellContentType.Empty)
                        destination = destination.West;
                    else if (destination.East != null && destination.East.Content.Type == CellContentType.Empty)
                        destination = destination.East;
                    else if (destination.North != null && destination.North.Content.Type == CellContentType.Empty)
                        destination = destination.North;
                }

                destination.BecomeDestination();
                _searchFrontier.Enqueue(destination);
            }

            if (_searchFrontier.Count == 0)
            {
                nextCell = null;
                return false;
            }

            while (_searchFrontier.Count > 0)
            {
                Cell.Cell cell = _searchFrontier.Dequeue();

                if (cell != null)
                {
                    if (cell.IsAlternative)
                    {
                        _searchFrontier.Enqueue(cell.GrowPathNorth());
                        _searchFrontier.Enqueue(cell.GrowPahtSouth());
                        _searchFrontier.Enqueue(cell.GrowPathEast());
                        _searchFrontier.Enqueue(cell.GrowPathWest());
                    }
                    else
                    {
                        _searchFrontier.Enqueue(cell.GrowPathWest());
                        _searchFrontier.Enqueue(cell.GrowPathEast());
                        _searchFrontier.Enqueue(cell.GrowPahtSouth());
                        _searchFrontier.Enqueue(cell.GrowPathNorth());
                    }
                }
            }

            foreach (var cell in _cells)
                cell.ShowPath();

            nextCell = startCell.NextOnPath;

            if (nextCell == null)
                return false;

            return true;
        }

        public Cell.Cell GetCell(Ray ray)
        {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                int x = (int)(hit.point.x + _size.x / _divider);
                int y = (int)(hit.point.z + _size.y / _divider);

                if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
                    return _cells[x + y * _size.x];
            }

            return null;
        }

        [ContextMenu("Generate map")]
        private void GenerateMap()
        {
            _ground.localScale = new Vector3(_size.x, _size.y, _mapOffsetSizeRatio);

            Vector2 offSet = new Vector2((_size.x - _mapOffsetSizeRatio) * _divider, (_size.y - _mapOffsetSizeRatio) / _divider);
            _cells = new Cell.Cell[_size.x * _size.y].ToList();

            for (int i = 0, y = 0; y < _size.y; y++)
            {
                for (int x = 0; x < _size.x; x++, i++)
                {
                    Cell.Cell cell = _cells[i] = Instantiate(_cellTemplate);
                    cell.transform.SetParent(transform, false);
                    cell.transform.localPosition = new Vector3(x - offSet.x, 0f, y - offSet.y);

                    if (x > 0)
                        Cell.Cell.MakeEastWestNeighbors(cell, _cells[i - 1]);

                    if (y > 0)
                        Cell.Cell.MakeNorthSouthNeighbors(cell, _cells[i - _size.x]);

                    if ((x & 1) == 0)
                        cell.SetCellAsAlternative();

                    if ((y & 1) == 0)
                    {
                        if (cell.IsAlternative)
                        {
                            cell.SetCellAsNotAlternative();
                        }
                        else
                        {
                            cell.SetCellAsAlternative();
                        }
                    }

                    cell.Content.BecomeEmpty();
                }
            }
        }
    }
}
