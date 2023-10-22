using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public abstract bool CanUse();

    public virtual bool Cast(Cell cell)
    {
        Action(cell);
        return true;
    }

    protected abstract void Action(Cell cell);

    public abstract void Initialize();

    public abstract void Prepare();

    public abstract void Cancel();
}
