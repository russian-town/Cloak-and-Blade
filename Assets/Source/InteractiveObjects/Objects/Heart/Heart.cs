using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : InteractiveObject
{
    [SerializeField] private GameObject _linza;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private InteractiveObjectView _view;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private WhooshScript _whoosh;

    public bool IsActive { get; private set; }

    public override void Interact()
    {
        if (IsActive)
            return;

        IsActive = true;
        /*_linza.SetActive(true);*/
        _effect.Play();
        _whoosh.PlayWhoosh();
        _audioSource.Play();
        _view.gameObject.SetActive(false);
    }

    public override void Prepare()
    {
        if (IsActive)
            return; 

        if (CheckInteractionPossibility())
        {
            _view.Show();
            _view.InteractButton.onClick.AddListener(Interact);
        }
        else if (_view.isActiveAndEnabled)
        {
            _view.InteractButton.onClick.RemoveListener(Interact);
            _view.Hide();
        }
    }

    protected override void Disable()
    {
        _view.InteractButton.onClick.RemoveListener(Interact);
    }
}
