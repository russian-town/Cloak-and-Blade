using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private Model _model;
    [SerializeField] private Cell _north;
    [SerializeField] private Cell _south;
    [SerializeField] private Cell _east;
    [SerializeField] private Cell _west;
    [SerializeField] private Cell _nextOnPath;
    [SerializeField] private CellContent _content;
    [SerializeField] private Color _startColor;

    private int _distance;
    private Quaternion _northRotation = Quaternion.Euler(90f, 0f, 0f);
    private Quaternion _eastRotation = Quaternion.Euler(90f, 90f, 0f);
    private Quaternion _southRotation = Quaternion.Euler(90f, 180f, 0f);
    private Quaternion _westRotation = Quaternion.Euler(90f, 270f, 0f);

    public Cell North => _north;
    public Cell South => _south;
    public Cell East => _east;
    public Cell West => _west;
    public Cell NextOnPath => _nextOnPath;
    public bool HasPath => _distance != int.MaxValue;
    public bool IsAlternative { get; set; }
    public Quaternion Rotation => _model.transform.localRotation;
    public Color StartColor => _startColor;
    public CellContent Content
    {
        get => _content;

        set
        {
            if (_content != null) 
            {
                _content.Recycle();
            }

            _content = value;
            _content.transform.localPosition = Vector3.zero;
        }
    }

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

    public void Initialize(Color color)
    {
        _model.SetColor(color);
    }

    public void SwithColor(Color color)
    {
        _model.SetColor(color);
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
        return neighbor.Content.Type != CellContentType.Wall ? neighbor : null;
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

        _model.transform.localRotation = _nextOnPath == _north ? _northRotation : _nextOnPath == _east ? _eastRotation : _nextOnPath == _south ? _southRotation : _westRotation;
    }
}
