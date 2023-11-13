using System.Collections.Generic;
using UnityEngine;

public class PlayersHandler : MonoBehaviour, IDataReader, IDataWriter
{
    [SerializeField] private List<Player> _playerTemplates = new List<Player>();
    [SerializeField] private Player _defaultPlayerTemplate;
    [SerializeField] private List<EffectChangeHanldler> _effectChangeHanldlers = new List<EffectChangeHanldler>();

    private Player _currentPlayer;

    public void SetCurrentPlayer(Player player) => _currentPlayer = player;

    public Player GetPlayer(GhostSpawner spawner, Cell playerSpawnCell)
    {
        Player player;

        if (_playerTemplates.Contains(_currentPlayer))
        {
            int index = _playerTemplates.IndexOf(_currentPlayer);
            player = (Player)spawner.Get(playerSpawnCell, _playerTemplates[index]);
        }
        else
        {
            player = (Player)spawner.Get(playerSpawnCell, _defaultPlayerTemplate);
        }

        if (_effectChangeHanldlers.Count == 0)
            return player;

        if (player is ISceneParticlesInfluencer sceneParticlesInfluencer)
            sceneParticlesInfluencer.AddSceneParticles(_effectChangeHanldlers);

        return player;
    }

    public void Write(PlayerData playerData) => playerData.CurrentPlayer = _currentPlayer;

    public void Read(PlayerData playerData) => _currentPlayer = playerData.CurrentPlayer;
}
