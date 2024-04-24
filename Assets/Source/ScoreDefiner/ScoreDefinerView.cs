using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class ScoreDefinerView : MonoBehaviour
{
    [SerializeField] private FirstStar _firstStarTemplate;
    [SerializeField] private SecondStar _secondStarTemplate;
    [SerializeField] private ThirdStar _thirdStarTemplate;
    [SerializeField] private StarSpawner _starSpawner;
    [SerializeField] private FirstStarContainer _firstStarContainer;
    [SerializeField] private SecondStarContainer _secondStarContainer;
    [SerializeField] private ThirdStarContainer _thirdStarContainer;
    [SerializeField] private float _starSpawnDelay;

    private Coroutine _startShowStars;
    private Queue<Star> _starTemplates = new ();
    private Queue<StarContainer> _starContainers = new ();
    private WaitForSeconds _starSpawnDelayWaitForSeconds;

    public void Initialize()
    {
        _starSpawnDelayWaitForSeconds = new WaitForSeconds(_starSpawnDelay);
        _starTemplates.Enqueue(_firstStarTemplate);
        _starTemplates.Enqueue(_secondStarTemplate);
        _starTemplates.Enqueue(_thirdStarTemplate);
        _starContainers.Enqueue(_firstStarContainer);
        _starContainers.Enqueue(_secondStarContainer);
        _starContainers.Enqueue(_thirdStarContainer);
    }

    public void ShowStars(int starCount)
    {
        if (starCount > 3)
            return;

        if (starCount == 0)
            starCount = 3;

        if (_startShowStars != null)
            return;

        _startShowStars = StartCoroutine(StartShowingStars(starCount));
    }

    private IEnumerator StartShowingStars(int starCount)
    {
        for (int i = 0; i < starCount; i++)
        {
            yield return _starSpawnDelay;
            _starSpawner.Get(_starTemplates.Dequeue(), _starContainers.Dequeue());
        }
    }
}
