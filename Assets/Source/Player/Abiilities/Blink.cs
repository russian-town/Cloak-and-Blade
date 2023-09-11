using System.Collections.Generic;

public class Blink : Ability
{
    private PlayerMover _mover;
    private List<Cell> _availableCells;
    private int _blinkRange = 4;
    private bool _isPrepared;

    public override void Initialize(PlayerMover mover)
    {
        _availableCells = new List<Cell>();
        _mover = mover;
    }

    public override void Prepare()
    {
        if (_isPrepared)
        {
            Cancel();
        }
        else
        {
            Cell currentCell = _mover.CurrentCell;
            BuildBlinkRange(currentCell);
            ShowBlinkRange();
            _isPrepared = true;
            print($"Blink prepared");
        }
    }

    public override void Cancel()
    {
        HideBlinkRange();
        _availableCells.Clear();
        _isPrepared = false;
        print($"Canceled blink");
    }

    public override void Cast(Cell clickedCell)
    {
        if (_availableCells.Contains(clickedCell))
        {
            if(clickedCell.Content.Type != CellContentType.Wall)
            StartCoroutine(_mover.StartMoveTo(clickedCell));
        }

        print($"Moving to {clickedCell}");
        Cancel();
        _isPrepared = false;
    }

    private void BuildBlinkRange(Cell currentCell)
    {
        _availableCells.Clear();
        Cell tempCellNorth = currentCell.North;
        Cell tempCellSouth = currentCell.South;
        Cell tempCellWest = currentCell.West;
        Cell tempCellEast = currentCell.East;

        for (int i = 0; i < _blinkRange; i++)
        {
            if (tempCellNorth != null)
            {
                _availableCells.Add(tempCellNorth);
                tempCellNorth = tempCellNorth.North;
            }

            if (tempCellSouth != null)
            {
                _availableCells.Add(tempCellSouth);
                tempCellSouth = tempCellSouth.South;
            }

            if (tempCellWest != null)
            {
                _availableCells.Add(tempCellWest);
                tempCellWest = tempCellWest.West;
            }

            if (tempCellEast != null)
            {
                _availableCells.Add(tempCellEast);
                tempCellEast = tempCellEast.East;
            }
        }
    }

    private void ShowBlinkRange()
    {
        foreach(var cell in _availableCells)
        {
            if(cell.Content.Type != CellContentType.Wall)
            cell.View.PlayAbilityRangeEffect();
        }

        print(_availableCells.Count);
        print("Showing cells");
    }

    private void HideBlinkRange()
    {
        foreach (var cell in _availableCells)
            cell.View.StopAbilityRangeEffect();
    }
}
