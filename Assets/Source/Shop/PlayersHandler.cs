using System.Collections.Generic;
using UnityEngine;

public class PlayersHandler : MonoBehaviour
{
    private List<Player> _aviablePlayers = new List<Player>();
    private Player _currentPlayer;

    public Player CurrentPlayer => _currentPlayer;

    public void AddAviablePlayer(Player player) => _aviablePlayers.Add(player);

    public void SetCurrentPlayer(Player player) => _currentPlayer = player;
}
