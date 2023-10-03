using System.Collections.Generic;
using UnityEngine;

public class ModelsScroll : MonoBehaviour
{
    private MenuPlayerModel[] _playerModels;
    private Vector3[] _place;
    private List<Character> _characters = new List<Character>();

    public void Initialize(Character caracter)
    {
        _characters.Add(caracter);
    }

    private void OnSelectedButtonClicked()
    {

    }
}
