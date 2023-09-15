using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    [SerializeField] private List<Cell> _cellsInInteractibleRange;

    protected Player Player { get; private set; }

    private void OnDisable()
    {
        Player.StepEnded -= OnStepEnded;
    }

    public virtual void Initialize(Player player)
    {
        Player = player;
        Player.StepEnded += OnStepEnded;
    }

    public abstract void Prepare();
    
    public abstract void Interact();

    protected bool CheckInteractionPossibility()
    {
        if (_cellsInInteractibleRange.Contains(Player.CurrentCell))
            return true;
        else
            return false;
    }

    private void OnStepEnded()
    {
        Prepare();
    }
}
