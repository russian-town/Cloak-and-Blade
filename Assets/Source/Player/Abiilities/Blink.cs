using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player), typeof(Navigator))]
public class Blink : Ability
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _blinkRange = 4;
    [SerializeField] private ParticleSystem _prepareEffect;
    [SerializeField] private ParticleSystem _actionEffect;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _prepareSound;
    [SerializeField] private AudioClip _actionSound;

    private UpgradeSetter _upgradeSetter;
    private Player _player;
    private List<Cell> _availableNorthCells = new List<Cell>();
    private List<Cell> _availableEastCells = new List<Cell>();
    private List<Cell> _availableSouthCells = new List<Cell>();
    private List<Cell> _availableWestCells = new List<Cell>();
    private List<Cell> _availableCells = new List<Cell>();
    private Navigator _navigator;
    private bool _canUse = true;

    public override void Initialize(UpgradeSetter upgradeSetter)
    {
        _player = GetComponent<Player>();
        _navigator = GetComponent<Navigator>();
        _upgradeSetter = upgradeSetter;
        _blinkRange += _upgradeSetter.Level;
    }

    public override void Prepare()
    {
        Cell currentCell = _player.CurrentCell;
        BuildBlinkRange(currentCell);
        ShowBlinkRange();
        _source.clip = _prepareSound;
        _source.Play();
        _prepareEffect.Play();
    }

    public override void Cancel()
    {
        HideBlinkRange();
        _prepareEffect.Stop();
        _source.Stop();
        _availableWestCells.Clear();
        _availableEastCells.Clear();
        _availableSouthCells.Clear();
        _availableNorthCells.Clear();
        _availableCells.Clear();
    }

    public override bool CanUse()
    {
        return _canUse;
    }

    protected override void Action(Cell cell)
    {
        if (_player.TryMoveToCell(cell, _moveSpeed, _rotationSpeed))
        {
            _canUse = false;
            Cancel();
            _source.clip = _actionSound;
            _source.Play();
            _actionEffect.Play();
        }
    }

    private void BuildBlinkRange(Cell currentCell)
    {
        _availableWestCells.Clear();
        _availableEastCells.Clear();
        _availableSouthCells.Clear();
        _availableNorthCells.Clear();
        _availableCells.Clear();

        Cell tempCellNorth = currentCell.North;
        Cell tempCellSouth = currentCell.South;
        Cell tempCellWest = currentCell.West;
        Cell tempCellEast = currentCell.East;

        for (int i = 0; i < _blinkRange; i++)
        {
            if (tempCellNorth != null)
            {
                _availableNorthCells.Add(tempCellNorth);
                tempCellNorth = tempCellNorth.North;
            }

            if (tempCellSouth != null)
            {
                _availableSouthCells.Add(tempCellSouth);
                tempCellSouth = tempCellSouth.South;
            }

            if (tempCellWest != null)
            {
                _availableWestCells.Add(tempCellWest);
                tempCellWest = tempCellWest.West;
            }

            if (tempCellEast != null)
            {
                _availableEastCells.Add(tempCellEast);
                tempCellEast = tempCellEast.East;
            }
        }

        _availableCells.AddRange(_availableWestCells);
        _availableCells.AddRange(_availableSouthCells);
        _availableCells.AddRange(_availableEastCells);
        _availableCells.AddRange(_availableNorthCells);
        _navigator.RefillAvailableCells(_availableNorthCells, _availableWestCells, _availableSouthCells, _availableEastCells);
    }

    private void ShowBlinkRange()
    {
        foreach (var cell in _availableCells)
            if (cell.Content.Type != CellContentType.Wall)
                cell.View.PlayAbilityRangeEffect();
    }

    private void HideBlinkRange()
    {
        foreach (var cell in _availableCells)
            cell.View.StopAbilityRangeEffect();
    }
}
