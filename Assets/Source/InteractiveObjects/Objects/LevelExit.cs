using UnityEngine;

public class LevelExit : InteractiveObject
{
    [SerializeField] private InteractiveObjectView _view;
    [SerializeField] private Treasure _treasure;

    public override void Interact()
    {
        TryOpen();
    }

    public override void Prepare()
    {
        if (CheckInteractionPossibility())
        {
            _view.Show();
            _view.InteractButton.onClick.AddListener(Interact);
        }
        else if (_view.isActiveAndEnabled)
        {
            _view.InteractButton.onClick.RemoveListener(Interact);
            _view.Hide();
        }
    }

    public void TryOpen()
    {
        if (Player.ItemsInHold.FindItemInList(_treasure))
            print("level passed");
        else
            print("level not passed");
    }
}
