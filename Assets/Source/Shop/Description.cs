using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class Description : MonoBehaviour
{
    [SerializeField] private ScreenAnimationHandler _animationHandler;

    private Canvas _canvas;

    public ScreenAnimationHandler ScreenAnimationHandler => _animationHandler;

    public void Show(Camera camera)
    {
        if (_canvas == null)
            _canvas = GetComponent<Canvas>();

        _canvas.worldCamera = camera;
        _canvas.planeDistance = 1f;

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
