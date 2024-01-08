using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Door : MonoBehaviour
{
    [SerializeField] private AudioClip _open;
    [SerializeField] private AudioClip _close;

    private Animator _animator;
    private AudioSource _source;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _source = GetComponent<AudioSource>();
    }

    public void Open()
    {
        _animator.SetTrigger(Constants.OpenParameter);
        _source.PlayOneShot(_open);
    }

    public void Close()
    {
        _animator.SetTrigger(Constants.CloseParameter);
        _source.PlayOneShot(_close);
    }
}
