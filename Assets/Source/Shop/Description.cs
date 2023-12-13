using UnityEngine;

public class Description : MonoBehaviour
{
    [SerializeField] private ScreenAnimationHandler _animationHandler;

    public ScreenAnimationHandler ScreenAnimationHandler => _animationHandler;

    public void Show()
    {
        _animationHandler.ScreenFadeIn();
    }

    public void Hide()
    {
        _animationHandler.ScreenFadeOut();
    }
}
