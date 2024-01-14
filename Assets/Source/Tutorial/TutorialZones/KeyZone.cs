using UnityEngine;

public class KeyZone : TutorialZone
{
    [SerializeField] private Gameboard _gameboard;

    public override void Interact()
    {
        _gameboard.Disable();
        base.Interact();
        Element.Show(Player);
    }
}
