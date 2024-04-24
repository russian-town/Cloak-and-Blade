using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour, ITurnHandler
{
    private readonly List<Cell> _availableNorthCells = new List<Cell>();
    private readonly List<Cell> _availableSouthCells = new List<Cell>();
    private readonly List<Cell> _availableWestCells = new List<Cell>();
    private readonly List<Cell> _availableEastCells = new List<Cell>();
    private readonly List<Cell> _availableCells = new List<Cell>();
    private readonly List<Cell> _tempCells = new List<Cell>();
    private Turn _turn;
    private Player _player;

    public IReadOnlyList<Cell> AvailableCells => _availableCells;

    public void Initialize(Player player)
        => _player = player;

    public void RefillAvailableCells(Cell currentCell, int range)
        => RefillAvailableCells(currentCell, false, range);

    public void RefillAvailableCellsIgnoredWalls(Cell currentCell, int range)
        => RefillAvailableCells(currentCell, true, range);

    public void SetTurn(Turn turn)
        => _turn = turn;

    public bool CanMoveToCell(ref Cell cell)
    {
        if (_turn == Turn.Enemy || _player.IsDead)
            return false;

        if (TryFindCellHasTrap(_availableNorthCells, cell, out Cell findNorthCell))
        {
            cell = findNorthCell;
            return true;
        }
        else if (TryFindCellHasTrap(_availableSouthCells, cell, out Cell findSouthCell))
        {
            cell = findSouthCell;
            return true;
        }
        else if (TryFindCellHasTrap(_availableEastCells, cell, out Cell findEastCell))
        {
            cell = findEastCell;
            return true;
        }
        else if (TryFindCellHasTrap(_availableWestCells, cell, out Cell findWestCell))
        {
            cell = findWestCell;
            return true;
        }

        return false;
    }

    public void ShowAvailableCells()
        => ShowAvailableCells(_availableCells);

    public void HideAvailableCells()
    {
        if (_tempCells.Count == 0)
            return;

        foreach (var cell in _tempCells)
            cell.View.Hide();

        _tempCells.Clear();
    }

    public void RefillNorthAvailableCells(Cell currentCell, int range)
    {
        ClearAllCells();
        Cell tempCellNorth = currentCell.North;

        for (int i = 0; i < range; i++)
        {
            if (TryAddCell(tempCellNorth, _availableNorthCells, false))
            {
                tempCellNorth = tempCellNorth.North;
            }
        }
    }

    public void RefillSouthAvailableCells(Cell currentCell, int range)
    {
        ClearAllCells();
        Cell tempCellSouth = currentCell.South;

        for (int i = 0; i < range; i++)
        {
            if (TryAddCell(tempCellSouth, _availableSouthCells, false))
            {
                tempCellSouth = tempCellSouth.South;
            }
        }
    }

    public void RefillEastAvailableCells(Cell currentCell, int range)
    {
        ClearAllCells();
        Cell tempCellEast = currentCell.East;

        for (int i = 0; i < range; i++)
        {
            if (TryAddCell(tempCellEast, _availableEastCells, false))
            {
                tempCellEast = tempCellEast.East;
            }
        }
    }

    public void RefillWestAvailableCells(Cell currentCell, int range)
    {
        ClearAllCells();
        Cell tempCellWest = currentCell.West;

        for (int i = 0; i < range; i++)
        {
            if (TryAddCell(tempCellWest, _availableWestCells, false))
            {
                tempCellWest = tempCellWest.West;
            }
        }
    }

    private void ShowAvailableCells(List<Cell> availableCells)
    {
        HideAvailableCells();

        if (availableCells.Count == 0)
            return;

        foreach (var cell in availableCells)
        {
            if (cell.Content.Type != CellContentType.Wall)
            {
                cell.View.Show();
            }
        }

        _tempCells.AddRange(availableCells);
    }

    private void RefillAvailableCells(Cell currentCell, bool ignoreWalls, int range)
    {
        ClearAllCells();

        Cell tempCellNorth = currentCell.North;
        Cell tempCellSouth = currentCell.South;
        Cell tempCellWest = currentCell.West;
        Cell tempCellEast = currentCell.East;

        for (int i = 0; i < range; i++)
        {
            if (TryAddCell(tempCellNorth, _availableNorthCells, ignoreWalls))
                tempCellNorth = tempCellNorth.North;

            if (TryAddCell(tempCellSouth, _availableSouthCells, ignoreWalls))
                tempCellSouth = tempCellSouth.South;

            if (TryAddCell(tempCellEast, _availableEastCells, ignoreWalls))
                tempCellEast = tempCellEast.East;

            if (TryAddCell(tempCellWest, _availableWestCells, ignoreWalls))
                tempCellWest = tempCellWest.West;
        }
    }

    private void ClearAllCells()
    {
        _availableNorthCells.Clear();
        _availableSouthCells.Clear();
        _availableEastCells.Clear();
        _availableWestCells.Clear();
        _availableCells.Clear();
    }

    private bool TryFindCellHasTrap(List<Cell> cells, Cell findCell, out Cell targetCell)
    {
        if (!cells.Contains(findCell))
        {
            targetCell = null;
            return false;
        }

        foreach (var cell in cells)
        {
            if (cell.HasTrap)
            {
                if (cells.IndexOf(findCell) < cells.IndexOf(cell))
                {
                    targetCell = findCell;
                    return true;
                }

                targetCell = cell;
                return true;
            }
        }

        targetCell = findCell;
        return true;
    }

    private bool TryAddCell(Cell tempCell, List<Cell> cells, bool ignoreWalls)
    {
        if (tempCell == null)
            return false;

        if (ignoreWalls == true)
        {
            if (tempCell.Content.Type == CellContentType.Wall || tempCell.IsOccupied == true)
                return false;

            cells.Add(tempCell);
            _availableCells.Add(tempCell);
            return true;
        }
        else if (ignoreWalls == false)
        {
            if (tempCell.IsOccupied == true)
                return false;

            cells.Add(tempCell);
            _availableCells.Add(tempCell);
        }

        return false;
    }
}
