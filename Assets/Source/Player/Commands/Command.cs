using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public abstract void Prepare();

    public abstract void Execute();

    public abstract void Cancel();
}
