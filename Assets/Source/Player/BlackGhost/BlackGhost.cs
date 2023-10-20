using UnityEngine;

[RequireComponent(typeof(Transformation))]
public class BlackGhost : Player
{
    private Transformation _transformation;

    protected override Command AbilityCommand()
    {
        _transformation = GetComponent<Transformation>();
        return new TransformationCommand(_transformation, Input, Gameboard, this, Navigator);
    }
}
