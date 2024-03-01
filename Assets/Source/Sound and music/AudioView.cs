using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioView : MonoBehaviour, IDataReader, IDataWriter
{
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Audio _audio;

    private void OnEnable()
    {
        _masterSlider.onValueChanged.AddListener(_audio.ChangeMasterVolume);
        _soundSlider.onValueChanged.AddListener(_audio.ChangeSoundVolume);
        _musicSlider.onValueChanged.AddListener(_audio.ChangeMusicVolume);
    }

    private void OnDisable()
    {
        _masterSlider.onValueChanged.RemoveListener(_audio.ChangeMasterVolume);
        _soundSlider.onValueChanged.RemoveListener(_audio.ChangeSoundVolume);
        _musicSlider.onValueChanged.RemoveListener(_audio.ChangeMusicVolume);
    }

    public void Write(PlayerData playerData)
    {
        playerData.MasterSliderValue = _masterSlider.value;
        playerData.SoundSliderValue = _soundSlider.value;
        playerData.MusicSliderValue = _musicSlider.value;
    }

    public void Read(PlayerData playerData)
    {
        _masterSlider.value = playerData.MasterSliderValue;
        _soundSlider.value = playerData.SoundSliderValue;
        _musicSlider.value = playerData.MusicSliderValue;
    }
}
