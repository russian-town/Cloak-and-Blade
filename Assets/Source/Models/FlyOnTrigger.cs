using Source.InteractiveObjects;
using Source.Root;
using UnityEngine;

namespace Source.Models
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(MoveAlongSpline))]
    public class FlyOnTrigger : InteractiveObject
    {
        [SerializeField] private AudioClip _flappingWingsSound;
        [SerializeField] private AudioSource _takeOfSound;

        private Animator _animator;
        private MoveAlongSpline _splineAnimate;
        private AudioSource _source;
        private bool _isExecuted;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _splineAnimate = GetComponent<MoveAlongSpline>();
            _source = GetComponent<AudioSource>();
        }

        public override void Interact()
        {
            _isExecuted = true;
            _source.clip = _flappingWingsSound;
            _takeOfSound.Play();
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
        {
        }
    }
}
