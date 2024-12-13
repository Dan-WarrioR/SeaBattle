using Source.MapGeneration;
using Source.Rendering;
using Source.Users;
using Source.Abilities;

namespace Source.Core
{
	public enum RoundResult
	{
		Player1Win,
		Player2Win,
	}

	public class Round
    {
        public RoundResult RoundResult { get; private set; }  

        private Renderer _renderer;

        private Player _player1;
        private Player _player2;
        private Player _currentPlayer;

        private bool _onePlayerLostAllShips = false;
        private bool _isShipBombed = false;

        private List<BaseAbility> _activeAbilities = new();

        public Round(Player player1, Player player2)
        {
			_player1 = player1;
			_player2 = player2;

			_player1.Map.OnCellBombed += OnBombedCell;
			_player2.Map.OnCellBombed += OnBombedCell;

			_renderer = new(_player1, _player2);

			_currentPlayer = _player1;
		}

		public void PlayRound()
        {
			Draw();

            while (!IsRoundEnd())
            {
                CalculateInput();
                ProcessTurn();
                Draw();
            }

            EndRound();
		}     

        private void CalculateInput()
        {
            _currentPlayer.CalculateInput();
        }

        private void ProcessTurn()
        {
            ResetPreviousActiveAbilities();

            ProcessAbility();

            if (!_currentPlayer.IsConfirmed)
            {
                return;
            }

            _currentPlayer.TryBombCell();

            if (!_isShipBombed)
            {
                SwitchTurn();

                _isShipBombed = false;
            }

            _currentPlayer.ResetCurrentAbility();
        }

        private void ProcessAbility()
        {
            var ability = _currentPlayer.GetSelectedAbility();

            if (ability != null)
            {
                ability.Apply(_currentPlayer.CurrentPosition);

                _activeAbilities.Add(ability);
            }
        }

        private void ResetPreviousActiveAbilities()
        {
            foreach (var ability in _activeAbilities)
            {
                ability.Reset();
            }

            _activeAbilities.Clear();
        }

        private void SwitchTurn()
        {
            _currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
        }

        private void OnBombedCell(Cell cell)
        {
            _isShipBombed = cell.IsShip;

            _onePlayerLostAllShips = _player1.ShipsCount <= 0 || _player2.ShipsCount <= 0;
        }

        private void EndRound()
        {
            RoundResult = _player1.ShipsCount <= 0 ? RoundResult.Player1Win : RoundResult.Player2Win;
		}



        private bool IsRoundEnd()
        {
            return _onePlayerLostAllShips;
        }



        private void Draw()
        {
            _renderer.Draw();
        }
    }
}