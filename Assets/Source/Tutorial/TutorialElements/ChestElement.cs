using UnityEngine;

public class ChestElement : BaseTutorialElement
{
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private Treasure _treasure;

    public override void Show(Player player)
    {
        _treasure.Opened += OnTreasureOpened;
    }

    private void OnTreasureOpened()
    {
        _gameboard.Disable();
        _treasure.Opened -= OnTreasureOpened;
        InvokeTutorialZoneCompleteAction();
    }
}
