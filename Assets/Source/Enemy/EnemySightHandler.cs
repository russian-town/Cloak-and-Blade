using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemySightHandler : MonoBehaviour
{
    [SerializeField] private int _sightRange;

    private Enemy _enemy;
    private List<Cell> _cellsInStraightSight = new List<Cell>();
    private List<Cell> _generatedSight = new List<Cell>();

    private void Start()
    {
        _cellsInStraightSight = new List<Cell>();
        _enemy = GetComponent<Enemy>();
    }

    public void GenerateSight(Cell currentCell, Cell facingCell)
    {
        ClearSight();

        if (currentCell.North == facingCell)
        {
            for (int i = 0; i < _sightRange; i++)
            {
                if (currentCell.North != null && currentCell.North.Content.Type != CellContentType.Wall)
                {
                    _cellsInStraightSight.Add(currentCell.North);
                    currentCell = currentCell.North;
                }
            }

            if (_cellsInStraightSight.Count > 0)
            {
                List<Cell> temp = new List<Cell>();

                for (int i = 0; i < _cellsInStraightSight.Count; i++)
                {
                    if (i == 0)
                        continue;

                    Cell westCell = _cellsInStraightSight[i];
                    Cell eastCell = _cellsInStraightSight[i];

                    for (int j = 0; j < i; j++)
                    {
                        BuildeSide(temp, ref westCell, westCell.West);
                        BuildeSide(temp, ref eastCell, eastCell.East);
                    }
                }

                _cellsInStraightSight.AddRange(temp);
            }
        }

        if (currentCell.South == facingCell)
        {
            for (int i = 0; i < _sightRange; i++)
            {
                if (currentCell.South != null && currentCell.South.Content.Type != CellContentType.Wall)
                {
                    _cellsInStraightSight.Add(currentCell.South);
                    currentCell = currentCell.South;
                }
            }

            if (_cellsInStraightSight.Count > 0)
            {
                List<Cell> temp = new List<Cell>();

                for (int i = 0; i < _cellsInStraightSight.Count; i++)
                {
                    if (i == 0)
                        continue;

                    Cell westCell = _cellsInStraightSight[i];
                    Cell eastCell = _cellsInStraightSight[i];

                    for (int j = 0; j < i; j++)
                    {
                        BuildeSide(temp, ref westCell, westCell.West);
                        BuildeSide(temp, ref eastCell, eastCell.East);
                    }
                }

                _cellsInStraightSight.AddRange(temp);
            }
        }

        if (currentCell.West == facingCell)
        {
            for (int i = 0; i < _sightRange; i++)
            {
                if (currentCell.West != null && currentCell.West.Content.Type != CellContentType.Wall)
                {
                    _cellsInStraightSight.Add(currentCell.West);
                    currentCell = currentCell.West;
                }
            }

            if (_cellsInStraightSight.Count > 0)
            {
                List<Cell> temp = new List<Cell>();

                for (int i = 0; i < _cellsInStraightSight.Count; i++)
                {
                    if (i == 0)
                        continue;

                    Cell northCell = _cellsInStraightSight[i];
                    Cell southCell = _cellsInStraightSight[i];

                    for (int j = 0; j < i; j++)
                    {
                        BuildeSide(temp, ref northCell, northCell.North);
                        BuildeSide(temp, ref southCell, southCell.South);
                    }
                }

                _cellsInStraightSight.AddRange(temp);
            }
        }

        if (currentCell.East == facingCell)
        {
            for (int i = 0; i < _sightRange; i++)
            {
                if (currentCell.East != null && currentCell.East.Content.Type != CellContentType.Wall)
                {
                    _cellsInStraightSight.Add(currentCell.East);
                    currentCell = currentCell.East;
                }
            }

            if (_cellsInStraightSight.Count > 0)
            {
                List<Cell> temp = new List<Cell>();

                for (int i = 0; i < _cellsInStraightSight.Count; i++)
                {
                    if (i == 0)
                        continue;

                    Cell northCell = _cellsInStraightSight[i];
                    Cell southCell = _cellsInStraightSight[i];

                    for (int j = 0; j < i; j++)
                    {
                        BuildeSide(temp, ref northCell, northCell.North);
                        BuildeSide(temp, ref southCell, southCell.South);
                    }
                }

                _cellsInStraightSight.AddRange(temp);
            }
        }

        ShowSight(_cellsInStraightSight);
    }

    public void ClearSight()
    {
        if (_cellsInStraightSight.Count > 0)
        {
            foreach (Cell cell in _cellsInStraightSight)
                _enemy.Gameboard.SetDefaultCellColor(cell);

            _cellsInStraightSight.Clear();
        }
    }

    private void BuildeSide(List<Cell> tempList, ref Cell cell, Cell sideCell)
    {
        if (sideCell != null && sideCell.Content.Type != CellContentType.Wall)
        {
            tempList.Add(sideCell);
            cell = sideCell;
        }
    }

    private void ShowSight(List<Cell> cellsInSight)
    {
        if (cellsInSight.Count > 0)
            foreach (Cell cell in cellsInSight)
                cell.SwithColor(Color.red);
    }
}
