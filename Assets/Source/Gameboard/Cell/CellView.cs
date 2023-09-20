using UnityEngine;

public class CellView : MonoBehaviour
{
    private ParticleSystem _abilityRangeEffectTemplate;
    private ParticleSystem _abilityRangeEffect;

    public void Initialize(ParticleSystem abilityRangeEffectTemplate)
    {
        _abilityRangeEffectTemplate = abilityRangeEffectTemplate;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
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
