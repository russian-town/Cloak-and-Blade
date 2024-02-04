using UnityEngine;
using UnityEngine.UI;

public class LevelsViewScroll : MonoBehaviour
{
    [SerializeField] private ScreenAnimationHandler _screen;
    [SerializeField] private Image _blackOut;
    [SerializeField] private Scrollbar _scrollbar;

    public void Show()
    {
        _scrollbar.value = 0;
        _screen.FadeIn();
    }

    public void Hide()
    {
        _screen.FadeOut();
    }
}
