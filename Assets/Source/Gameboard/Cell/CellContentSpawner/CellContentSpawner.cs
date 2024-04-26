using Source.Gameboard.Cell.CellContent;
using UnityEngine;

namespace Source.Gameboard.Cell.CellContentSpawner
{
    public class CellContentSpawner : MonoBehaviour
    {
        [SerializeField] private CellContent.CellContent _destinationTemplate;
        [SerializeField] private CellContent.CellContent _emptyTemplate;

        public void Reclaim(CellContent.CellContent content)
            => Destroy(content.gameObject);

        public CellContent.CellContent Get(CellContentType type, Transform parent)
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

        private CellContent.CellContent Get(CellContent.CellContent template, Transform parent)
        {
            CellContent.CellContent content = Instantiate(template);
            content.Spawner = this;
            content.transform.SetParent(parent, false);
            return content;
        }
    }
}
