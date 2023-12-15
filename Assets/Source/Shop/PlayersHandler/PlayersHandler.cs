using System.Collections.Generic;
using UnityEngine;

public class PlayersHandler : MonoBehaviour, IDataReader, IDataWriter
{
    [SerializeField] private List<Player> _playerTemplates = new List<Player>();
    [SerializeField] private Player _defaultPlayerTemplate;
    [SerializeField] private List<EffectChangeHanldler> _effectList = new List<EffectChangeHanldler>();
    [SerializeField] private List<SoundChangeHandler> _soundList = new List<SoundChangeHandler>();
    [SerializeField] private List<SplineChangeHandler> _splineList = new List<SplineChangeHandler>();
    [SerializeField] private List<AnimationChangeHandler> _animationList = new List<AnimationChangeHandler>();

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

        if (_effectList.Count == 0 && _soundList.Count == 0 && _splineList.Count == 0 && _animationList.Count == 0)
            return player;

        if (player is ISceneParticlesInfluencer sceneParticlesInfluencer)
            sceneParticlesInfluencer.AddSceneEffectsToChange(_effectList, _soundList, _splineList, _animationList);

        return player;
    }

    public void Write(PlayerData playerData) => playerData.CurrentPlayer = _currentPlayer;

    public void Read(PlayerData playerData) => _currentPlayer = playerData.CurrentPlayer;
}
