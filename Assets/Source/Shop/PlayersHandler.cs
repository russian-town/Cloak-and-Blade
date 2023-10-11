using System.Collections.Generic;
using UnityEngine;

public class PlayersHandler : MonoBehaviour
{
    private List<Player> _aviablePlayers = new List<Player>();
    private Player _currentPlayer;
    private CloudSave _cloudSave = new CloudSave();

    public Player CurrentPlayer => _currentPlayer;

    public void AddAviablePlayer(Player player) => _aviablePlayers.Add(player);

    public void SetCurrentPlayer(Player player)
    {
        _currentPlayer = player;
        PlayerData playerData = new PlayerData(_currentPlayer);
        _cloudSave.Save(playerData);
    }
}
