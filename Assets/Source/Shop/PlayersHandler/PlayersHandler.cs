using System.Collections.Generic;
using Source.Gameboard.Cell;
using Source.Ghost.GhostSpawner;
using Source.Player;
using Source.Player.Abiilities.SceneEffectChangers;
using Source.Saves;
using UnityEngine;

namespace Source.Shop.PlayersHandler
{
    public class PlayersHandler : MonoBehaviour, IDataReader, IDataWriter
    {
        [SerializeField] private List<Player.Player> _playerTemplates = new List<Player.Player>();
        [SerializeField] private Player.Player _defaultPlayerTemplate;
        [SerializeField] private List<EffectChangeHandler> _effectList = new List<EffectChangeHandler>();
        [SerializeField] private List<SoundChangeHandler> _soundList = new List<SoundChangeHandler>();
        [SerializeField] private List<SplineChangeHandler> _splineList = new List<SplineChangeHandler>();
        [SerializeField] private List<AnimationChangeHandler> _animationList = new List<AnimationChangeHandler>();

        private Player.Player _currentPlayer;

        public void SetCurrentPlayer(Player.Player player) => _currentPlayer = player;

        public Player.Player GetPlayer(GhostSpawner spawner, Cell playerSpawnCell)
        {
            Player.Player player;

            if (_playerTemplates.Contains(_currentPlayer))
            {
                int index = _playerTemplates.IndexOf(_currentPlayer);
                player = (Player.Player)spawner.Get(playerSpawnCell, _playerTemplates[index]);
            }
            else
            {
                player = (Player.Player)spawner.Get(playerSpawnCell, _defaultPlayerTemplate);
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
}
