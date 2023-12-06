using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class StarAnimationHandler : MonoBehaviour
{
    private float _animationLength = .5f;
    private Vector3 _popScale = new Vector3(1.2f, 1.2f, 1.2f);
    private bool _canAppear;

    public void PlayAppearAnimation()
    {
        transform.DOPunchScale(_popScale, _animationLength, 5, .5f);
    }
}
