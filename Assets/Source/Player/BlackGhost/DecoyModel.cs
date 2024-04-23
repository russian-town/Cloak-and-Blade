using UnityEngine;

public class DecoyModel : MonoBehaviour
{
    public void Hide()
        => gameObject.SetActive(false);

    public void Show()
        => gameObject.SetActive(true);
}
