using UnityEngine;

public class CellView : MonoBehaviour
{
    private ParticleSystem _abilityRangeEffectTemplate;
    private ParticleSystem _abilityRangeEffect;
    private MeshRenderer _meshRenderer;

    public void Initialize(ParticleSystem abilityRangeEffectTemplate)
    {
        _abilityRangeEffectTemplate = abilityRangeEffectTemplate;
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Hide()
    {
        _meshRenderer.enabled = false;
    }

    public void Show()
    {
        _meshRenderer.enabled = true;
    }

    public void PlayAbilityRangeEffect()
    {
        if (_abilityRangeEffect == null)
        {
            _abilityRangeEffect = Instantiate(_abilityRangeEffectTemplate);
            _abilityRangeEffect.transform.position = transform.position;
            _abilityRangeEffect.Play();
        }
        else
        {
            _abilityRangeEffect.Play();
        }
    }

    public void StopAbilityRangeEffect()
    {
        if (_abilityRangeEffect != null)
            _abilityRangeEffect.Stop();
    }
}
