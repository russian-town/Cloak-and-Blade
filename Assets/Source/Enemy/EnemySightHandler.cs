using System.Collections.Generic;
using UnityEngine;

public class EnemySightHandler : MonoBehaviour
{
    [SerializeField] private int _sightRange;

    private List<Cell> _cellsInSight = new List<Cell>();
    private EnemyZoneDrawer _zoneDrawer;

    public void Initialize(EnemyZoneDrawer enemyZoneDrawer)
    {
        _cellsInSight = new ();
        _zoneDrawer = enemyZoneDrawer;
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

                BuildVerticalSide();
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

                BuildVerticalSide();
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

                BuildHorizontalSide();
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

                BuildHorizontalSide();
                break;
        }

        _zoneDrawer.GenerateMesh(_cellsInSight);
    }

    public bool TryFindPlayer(Player player)
    {
        if (player.CurrentCell == null || _cellsInSight.Count == 0)
            return false;

        if (_cellsInSight.Contains(player.CurrentCell))
            return true;

        return false;
    }

    public void ClearSight()
    {
        if (_cellsInSight.Count > 0)
        {
            _cellsInSight.Clear();
            _zoneDrawer.ClearMesh();
        }
    }

    /*private void DrawSightZone(Cell startingCell, Cell neighborCell)
    {
        Cell currentCell;

        for (int i = 0; i < _sightRange; i++)
        {
            if (neighborCell == null || neighborCell.Content.Type == CellContentType.Wall)
            {
                break;
            }
            else
            {
                _cellsInSight.Add(startingCell.North);
                currentCell = startingCell.North;
            }
        }

        
    }*/

    private void BuildVerticalSide()
    {
        if (_cellsInSight.Count > 0)
        {
            List<Cell> temp = new();
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
                    BuildSide(temp, ref westCell, westCell.West, ref maxWest, ref lastMaxWest, ref isWestWallHit);

                for (int j = 0; j < maxEast; j++)
                    BuildSide(temp, ref eastCell, eastCell.East, ref maxEast, ref lastMaxEast, ref isEastWallHit);
            }

            _cellsInSight.AddRange(temp);
        }
    }

    private void BuildHorizontalSide()
    {
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
                    BuildSide(temp, ref northCell, northCell.North, ref maxNorth, ref lastMaxNorth, ref isNorthWallHit);

                for (int j = 0; j < maxSouth; j++)
                    BuildSide(temp, ref southCell, southCell.South, ref maxSouth, ref lastMaxSouth, ref isSouthWallHit);
            }

            _cellsInSight.AddRange(temp);
        }
    }

    private void BuildSide(List<Cell> tempList, ref Cell cell, Cell sideCell, ref int maxSide, ref int lastMaxSide, ref bool isWallhit)
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
}
