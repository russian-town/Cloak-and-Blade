using System.Collections;
using UnityEngine;

public class AbilityCommand : Command
{
    private readonly Ability _ability;

    private Gameboard _gameboard;
    private Camera _camera;
    private Player _player;
    private Navigator _navigator;
    private Coroutine _executeCoroutine;

    public AbilityCommand(Ability ability, Gameboard gameboard, Player player, Navigator navigator)
    {
        _ability = ability;
        _gameboard = gameboard;
        _camera = Camera.main;
        _player = player;
        _navigator = navigator;
    }

    protected override IEnumerator PrepareAction()
    {
        _ability.Prepare();
        yield return null;
    }

    public override void Cancel(MonoBehaviour context)
    {
        _ability.Cancel();

        if(_executeCoroutine != null) 
        {
            _player.StopCoroutine(_executeCoroutine);
            _executeCoroutine = null;
        }
    }

    protected override IEnumerator ExecuteAction(Cell clickedCell)
    {
        yield return new WaitUntil(() => _ability.Cast(clickedCell));
    }

    public override IEnumerator WaitOfExecute()
    {
        WaitOfClickedCell waitOfClickedCell = new WaitOfClickedCell(_gameboard, _camera, _navigator);
        yield return waitOfClickedCell;
        _executeCoroutine = _player.StartCoroutine(Execute(waitOfClickedCell.Cell, _player));
        yield return _executeCoroutine;
    }
}
