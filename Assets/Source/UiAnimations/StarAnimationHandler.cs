using UnityEngine;
using DG.Tweening;

public class StarAnimationHandler : MonoBehaviour
{
    private float _animationLength = .5f;
    private Vector3 _popScale = new Vector3(1.1f, 1.1f, 1.1f);
        
    public void PlayAppearAnimation()
    {
        transform.DOPunchScale(_popScale, _animationLength, 3, 1);
    }
}
