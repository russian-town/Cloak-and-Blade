using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]  
public class EffectChangeHanldler : MonoBehaviour
{
    private ParticleSystem _system;
    private ParticleSystem.MainModule _main;
    private Coroutine _changeSpeedOverTime;

    private void Start()
    {
        _system = GetComponent<ParticleSystem>();
        _main = _system.main;
    }

    public void ChangeEffectSpeed(float value, float duration)
    {
        if (_changeSpeedOverTime != null)
            return;

        _changeSpeedOverTime = StartCoroutine(ChangeEffectSpeedOverTime(value, duration));
    }

    private IEnumerator ChangeEffectSpeedOverTime(float value, float duration)
    {
        while(_main.simulationSpeed != value)
        {
            _main.simulationSpeed = Mathf.MoveTowards(_main.simulationSpeed, value, duration * Time.deltaTime);
            yield return null;
        }

        _changeSpeedOverTime = null;
    }
}
