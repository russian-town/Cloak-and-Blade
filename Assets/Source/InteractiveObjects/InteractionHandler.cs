using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [SerializeField] private InteractiveObject _interactiveObject;

    public void Interact()
    {
        if (_interactiveObject != null)
            _interactiveObject.Interact();
    }
}
