using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public abstract bool CanUse();

    public abstract void ResetAbility();

    public virtual bool Cast(Cell cell)
    {
        Action(cell);
        return true;
    }
    
    public abstract void Initialize(UpgradeSetter upgradeSetter, PlayerView playerView);

    public abstract void Prepare();

    public abstract void Cancel();

    protected abstract void Action(Cell cell);
}
