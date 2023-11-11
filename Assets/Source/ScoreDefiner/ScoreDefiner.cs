using UnityEngine;

[RequireComponent (typeof(ScoreDefinerView))]
public class ScoreDefiner : MonoBehaviour
{
    [SerializeField] private int _stepCountToSecodStart;
    [SerializeField] private int _stepCountToThirdStart;
    [SerializeField] private LeaderBoardScoreSetter _scoreSetter;

    private ScoreDefinerView _view;
    private int _starsCount;

    public int StarsCount => _starsCount;

    public void Initialize()
    {
        _view = GetComponent<ScoreDefinerView>();
        _view.Initialize();
    }

    public void RecieveStars(int stepCount)
    {
        if (stepCount <= _stepCountToThirdStart)
            _starsCount = 3;
        else if (stepCount <= _stepCountToSecodStart)
            _starsCount = 2;
        else
            _starsCount = 1;

#if UNITY_WEBGL && !UNITY_EDITOR
        _scoreSetter.SetPlayerScore(_starsCount);
#endif

        _view.ShowStars(_starsCount);
    }
}
