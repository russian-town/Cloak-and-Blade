using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard : MonoBehaviour
{
    [SerializeField] private Transform _ground;
    [SerializeField] private Cell _cellTemplate;

    private Vector2Int _size;
    private Cell[] _cells;
    private Queue<Cell> _searchFrontier = new Queue<Cell>();

    public void Initialize(Vector2Int size)
    {
        _size = size;
        _ground.localScale = new Vector3(_size.x, _size.y, 1f);

        Vector2 offSet = new Vector2((_size.x - 1f) * 0.5f, (_size.y - 1f) * 0.5f);
        _cells = new Cell[_size.x * _size.y];

        for (int i = 0, y = 0; y < _size.y; y++)
        {
            for(int x = 0; x < _size.x; x++, i++)
            {
                Cell cell = _cells[i] = Instantiate(_cellTemplate);
                cell.transform.SetParent(transform, false);
                cell.transform.localPosition = new Vector3(x - offSet.x, 0f, y - offSet.y);

                if(x > 0)
                {
                    Cell.MakeEastWestNeighbors(cell, _cells[i - 1]);
                }

                if(y > 0) 
                {
                    Cell.MakeNorthSouthNeighbors(cell, _cells[i - _size.x]);
                }

                cell.IsAlternative = (x & 1) == 0;

                if((y & 1) == 0)
                {
                    cell.IsAlternative = !cell.IsAlternative;
                }
            }
        }

        FindPath();
    }

    public void FindPath()
    {
        foreach (var cell in _cells)
        {
            cell.ClearPath();
        }

        int destinationIndex = _cells.Length / 2;
        _cells[destinationIndex].BecomeDestination();
        _searchFrontier.Enqueue(_cells[destinationIndex]);

        while(_searchFrontier.Count > 0)
        {
            Cell cell = _searchFrontier.Dequeue();

            if(cell != null)
            {
                if(cell.IsAlternative)
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
        {
            cell.ShowPath();
        }
    }
}
