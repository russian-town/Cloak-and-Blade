using System;
using UnityEngine;

namespace Source.Tutorial.TutorialElements
{
    public abstract class BaseTutorialElement : MonoBehaviour
    {
        public event Action TutorialZoneComplete;

        public abstract void Show(Player.Player player);

        public void InvokeTutorialZoneCompleteAction() => TutorialZoneComplete?.Invoke();
    }
}
