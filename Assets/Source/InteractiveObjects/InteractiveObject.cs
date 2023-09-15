using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    [SerializeField] private List<Cell> _cellsInInteractibleRange;
    [SerializeField] private InteractiveObjectView _view;

    protected Player Player { get; private set; }

    public void Initialize(Player player)
    {
        Player = player;
    }

    public abstract void Interact();

    protected bool CheckInteractionPossibility()
    {
        if (_cellsInInteractibleRange.Contains(Player.CurrentCell))
        {
            _view.Show();
            _view.InteractButton.onClick.AddListener(Interact);
            return true;
        }
        else
        {
            if (_view.isActiveAndEnabled)
            {
                _view.InteractButton.onClick.RemoveListener(Interact);
                _view.Hide();
            }

            return false;
        }
    }
}
