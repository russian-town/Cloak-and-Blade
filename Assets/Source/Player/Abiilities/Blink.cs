using System.Collections.Generic;

public class Blink : Ability
{
    private PlayerMover _mover;
    private PlayerInput _input;
    private List<Cell> _availableCells;
    private int _blinkRange = 4;

    private void OnDisable()
    {
        _input.CellClicked -= OnCellClicked; 
    }

    public override void Initialize(PlayerMover mover, PlayerInput input)
    {
        _availableCells = new List<Cell>();
        _mover = mover;
        _input = input;
    }

    public override void Prepare()
    {
        Cell currentCell = _mover.CurrentCell;
        _input.CellClicked += OnCellClicked;
        BuildBlinkRange(currentCell);
        ShowBlinkRange();
    }

    public override void Cancel()
    {
        _input.CellClicked -= OnCellClicked;
        HideBlinkRange();
    }

    private void OnCellClicked(Cell clickedCell)
    {
        if (_availableCells.Contains(clickedCell))
            StartCoroutine(_mover.StartMoveTo(clickedCell));

        print($"Moving to {clickedCell}");
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
            if (tempCellNorth != null && tempCellNorth.Content.Type != CellContentType.Wall)
            {
                _availableCells.Add(tempCellNorth);
                tempCellNorth = tempCellNorth.North;
            }

            if (tempCellSouth != null && tempCellSouth.Content.Type != CellContentType.Wall)
            {
                _availableCells.Add(tempCellSouth);
                tempCellSouth = tempCellSouth.South;
            }

            if (tempCellWest != null && tempCellWest.Content.Type != CellContentType.Wall)
            {
                _availableCells.Add(tempCellWest);
                tempCellWest = tempCellWest.West;
            }

            if (tempCellEast != null && tempCellEast.Content.Type != CellContentType.Wall)
            {
                _availableCells.Add(tempCellEast);
                tempCellEast = tempCellEast.East;
            }
        }
    }

    private void ShowBlinkRange()
    {
        foreach(var cell in _availableCells)
            cell.View.PlayAbilityRangeEffect();

        print(_availableCells.Count);
        print("Showing cells");
    }

    private void HideBlinkRange()
    {
        foreach (var cell in _availableCells)
            cell.View.StopAbilityRangeEffect();
    }
}
