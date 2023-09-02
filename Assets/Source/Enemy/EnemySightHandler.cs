using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemySightHandler : MonoBehaviour
{
    [SerializeField] private int _sightRange;

    private Enemy _enemy;
    private List<Cell> _cellsInSight = new List<Cell>();

    private void Start()
    {
        _cellsInSight = new List<Cell>();
        _enemy = GetComponent<Enemy>();
    }

    public void GenerateSight(Cell currentCell, Cell facingCell)
    {
        ClearSight();

        if (currentCell.North == facingCell)
        {
            print("Facing north");
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

                for (int i = 0; i < _cellsInSight.Count; i++)
                {
                    Cell westCell = _cellsInSight[i];
                    Cell eastCell = _cellsInSight[i];

                    for (int j = 0; j < i; j++)
                    {
                        BuildeSide(temp, ref westCell, westCell.West);
                        BuildeSide(temp, ref eastCell, eastCell.East);
                    }
                }

                _cellsInSight.AddRange(temp);
            }
        }

        else if (currentCell.South == facingCell)
        {
            print("Facing south");
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

                for (int i = 0; i < _cellsInSight.Count; i++)
                {
                    Cell westCell = _cellsInSight[i];
                    Cell eastCell = _cellsInSight[i];

                    for (int j = 0; j < i; j++)
                    {
                        BuildeSide(temp, ref westCell, westCell.West);
                        BuildeSide(temp, ref eastCell, eastCell.East);
                    }
                }

                _cellsInSight.AddRange(temp);
            }
        }

        else if (currentCell.West == facingCell)
        {
            print("Facing west");
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

                for (int i = 0; i < _cellsInSight.Count; i++)
                {
                    Cell northCell = _cellsInSight[i];
                    Cell southCell = _cellsInSight[i];

                    for (int j = 0; j < i; j++)
                    {
                        BuildeSide(temp, ref northCell, northCell.North);
                        BuildeSide(temp, ref southCell, southCell.South);
                    }
                }

                _cellsInSight.AddRange(temp);
            }
        }

        else if (currentCell.East == facingCell)
        {
            print("Facing east");
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

                for (int i = 0; i < _cellsInSight.Count; i++)
                {
                    Cell northCell = _cellsInSight[i];
                    Cell southCell = _cellsInSight[i];

                    for (int j = 0; j < i; j++)
                    {
                        BuildeSide(temp, ref northCell, northCell.North);
                        BuildeSide(temp, ref southCell, southCell.South);
                    }
                }

                _cellsInSight.AddRange(temp);
            }
        }

        ShowSight(_cellsInSight);
    }

    public void ClearSight()
    {
        if (_cellsInSight.Count > 0)
        {
            foreach (Cell cell in _cellsInSight)
                _enemy.Gameboard.SetDefaultCellColor(cell);

            _cellsInSight.Clear();
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
