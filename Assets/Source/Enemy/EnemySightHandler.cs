using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemySightHandler : MonoBehaviour
{
    [SerializeField] private int _sightRange;

    private Enemy _enemy;
    private List<Cell> _cellsInStraightSight;
    private List<Cell> _generatedSight;

    private void Start()
    {
        _cellsInStraightSight = new List<Cell>();
        _enemy = GetComponent<Enemy>();
    }

    public void GenerateSight(Cell currentCell, Cell facingCell)
    {
        if (_cellsInStraightSight.Count > 0)
        {
            foreach (Cell cell in _cellsInStraightSight)
                _enemy.Gameboard.SetDefaultCellColor(cell);

            _cellsInStraightSight.Clear();
        }

        if (currentCell.North == facingCell)
        {
            _cellsInStraightSight.Add(currentCell);

            for (int i = 0; i < _sightRange; i++)
            {
                if (_cellsInStraightSight[i].North == null || _cellsInStraightSight[i].North.Content.Type == CellContentType.Wall)
                    break;

                _cellsInStraightSight.Add(_cellsInStraightSight[i].North);

                for (int j = 0; j < i; j++)
                    if (_cellsInStraightSight[i].North.East.Content.Type != CellContentType.Wall)
                        _cellsInStraightSight.Add(_cellsInStraightSight[i].North.East);

                for (int k = 0; k < i; k++)
                    if (_cellsInStraightSight[i].North.West.Content.Type != CellContentType.Wall)
                        _cellsInStraightSight.Add(_cellsInStraightSight[i].North.West);
            }

            ShowSight(_cellsInStraightSight);
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

            ShowSight(_cellsInStraightSight);
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

            ShowSight(_cellsInStraightSight);
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

            ShowSight(_cellsInStraightSight);
        }
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

    private void ShowSight(List<Cell> cellsInSight)
    {
        if (cellsInSight.Count > 0)
            foreach (Cell cell in cellsInSight)
                cell.SwithColor(Color.red);
    }
}
