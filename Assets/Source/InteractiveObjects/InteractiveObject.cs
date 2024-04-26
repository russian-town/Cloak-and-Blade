using System.Collections.Generic;
using Source.Gameboard.Cell;
using UnityEngine;

namespace Source.InteractiveObjects
{
    public abstract class InteractiveObject : MonoBehaviour
    {
        [SerializeField] private List<Cell> _cellsInInteractibleRange;

        private Player.Player _player;

        public Player.Player Player => _player;

        protected IReadOnlyList<Cell> CellsInInteractibleRange => _cellsInInteractibleRange;

        private void OnDisable()
        {
            _player.StepEnded -= OnStepEnded;
            Disable();
        }

        public virtual void Initialize(Player.Player player)
        {
            _player = player;
            _player.StepEnded += OnStepEnded;
        }

        public abstract void Prepare();

        public abstract void Interact();

        protected abstract void Disable();

        protected bool CheckInteractionPossibility()
            => _cellsInInteractibleRange.Contains(Player.CurrentCell);

        private void OnStepEnded()
            => Prepare();
    }
}
