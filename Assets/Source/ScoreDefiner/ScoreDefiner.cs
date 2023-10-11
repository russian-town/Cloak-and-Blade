using UnityEngine;

public class ScoreDefiner : MonoBehaviour
{
    [SerializeField] private int _stepCountToFirstStart;
    [SerializeField] private int _stepCountToSecodStart;
    [SerializeField] private int _stepCountToThirdStart;

    private int _starsCount;

    private void AccrueStars(int stepCount)
    {
        if (stepCount <= _stepCountToFirstStart)
            _starsCount = 1;
        else if (stepCount <= _stepCountToSecodStart)
            _starsCount = 2;
        else if (stepCount <= _stepCountToThirdStart)
            _starsCount = 3;
        else
            return;
    }
}
