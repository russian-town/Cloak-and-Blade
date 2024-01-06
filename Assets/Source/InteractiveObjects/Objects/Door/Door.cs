using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour
{
    private Animator _animator;

    private void Awake() => _animator = GetComponent<Animator>();

    public void Open() => _animator.SetTrigger(Constants.OpenParameter);

    public void Close() => _animator.SetTrigger(Constants.CloseParameter);
}
