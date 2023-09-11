using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Command
{
    public bool IsExecuting { get; protected set; }

    public abstract void Prepare();

    public abstract void Execute(Cell clickedCell);

    public abstract void Cancel();
}
