using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Transform _model;

    private Cell _north;
    private Cell _south;
    private Cell _east;
    private Cell _west;
    private Cell _nextOnPath;
    private List<Cell> _neighboringCells;
    private int _distance;
    private Quaternion _northRotation = Quaternion.Euler(90f, 0f, 0f);
    private Quaternion _eastRotation = Quaternion.Euler(90f, 90f, 0f);
    private Quaternion _southRotation = Quaternion.Euler(90f, 180f, 0f);
    private Quaternion _westRotation = Quaternion.Euler(90f, 270f, 0f);

    public bool HasPath => _distance != int.MaxValue;
    public bool IsAlternative { get; set; }

    public static void MakeEastWestNeighbors(Cell east, Cell west)
    {
        west._east = east;
        east._west = west;
    }

    public static void MakeNorthSouthNeighbors(Cell north, Cell south)
    {
        north._south = south;
        south._north = north;
    }

    public void ClearPath()
    {
        _distance = int.MaxValue;
        _nextOnPath = null;
    }
    
    public void BecomeDestination()
    {
        _distance = 0;
        _nextOnPath = null;
    }

    public Cell GrowPathTo(Cell neighbor)
    {
        if(!HasPath || neighbor == null || neighbor.HasPath)
        {
            return null;
        }

        neighbor._distance = _distance + 1;
        neighbor._nextOnPath = this;
        return neighbor;
    }

    public Cell GrowPathNorth() => GrowPathTo(_north);
    public Cell GrowPathEast() => GrowPathTo(_east);
    public Cell GrowPahtSouth() => GrowPathTo(_south);
    public Cell GrowPathWest() => GrowPathTo(_west);

    public void ShowPath()
    {
        if(_distance == 0)
        {
            return;
        }

        _model.localRotation = _nextOnPath == _north ? _northRotation : _nextOnPath == _east ? _eastRotation : _nextOnPath == _south ? _southRotation : _westRotation;
    }
}
