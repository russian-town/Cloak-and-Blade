using UnityEngine;

public class CellContentSpawner : MonoBehaviour
{
    [SerializeField] private CellContent _destinationTemplate;
    [SerializeField] private CellContent _emptyTemplate;

    public void Reclaim(CellContent content)
        => Destroy(content.gameObject);

    public CellContent Get(CellContentType type, Transform parent)
    {
        switch (type)
        {
            case CellContentType.Empty:
                return Get(_emptyTemplate, parent);
            case CellContentType.Destination:
                return Get(_destinationTemplate, parent);
        }

        return null;
    }

    private CellContent Get(CellContent template, Transform parent)
    {
        CellContent content = Instantiate(template);
        content.Spawner = this;
        content.transform.SetParent(parent, false);
        return content;
    }
}
