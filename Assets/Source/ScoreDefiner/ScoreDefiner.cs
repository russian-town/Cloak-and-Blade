using UnityEngine;

[RequireComponent(typeof(ScoreDefinerView))]
public class ScoreDefiner : MonoBehaviour
{
    [SerializeField] private int _stepCountToSecodStart;
    [SerializeField] private int _stepCountToThirdStart;
    [SerializeField] private LeaderBoardScoreSetter _scoreSetter;
    [SerializeField] private int _oneStarReward;
    [SerializeField] private int _twoStarReward;
    [SerializeField] private int _threeStarReward;

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
            _starsCount = _threeStarReward;
        else if (stepCount <= _stepCountToSecodStart)
            _starsCount = _twoStarReward;
        else
            _starsCount = _oneStarReward;

#if UNITY_WEBGL && !UNITY_EDITOR
        _scoreSetter.SetPlayerScore(_starsCount);
#endif

        _view.ShowStars(_starsCount);
    }
}
