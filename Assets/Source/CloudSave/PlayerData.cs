using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private Player _currentPlayer;
    private CharacterView _currentCharacterView;
    private Character _currentSelectedCharacter;

    public Player CurrentPlayer => _currentPlayer;
    public Character CurrentSelectedCharacter => _currentSelectedCharacter;
    public CharacterView CurrentCharacterView => _currentCharacterView;

    public PlayerData(Player currentPlayer, CharacterView currentCharacterView, Character character)
    {
        _currentPlayer = currentPlayer;
        _currentCharacterView = currentCharacterView;
        _currentSelectedCharacter = character;
    }
}
