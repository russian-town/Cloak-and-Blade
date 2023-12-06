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
    [SerializeField] private float _duration;

    private Queue<Star> _starTemplates = new Queue<Star>();
    private Queue<StarContainer> _starContainers = new Queue<StarContainer>();

    public void Initialize()
    {
        _starTemplates.Enqueue(_firstStarTemplate);
        _starTemplates.Enqueue(_secondStarTemplate);
        _starTemplates.Enqueue(_thirdStarTemplate);
        _starContainers.Enqueue(_firstStarContainer);
        _starContainers.Enqueue(_secondStarContainer);
        _starContainers.Enqueue(_thirdStarContainer);
    }

    public void ShowStars(int starCount)
    {
        if (starCount == 0 || starCount > 3)
            return;

        StartCoroutine(StartShowingStars(starCount));
    }

    private IEnumerator StartShowingStars(int starCount)
    {
        for (int i = 0; i < starCount; i++)
        {
            yield return new WaitForSeconds(_duration);
            _starSpawner.Get(_starTemplates.Dequeue(), _starContainers.Dequeue());
        }
    }
}
