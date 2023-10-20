using UnityEngine;

[RequireComponent(typeof(Blink))]
public class RedGhost : Player
{
    private Blink _blink;

    protected override Command AbilityCommand()
    {
        _blink = GetComponent<Blink>();
        return new BlinkCommand(_blink, Input, Gameboard, this, Navigator);
    }
}
