using System.Collections.Generic;
using UnityEngine;

public class PlayersHandler : MonoBehaviour, IDataReader, IDataWriter
{
    private Player _currentPlayer;

    public Player CurrentPlayer => _currentPlayer;

    public void SetCurrentPlayer(Player player) => _currentPlayer = player;

    public void Write(PlayerData playerData) => playerData.CurrentPlayer = _currentPlayer;

    public void Read(PlayerData playerData) => _currentPlayer = playerData.CurrentPlayer;
}
