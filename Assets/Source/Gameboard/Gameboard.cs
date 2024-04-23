using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gameboard : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private Cell _cellTemplate;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private List<Cell> _cells;
    [SerializeField] private Queue<Cell> _searchFrontier = new();

    public IReadOnlyList<Cell> Cells => _cells;

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

    public bool FindPath(Cell destination, ref Cell nextCell, Cell startCell)
    {
        foreach (Cell cell in _cells)
        {
            if (cell == destination)
            {
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
            else
            {
                cell.ClearPath();
            }
        }

        if (_searchFrontier.Count == 0)
        {
            nextCell = null;
            return false;
        }

        while (_searchFrontier.Count > 0)
        {
            Cell cell = _searchFrontier.Dequeue();

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

    public Cell GetCell(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            int x = (int)(hit.point.x + _size.x * 0.5f);
            int y = (int)(hit.point.z + _size.y * 0.5f);

            if (x >= 0 && x < _size.x && y >= 0 && y < _size.y)
                return _cells[x + y * _size.x];
        }

        return null;
    }

    [ContextMenu("Generate map")]
    private void GenerateMap()
    {
        _ground.localScale = new Vector3(_size.x, _size.y, 1f);

        Vector2 offSet = new Vector2((_size.x - 1f) * 0.5f, (_size.y - 1f) * 0.5f);
        _cells = new Cell[_size.x * _size.y].ToList();

        for (int i = 0, y = 0; y < _size.y; y++)
        {
            for (int x = 0; x < _size.x; x++, i++)
            {
                Cell cell = _cells[i] = Instantiate(_cellTemplate);
                cell.transform.SetParent(transform, false);
                cell.transform.localPosition = new Vector3(x - offSet.x, 0f, y - offSet.y);

                if (x > 0)
                    Cell.MakeEastWestNeighbors(cell, _cells[i - 1]);

                if (y > 0)
                    Cell.MakeNorthSouthNeighbors(cell, _cells[i - _size.x]);

                cell.IsAlternative = (x & 1) == 0;

                if ((y & 1) == 0)
                    cell.IsAlternative = !cell.IsAlternative;

                cell.Content.BecomeEmpty();
            }
        }
    }
}
