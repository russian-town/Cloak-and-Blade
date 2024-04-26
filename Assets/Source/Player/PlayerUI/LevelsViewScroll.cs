using Source.UiAnimations;
using UnityEngine;

namespace Source.Player.PlayerUI
{
    public class LevelsViewScroll : MonoBehaviour
    {
        [SerializeField] private ScreenAnimationHandler _screen;

        public void Show() => _screen.FadeIn();

        public void Hide() => _screen.FadeOut();
    }
}
