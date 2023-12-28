using UnityEngine;

public class KeyElement : BaseTutorialElement
{
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private Key _key;

    public override void Show(Player player)
    {
        _key.PickedUp += OnPickedUp;
    }

    private void OnPickedUp()
    {
        _gameboard.Disable();
        _key.PickedUp -= OnPickedUp;
        InvokeTutorialZoneCompleteAction();
    }
}
