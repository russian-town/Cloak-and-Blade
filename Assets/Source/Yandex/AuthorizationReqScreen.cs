using UnityEngine;

[RequireComponent(typeof(ScreenAnimationHandler))]
[RequireComponent(typeof(CanvasGroup))]
public class AuthorizationReqScreen : MonoBehaviour
{
    private ScreenAnimationHandler _animationHandler;

    public CanvasGroup CanvasGroup { get; private set; }

    private void Start()
    {
        _animationHandler = GetComponent<ScreenAnimationHandler>();
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    public void Enable()
    {
        _animationHandler.FadeIn();
    }

    public void Disable()
    {
        _animationHandler.FadeOut();
    }
}
