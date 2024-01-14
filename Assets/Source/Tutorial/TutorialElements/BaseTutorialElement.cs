using System;
using UnityEngine;

public abstract class BaseTutorialElement : MonoBehaviour
{
    public event Action TutorialZoneComplete;

    public abstract void Show(Player player);

    public void InvokeTutorialZoneCompleteAction() => TutorialZoneComplete?.Invoke();
}
