using UnityEngine;
using UnityEngine.UI;

public class LevelsViewScroll : MonoBehaviour
{
    [SerializeField] private ScreenAnimationHandler _screen;
    [SerializeField] private Image _blackOut;

    public void Show()
    {
        _screen.FadeIn();
    }

    public void Hide()
    {
        _screen.FadeOut();
    }
}
