using System.Collections;
using UnityEngine;

public class AbilityCommand : Command
{
    private readonly Ability _ability;
    private WaitPlayerClick _waitPlayerClick;
    private Gameboard _gameboard;
    private bool _isSelected;
    private Player _player;

    public AbilityCommand(Ability ability, Gameboard gameboard, Player player)
    {
        _ability = ability;
        _gameboard = gameboard;
        _player = player;
        _waitPlayerClick = new WaitPlayerClick(_gameboard);
    }

    protected override IEnumerator PrepareAction()
    {
        _ability.Prepare();
        _isSelected = true;

        while (_isSelected)
        {
            yield return _waitPlayerClick;

            if (_player.TryMoveToCell(_waitPlayerClick.Cell))
                _isSelected = false;

            _player.ExecuteCurrentCommand();
        }
    }

    public override void Cancel()
    {
        _ability.Cancel();
        _isSelected = false;
    }

    protected override IEnumerator ExecuteAction()
    {
        yield return new WaitUntil(() => _ability.Cast(_waitPlayerClick.Cell));
    }
}
