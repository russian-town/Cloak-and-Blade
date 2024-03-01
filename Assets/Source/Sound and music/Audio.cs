using System;
using UnityEngine;
using UnityEngine.Audio;

public class Audio : MonoBehaviour, IDataReader
{
    [SerializeField] private AudioMixerGroup _masterGroup;
    [SerializeField] private AudioMixerGroup _soundGroup;
    [SerializeField] private AudioMixerGroup _musicGroup;

    private float _currentMasterVolume;

    public event Action AudioValueChanged;

    public void Read(PlayerData playerData)
    {
        _currentMasterVolume = Mathf.Log10(playerData.MasterSliderValue) * 20f;
        _masterGroup.audioMixer.SetFloat(Constants.MasterVolume, Mathf.Log10(playerData.MasterSliderValue) * 20f);
        _masterGroup.audioMixer.SetFloat(Constants.SoundVolume, Mathf.Log10(playerData.SoundSliderValue) * 20f);
        _masterGroup.audioMixer.SetFloat(Constants.MusicVolume, Mathf.Log10(playerData.MusicSliderValue) * 20f);
    }

    public void Mute() => AudioListener.pause = true;

    public void UnMute() => AudioListener.pause = false;

    public void ChangeMasterVolume(float value)
    {
        _masterGroup.audioMixer.SetFloat(Constants.MasterVolume, Mathf.Log10(value) * 20f);
        AudioValueChanged?.Invoke();
    }

    public void ChangeSoundVolume(float value)
    {
        _soundGroup.audioMixer.SetFloat(Constants.SoundVolume, Mathf.Log10(value) * 20f);
        AudioValueChanged?.Invoke();
    }

    public void ChangeMusicVolume(float value)
    {
        _musicGroup.audioMixer.SetFloat(Constants.MusicVolume, Mathf.Log10(value) * 20f);
        AudioValueChanged?.Invoke();
    }
}
