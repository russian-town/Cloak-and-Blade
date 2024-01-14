using UnityEngine;

public class Description : MonoBehaviour
{
    [SerializeField] private ScreenAnimationHandler _animationHandler;

    public ScreenAnimationHandler ScreenAnimationHandler => _animationHandler;

    public void Show()
    {
        if (_animationHandler.IsEnabled)
            return;

        _animationHandler.FadeIn();
    }

    public void Hide()
    {
        if (!_animationHandler.IsEnabled)
            return;

        _animationHandler.FadeOut();
    }
}
