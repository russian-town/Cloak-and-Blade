using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Audio : MonoBehaviour
{
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private AudioMixerGroup _masterGroup;
    [SerializeField] private AudioMixerGroup _soundGroup;
    [SerializeField] private AudioMixerGroup _musicGroup;

    private void OnEnable()
    {
        _masterSlider.onValueChanged.AddListener(ChangeMasterVolume);
        _soundSlider.onValueChanged.AddListener(ChangeSoundVolume);
        _musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
    }

    private void OnDisable()
    {
        _masterSlider.onValueChanged.RemoveListener(ChangeMasterVolume);
        _soundSlider.onValueChanged.RemoveListener(ChangeSoundVolume);
        _musicSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
    }

    private void ChangeMasterVolume(float value)
    {
        _masterGroup.audioMixer.SetFloat(Constants.MasterVolume, Mathf.Log10(value) * 20f);
    }

    private void ChangeSoundVolume(float value)
    {
        _soundGroup.audioMixer.SetFloat(Constants.SoundVolume, Mathf.Log10(value) * 20f);
    }

    private void ChangeMusicVolume(float value)
    {
        _musicGroup.audioMixer.SetFloat(Constants.MusicVolume, Mathf.Log10(value) * 20f);
    }
}
