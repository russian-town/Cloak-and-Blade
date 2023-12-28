using System.Collections.Generic;
using UnityEngine;

public class EnemyTutorial : BaseTutorialElement
{
    [SerializeField] private List<MainButton> _mainButtons = new List<MainButton>();
    [SerializeField] private int _skipCount;
    [SerializeField] private Gameboard _gameboard;

    private Player _player;

    public override void Show(Player player)
    {
        _player = player;
        _player.StepEnded += OnStepEnded;

        foreach (var button in _mainButtons)
        {
            button.Open();
            button.Show();
        }
    }

    private void OnStepEnded()
    {
        _skipCount--;

        if (_skipCount <= 0)
        {
            _player.StepEnded -= OnStepEnded;
            InvokeTutorialZoneCompleteAction();
        }
    }
}
