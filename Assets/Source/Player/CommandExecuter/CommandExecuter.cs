using System;
using Source.Player.Commands;
using Source.Room;
using UnityEngine;

namespace Source.Player.CommandExecuter
{
    public class CommandExecuter : MonoBehaviour, ITurnHandler
    {
        private Command _currentCommand;
        private Turn _turn;
        private AbilityCommand _abilityCommandToReset;

        public event Action<Command> CommandChanged;

        public event Action AbilityUseFail;

        public event Action AbilityReseted;

        public Command CurrentCommand => _currentCommand;

        public bool TrySwitchCommand(Command command)
        {
            if (command is AbilityCommand abilityCommand)
            {
                if (abilityCommand.IsUsed)
                {
                    AbilityUseFail?.Invoke();
                    _abilityCommandToReset = abilityCommand;
                }
            }

            if (_currentCommand != null)
            {
                if (_currentCommand.GetType() == command.GetType() && command is not SkipCommand)
                {
                    if (_currentCommand is AbilityCommand)
                    {
                        if (_currentCommand.Enabled == false)
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }

            if (CanSwith() == false)
                return false;

            _currentCommand = command;
            CommandChanged?.Invoke(command);
            return true;
        }

        public void PrepareCommand()
        {
            if (_turn == Turn.Enemy)
                return;

            if (_currentCommand == null)
                return;

            StartCoroutine(_currentCommand.Execute());
        }

        public void ResetCommand()
        {
            _currentCommand = null;
            CommandChanged?.Invoke(_currentCommand);
        }

        public void ResetAbilityOnReward()
        {
            if (_abilityCommandToReset != null)
            {
                _abilityCommandToReset.Ability.ResetAbility();
                AbilityReseted?.Invoke();
            }

            _currentCommand = null;
        }

        public void SetTurn(Turn turn)
            => _turn = turn;

        private bool CanSwith()
        {
            if (_currentCommand != null && _currentCommand.IsExecuting)
                return false;

            return true;
        }
    }
}
