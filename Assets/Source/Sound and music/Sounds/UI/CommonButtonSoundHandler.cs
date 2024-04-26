using Source.UiAnimations;
using UnityEngine;

namespace Source.Sound_and_music.Sounds.UI
{
    [RequireComponent(typeof(AudioSource))]
    public class CommonButtonSoundHandler : MonoBehaviour
    {
        [SerializeField] private AudioClip _poppingOutSound;
        [SerializeField] private AudioClip _clickSound;

        private ButtonAnimationHandler _animationHandler;
        private AudioSource _source;

        private void Awake()
        {
            _animationHandler = GetComponent<ButtonAnimationHandler>();
            _source = GetComponent<AudioSource>();
            _source.playOnAwake = false;
        }

        private void OnEnable()
        {
            _animationHandler.Bounce += OnButtonBounce;
            _animationHandler.PoppingOut += OnPoppingOut;
        }

        private void OnDisable()
        {
            _animationHandler.Bounce -= OnButtonBounce;
            _animationHandler.PoppingOut -= OnPoppingOut;
        }

        private void OnButtonBounce()
        {
            _source.clip = _clickSound;
            _source.Play();
        }

        private void OnPoppingOut()
        {
            _source.clip = _poppingOutSound;
            _source.Play();
        }
    }
}
