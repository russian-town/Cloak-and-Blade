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

            if(_cellsInStraightSight.Count > 0)
            {
                List<Cell> temp = new List<Cell>();

                for (int i = 0; i < _cellsInStraightSight.Count; i++)
                {
                    if (i == 0)
                        continue;

                    Cell westCell = _cellsInStraightSight[i];
                    Cell eastCell = _cellsInStraightSight[i];

                    /*BuildeSides(temp, ref westCell, westCell.West, i);
                    BuildeSides(temp, ref eastCell, eastCell.East, i);*/

                    for (int j = 0; j < i; j++)
                    {
                        if (westCell.West != null && westCell.West.Content.Type != CellContentType.Wall)
                        {
                            temp.Add(westCell.West);
                            westCell = westCell.West;
                        }

                        if (eastCell.East != null && eastCell.East.Content.Type != CellContentType.Wall)
                        {
                            temp.Add(eastCell.East);
                            eastCell = eastCell.East;
                        }
                    }
                }

                _cellsInStraightSight.AddRange(temp);
            }

            //for (int i = 0; i < _cellsInStraightSight.Count; i++)
            //{
            //    if (i == 0)
            //        continue;

            //    Cell westCell = _cellsInStraightSight[i].West;
            //    Cell eastCell = _cellsInStraightSight[i].East;

            //    if (westCell != null)
            //        _cellsInStraightSight.Add(westCell);

            //    if (eastCell != null)
            //        _cellsInStraightSight.Add(eastCell);
            //}
        }

        if (currentCell.South == facingCell)
        {
            _cellsInStraightSight.Add(currentCell);

            for (int i = 0; i < _sightRange; i++)
            {
                if (_cellsInStraightSight[i].South == null || _cellsInStraightSight[i].South.Content.Type == CellContentType.Wall)
                    break;

                _cellsInStraightSight.Add(_cellsInStraightSight[i].South);

                for (int j = 0; j < i; j++)
                    if(_cellsInStraightSight[i].South.East.Content.Type != CellContentType.Wall)
                    _cellsInStraightSight.Add(_cellsInStraightSight[i].South.East);

                for (int k = 0; k < i; k++)
                    if (_cellsInStraightSight[i].South.West.Content.Type != CellContentType.Wall)
                        _cellsInStraightSight.Add(_cellsInStraightSight[i].South.West);
            }
        }

        if (currentCell.West == facingCell)
        {
            _cellsInStraightSight.Add(currentCell);

            for (int i = 0; i < _sightRange; i++)
            {
                if (_cellsInStraightSight[i].West == null || _cellsInStraightSight[i].West.Content.Type == CellContentType.Wall)
                    break;

                _cellsInStraightSight.Add(_cellsInStraightSight[i].West);

                for (int j = 0; j < i; j++)
                    if (_cellsInStraightSight[i].West.North.Content.Type != CellContentType.Wall)
                        _cellsInStraightSight.Add(_cellsInStraightSight[i].West.North);

                for (int k = 0; k < i; k++)
                    if (_cellsInStraightSight[i].West.North.Content.Type != CellContentType.Wall)
                        _cellsInStraightSight.Add(_cellsInStraightSight[i].West.South);
            }
        }

        if (currentCell.East == facingCell)
        {
            _cellsInStraightSight.Add(currentCell);

            for (int i = 0; i < _sightRange; i++)
            {
                if (_cellsInStraightSight[i].East == null || _cellsInStraightSight[i].East.Content.Type == CellContentType.Wall)
                    break;

                _cellsInStraightSight.Add(_cellsInStraightSight[i].East);

                for (int j = 0; j < i; j++)
                    if (_cellsInStraightSight[i].East.North.Content.Type != CellContentType.Wall)
                        _cellsInStraightSight.Add(_cellsInStraightSight[i].East.North);

                for (int k = 0; k < i; k++)
                    if (_cellsInStraightSight[i].East.South.Content.Type != CellContentType.Wall)
                        _cellsInStraightSight.Add(_cellsInStraightSight[i].East.South);
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

    private void BuildeSides(List<Cell> tempList, ref Cell cell, Cell sideCell, int i)
    {
        for (int j = 0; j < i; j++)
        {
            if (sideCell != null)
            {
                tempList.Add(sideCell);
                cell = sideCell;
            }
        }
    }

    private void ShowSight(List<Cell> cellsInSight)
    {
        if (cellsInSight.Count > 0)
            foreach (Cell cell in cellsInSight)
                cell.SwithColor(Color.red);
    }
}
