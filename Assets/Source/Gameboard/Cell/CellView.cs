using UnityEngine;

public class CellView : MonoBehaviour
{
    private ParticleSystem _enemySightEffectTemplate;
    private ParticleSystem _enemySightEffect;
    private ParticleSystem _abilityRangeEffectTemplate;
    private ParticleSystem _abilityRangeEffect;

    public void Initialize(ParticleSystem enemySightEffectTemplate, ParticleSystem abilityRangeEffectTemplate)
    {
        _enemySightEffectTemplate = enemySightEffectTemplate;
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

    public void PlayEnemySightEffect()
    {
        if (_enemySightEffect == null)
        {
            _enemySightEffect = Instantiate(_enemySightEffectTemplate);
            _enemySightEffect.transform.position = transform.position;
            _enemySightEffect.Play();
        }
        else
        {
            _enemySightEffect.Play();
        }
    }

    public void PlayAbilityRangeEffect()
    {
        if (_abilityRangeEffect == null)
        {
            _abilityRangeEffect = Instantiate(_abilityRangeEffectTemplate);
            _abilityRangeEffect.transform.position = transform.position;
            _abilityRangeEffect.Play();
            print("Playing");
        }
        else
        {
            _abilityRangeEffect.Play();
        }
    }

    public void StopAbilityRangeEffect() => _abilityRangeEffect.Stop();

    public void StopEnemySightEffect() => _enemySightEffect.Stop();
}
