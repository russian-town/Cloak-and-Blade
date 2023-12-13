using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelector : MonoBehaviour
{
    [SerializeField] private ScreenAnimationHandler _screenAnimationHandler;

    private void OnEnable()
    {
        _screenAnimationHandler.ScreenEnabled += OnScreenEnabled;
    }

    private void OnDisable()
    {
        _screenAnimationHandler.ScreenEnabled -= OnScreenEnabled;
    }

    private void OnScreenEnabled()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
