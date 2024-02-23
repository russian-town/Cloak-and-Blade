using UnityEngine;
using Agava.YandexGames;
using Agava.WebUtility;
using System;
using UnityEngine.Events;

public class FocusHandler : MonoBehaviour
{
    [SerializeField] private Audio _audio;

    private IActiveScene _scene;

    public event Action<bool> FocusChaned;

    private void OnEnable()
    {
        WebApplication.InBackgroundChangeEvent += OnBackgroundChangeEvent;
    }

    private void OnDisable()
    {
        WebApplication.InBackgroundChangeEvent -= OnBackgroundChangeEvent;
    }

    private void OnBackgroundChangeEvent(bool poop)
    {
        FocusChaned?.Invoke(poop == false);

        if (poop)
            SetUnFocused();
        else
            SetFocused();
    }

    private void OnApplicationFocus(bool focus)
    {
        FocusChaned?.Invoke(focus);

        if (focus == false)
            SetUnFocused();
        else
            SetFocused();
    }

    public void SetActiveScene(IActiveScene scene) => _scene = scene;

    private void SetUnFocused()
    {
        if (enabled == false)
            return;

        if (_audio == null)
            return;

        _audio.Mute();

        if (_scene == null)
            return;

        _scene.SetPause();
    }

    private void SetFocused() 
    {
        if (enabled == false)
            return;

        if (_audio == null)
            return;

        _audio.UnMute();

        if (_scene == null)
            return;

        if (_scene is IAutoContinuer continuer)
            continuer.Continue();
    }
}
