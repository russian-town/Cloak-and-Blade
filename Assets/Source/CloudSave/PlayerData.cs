using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private Player _currentPlayer;

    public Player CurrentPlayer => _currentPlayer;

    public PlayerData(Player currentPlayer)
    {
        _currentPlayer = currentPlayer;
    }
}
