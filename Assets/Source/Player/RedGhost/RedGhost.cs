using UnityEngine;

[RequireComponent(typeof(Blink))]
public class RedGhost : Player
{
    private Blink _blink;

    protected override AbilityCommand AbilityCommand()
    {
        _blink = GetComponent<Blink>();
        return new BlinkCommand(_blink, Gameboard, Navigator, CommandExecuter);
    }
}
