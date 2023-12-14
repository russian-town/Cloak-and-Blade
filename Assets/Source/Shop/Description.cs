using UnityEngine;

public class Description : MonoBehaviour
{
    [SerializeField] private ScreenAnimationHandler _animationHandler;

    public ScreenAnimationHandler ScreenAnimationHandler => _animationHandler;

    public void Show()
    {
        if (_animationHandler.IsEnabled)
            return;

        _animationHandler.ScreenFadeIn();
    }

    public void Hide()
    {
        if (!_animationHandler.IsEnabled)
            return;

        _animationHandler.ScreenFadeOut();
    }
}
