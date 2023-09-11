using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public virtual void Cast(Cell cell)
    {

    }

    public abstract void Initialize(PlayerMover mover = null);

    public abstract void Prepare();

    public abstract void Cancel();
}
