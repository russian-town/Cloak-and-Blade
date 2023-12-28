using UnityEngine;

public class ChestZone : TutorialZone
{
    [SerializeField] private Key _key;

    public override void Interact()
    {
        if (Player.ItemsInHold.FindItemInList(_key))
        {
            base.Interact();
            Element.Show(Player);
        }
    }
}
