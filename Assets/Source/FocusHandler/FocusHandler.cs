using UnityEngine;
using Agava.YandexGames;
using Agava.WebUtility;
using System;

public class FocusHandler : MonoBehaviour
{
    [SerializeField] private Audio _audio;

    private IActiveScene _scene;

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
        if (poop)
            SetUnFocused();
        else
            SetFocused();
    }

    private void OnApplicationFocus(bool focus)
    {
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

        if (_scene == null || _audio == null)
            return;

        _scene.SetPause();
        _audio.Mute();
    }

    private void SetFocused() 
    {
        if (enabled == false)
            return;

        if (_scene == null || _audio == null)
            return;

        _audio.UnMute();

        if (_scene is IAutoContinuer continuer)
            continuer.Continue();
    }
}
