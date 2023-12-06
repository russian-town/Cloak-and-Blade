using UnityEngine;

public class StarSpawner : MonoBehaviour
{
    public Star Get(Star template, StarContainer container)
    {
        Star star = Instantiate(template, container.transform);
        star.PlayStarAppear();
        return star;
    }
}
