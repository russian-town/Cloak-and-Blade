using UnityEngine;

public class KeyElement : BaseTutorialElement
{
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private Key _key;

    public override void Show(Player player)
    {
        _key.PickedUp += OnPickedUp;
        _gameboard.Enable();
    }

    private void OnPickedUp()
    {
        _key.PickedUp -= OnPickedUp;
        InvokeTutorialZoneCompleteAction();
    }
}
