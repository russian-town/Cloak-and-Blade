using System.Collections;
using UnityEngine;

public class AbilityCommand : Command
{
    private readonly Ability _ability;
    private WaitPlayerClick _waitPlayerClick;
    private Gameboard _gameboard;
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
        yield return _waitPlayerClick;
        _player.ExecuteCurrentCommand();
        yield return null;
    }

    public override void Cancel()
    {
        _ability.Cancel();
    }

    protected override IEnumerator ExecuteAction()
    {
        yield return new WaitUntil(() => _ability.Cast(_waitPlayerClick.Cell));
    }
}
