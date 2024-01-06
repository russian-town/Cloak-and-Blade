using System.Collections.Generic;
using UnityEngine;

public class EnemyTutorial : BaseTutorialElement
{
    [SerializeField] private List<MainButton> _mainButtons = new List<MainButton>();
    [SerializeField] private int _skipCount;
    [SerializeField] private Gameboard _gameboard;
    [SerializeField] private List<Cell> _enemyPath = new List<Cell>();
    [SerializeField] private Bootstrap _bootstrap;
    [SerializeField] private Door _door;
    [SerializeField] private int _skipCountToOpenDoor;

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

        if(_skipCount == _skipCountToOpenDoor)
            _door.Open();

        if (_skipCount <= 0)
        {
            _player.StepEnded -= OnStepEnded;
            InvokeTutorialZoneCompleteAction();
            _door.Close();
            _bootstrap.RemoveEnemy();

            foreach (var cell in _enemyPath)
                cell.Content.BecomeWall();
        }
    }
}
