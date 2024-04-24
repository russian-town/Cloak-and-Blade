using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    [SerializeField] private List<Cell> _cellsInInteractibleRange;

    private Player _player;

    public Player Player => _player;

    protected IReadOnlyList<Cell> CellsInInteractibleRange => _cellsInInteractibleRange;

    private void OnDisable()
    {
        _player.StepEnded -= OnStepEnded;
        Disable();
    }

    public virtual void Initialize(Player player)
    {
        _player = player;
        _player.StepEnded += OnStepEnded;
    }

    public abstract void Prepare();
    
    public abstract void Interact();

    protected abstract void Disable();

    protected bool CheckInteractionPossibility()
    {
        if (_cellsInInteractibleRange.Contains(Player.CurrentCell))
            return true;

        return false;
    }

    private void OnStepEnded()
        => Prepare();
}
