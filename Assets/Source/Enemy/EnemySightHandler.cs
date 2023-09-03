using System.Collections.Generic;
using UnityEngine;

public class EnemySightHandler : MonoBehaviour
{
    [SerializeField] private int _sightRange;

    private List<Cell> _cellsInSight = new List<Cell>();

    public void Initialize()
    {
        _cellsInSight = new List<Cell>();
    }

    public void GenerateSight(Cell currentCell, string direction)
    {
        ClearSight();

        switch (direction)
        {
            case Constants.North:

                for (int i = 0; i < _sightRange; i++)
                {
                    if (currentCell.North == null || currentCell.North.Content.Type == CellContentType.Wall)
                    {
                        break;
                    }
                    else
                    {
                        _cellsInSight.Add(currentCell.North);
                        currentCell = currentCell.North;
                    }
                }

                if (_cellsInSight.Count > 0)
                {
                    List<Cell> temp = new List<Cell>();
                    int maxWest = 0;
                    int maxEast = 0;
                    
                    bool isWestWallHit = false;
                    bool isEastWallHit = false;

                    for (int i = 0; i < _cellsInSight.Count; i++)
                    {
                        Cell westCell = _cellsInSight[i];
                        Cell eastCell = _cellsInSight[i];
                        int lastMaxWest = 0;
                        int lastMaxEast = 0;

                        if (!isWestWallHit)
                            maxWest = i;

                        if (!isEastWallHit)
                            maxEast = i;

                        for (int j = 0; j < maxWest; j++)
                            BuildeSide(temp, ref westCell, westCell.West, ref maxWest, ref lastMaxWest, ref isWestWallHit);

                        for (int j = 0; j < maxEast; j++)
                            BuildeSide(temp, ref eastCell, eastCell.East, ref maxEast, ref lastMaxEast, ref isEastWallHit);
                    }

                    _cellsInSight.AddRange(temp);
                }
                break;

            case Constants.South:

                for (int i = 0; i < _sightRange; i++)
                {
                    if (currentCell.South == null || currentCell.South.Content.Type == CellContentType.Wall)
                    {
                        break;
                    }
                    else
                    {
                        _cellsInSight.Add(currentCell.South);
                        currentCell = currentCell.South;
                    }
                }

                if (_cellsInSight.Count > 0)
                {
                    List<Cell> temp = new List<Cell>();
                    int maxWest = 0;
                    int maxEast = 0;
                    bool isWestWallHit = false;
                    bool isEastWallHit = false;

                    for (int i = 0; i < _cellsInSight.Count; i++)
                    {
                        Cell westCell = _cellsInSight[i];
                        Cell eastCell = _cellsInSight[i];
                        int lastMaxWest = 0;
                        int lastMaxEast = 0;

                        if (!isWestWallHit)
                            maxWest = i;

                        if (!isEastWallHit)
                            maxEast = i;

                        for (int j = 0; j < maxWest; j++)
                            BuildeSide(temp, ref westCell, westCell.West, ref maxWest, ref lastMaxWest, ref isWestWallHit);

                        for (int j = 0; j < maxEast; j++)
                            BuildeSide(temp, ref eastCell, eastCell.East, ref maxEast, ref lastMaxEast, ref isEastWallHit);
                    }

                    _cellsInSight.AddRange(temp);
                }
                break;

            case Constants.West:

                for (int i = 0; i < _sightRange; i++)
                {
                    if (currentCell.West == null || currentCell.West.Content.Type == CellContentType.Wall)
                    {
                        break;
                    }
                    else
                    {
                        _cellsInSight.Add(currentCell.West);
                        currentCell = currentCell.West;
                    }
                }

                if (_cellsInSight.Count > 0)
                {
                    List<Cell> temp = new List<Cell>();
                    int maxNorth = 0;
                    int maxSouth = 0;
                    bool isNorthWallHit = false;
                    bool isSouthWallHit = false;

                    for (int i = 0; i < _cellsInSight.Count; i++)
                    {
                        Cell northCell = _cellsInSight[i];
                        Cell southCell = _cellsInSight[i];
                        int lastMaxNorth = 0;
                        int lastMaxSouth = 0;

                        if (!isNorthWallHit)
                            maxNorth = i;

                        if (!isSouthWallHit)
                            maxSouth = i;

                        for (int j = 0; j < maxNorth; j++)
                            BuildeSide(temp, ref northCell, northCell.North, ref maxNorth, ref lastMaxNorth, ref isNorthWallHit);

                        for (int j = 0; j < maxSouth; j++)
                            BuildeSide(temp, ref southCell, southCell.South, ref maxSouth, ref lastMaxSouth, ref isSouthWallHit);
                    }

                    _cellsInSight.AddRange(temp);
                }
                break;

            case Constants.East:

                for (int i = 0; i < _sightRange; i++)
                {
                    if (currentCell.East == null || currentCell.East.Content.Type == CellContentType.Wall)
                    {
                        break;
                    }
                    else
                    {
                        _cellsInSight.Add(currentCell.East);
                        currentCell = currentCell.East;
                    }
                }

                if (_cellsInSight.Count > 0)
                {
                    List<Cell> temp = new List<Cell>();
                    int maxNorth = 0;
                    int maxSouth = 0;
                    bool isNorthWallHit = false;
                    bool isSouthWallHit = false;

                    for (int i = 0; i < _cellsInSight.Count; i++)
                    {
                        Cell northCell = _cellsInSight[i];
                        Cell southCell = _cellsInSight[i];
                        int lastMaxNorth = 0;
                        int lastMaxSouth = 0;

                        if (!isNorthWallHit)
                            maxNorth = i;

                        if (!isSouthWallHit)
                            maxSouth = i;

                        for (int j = 0; j < maxNorth; j++)
                            BuildeSide(temp, ref northCell, northCell.North, ref maxNorth, ref lastMaxNorth, ref isNorthWallHit);

                        for (int j = 0; j < maxSouth; j++)
                            BuildeSide(temp, ref southCell, southCell.South, ref maxSouth, ref lastMaxSouth, ref isSouthWallHit);
                    }

                    _cellsInSight.AddRange(temp);
                }
                break;
        }

        ShowSight(_cellsInSight);
    }

    public void ClearSight()
    {
        if (_cellsInSight.Count > 0)
        {
            foreach (Cell cell in _cellsInSight)
                cell.CellView.StopEnemySightEffect();

            _cellsInSight.Clear();
        }
    }

    private void BuildeSide(List<Cell> tempList, ref Cell cell, Cell sideCell, ref int maxSide, ref int lastMaxSide, ref bool isWallhit)
    {
        if (sideCell.Content.Type == CellContentType.Wall && isWallhit == false)
        {
            isWallhit = true;
            maxSide = lastMaxSide;
        }
        else if (lastMaxSide < maxSide && sideCell.Content.Type == CellContentType.Wall && isWallhit == true)
        {
            maxSide = lastMaxSide;
        }
        else if (sideCell.Content.Type != CellContentType.Wall && sideCell != null)
        {
            lastMaxSide++;
            tempList.Add(sideCell);
            cell = sideCell;
        }
    }

    private void ShowSight(List<Cell> cellsInSight)
    {
        if (cellsInSight.Count > 0)
            foreach (Cell cell in cellsInSight)
                cell.CellView.PlayEnemySightEffect();
    }
}
