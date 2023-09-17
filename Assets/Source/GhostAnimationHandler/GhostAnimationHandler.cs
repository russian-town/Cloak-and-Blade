using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimationHandler : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    public void PlayFlyAnimation() => _animator.SetBool(Constants.IsMovingBool, true);

    public void StopFlyAnimation() => _animator.SetBool(Constants.IsMovingBool, false);

    public void PlaySkipAnimation() => _animator.SetBool(Constants.IsSkippingBool, true);

    public void StopSkipAnimation() => _animator.SetBool(Constants.IsSkippingBool, false);
}
