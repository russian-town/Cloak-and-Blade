using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    [SerializeField] private List<Cell> _cellsInInteractibleRange;
    [SerializeField] private InteractiveObjectView _view;

    protected Player Player { get; private set; }

    private void OnDisable()
    {
        Player.StepEnded -= CheckInteractionPossibility;
    }

    public void Initialize(Player player)
    {
        Player = player;
        Player.StepEnded += CheckInteractionPossibility;
    }
    
    public abstract void Interact();

    protected void CheckInteractionPossibility()
    {
        if (_cellsInInteractibleRange.Contains(Player.CurrentCell))
        {
            _view.Show();
            _view.InteractButton.onClick.AddListener(Interact);
        }
        else
        {
            if (_view.isActiveAndEnabled)
            {
                _view.InteractButton.onClick.RemoveListener(Interact);
                _view.Hide();
            }
        }
    }
}
