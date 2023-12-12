using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SplineAnimate))]
public class FlyOnTrigger : InteractiveObject
{
    [SerializeField] private AudioClip _flappingWingsSound;

    private Animator _animator;
    private SplineAnimate _splineAnimate;
    private AudioSource _source;
    private bool _isExecuted;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _splineAnimate = GetComponent<SplineAnimate>();
        _source = GetComponent<AudioSource>();
    }

    public override void Interact()
    {
        _isExecuted = true;
        _source.clip = _flappingWingsSound;
        _source.Play();
        _splineAnimate.Play();
    }

    public override void Prepare()
    {
        if (!_isExecuted)
        {
            if (CheckInteractionPossibility())
            {
                _animator.SetBool(Constants.FlyTrigger, true);
                Interact();
            }
        }
    }

    protected override void Disable()
    { }
}
