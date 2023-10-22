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

    private Player _player;
    private List<Cell> _availableCells = new List<Cell>();
    private Navigator _navigator;
    private bool _canUse = true;

    public override void Initialize()
    {
        _player = GetComponent<Player>();
        _navigator = GetComponent<Navigator>();
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

        _navigator.RefillAvailableCells(_availableCells);
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
