using UnityEngine;

[RequireComponent(typeof(ScreenAnimationHandler))]
public class AuthorizationReqScreen : MonoBehaviour
{
    private ScreenAnimationHandler _animationHandler;

    private void Start()
    {
        _animationHandler = GetComponent<ScreenAnimationHandler>();
    }

    public void Enable()
    {
        _animationHandler.ScreenFadeIn();
    }
}
