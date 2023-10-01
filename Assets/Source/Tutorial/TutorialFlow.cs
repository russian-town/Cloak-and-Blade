using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialFlow : MonoBehaviour
{
    [SerializeField] private Canvas _movementGuideScreen;
    [SerializeField] private Canvas _playerUI;

    private void Start()
    {
        _movementGuideScreen.gameObject.SetActive(true);
        _playerUI.gameObject.SetActive(false);
    }
}
