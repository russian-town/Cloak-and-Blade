using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    [SerializeField] private List<Cell> _cellsInInteractibleRange;

    protected Player Player { get; private set; }

    public void Initialize(Player player)
    {
        Player = player;
    }

    public abstract void Interact();

    protected bool CheckInteractionPossibility()
    {
        if(_cellsInInteractibleRange.Contains(Player.CurrentCell))
            return true;

        return false;
    }
}
