using UnityEngine;

public class CharacterViewSoundHandler : MonoBehaviour
{
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _positiveClick;
    [SerializeField] private AudioClip _negativeClick;
    [SerializeField] private AudioClip _upgradeClick;
    [SerializeField] private AudioClip _unlockClick;

    public void PlayPositive() => _source.PlayOneShot(_positiveClick);

    public void PlayNegative() => _source.PlayOneShot(_negativeClick);

    public void PlayUpgrade() => _source.PlayOneShot(_upgradeClick);

    public void PlayUnlock() => _source.PlayOneShot(_unlockClick);
}
