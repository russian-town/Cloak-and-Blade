using UnityEngine;
using UnityEngine.EventSystems;

public class AutoSelector : MonoBehaviour
{
    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}
