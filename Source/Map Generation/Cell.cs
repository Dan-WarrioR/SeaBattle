using Source.Configs;

namespace Source.MapGeneration
{
	public enum CellState
	{
		Empty,
		Ship,
		Hit,
		Miss
	}
	
	public class Cell
	{
		public CellState State { get; private set; } = CellState.Empty;

		public event Action<CellState> OnBombed;

		public Cell()
		{
			State = CellState.Empty;
		}

		public void PlaceShip()
		{
			State = CellState.Ship;
		}
		
		public void BombCell()
		{
			State = State == CellState.Ship ? CellState.Hit : CellState.Miss;

			OnBombed?.Invoke(State);
		}

		public bool CanBombCell()
		{
			return State == CellState.Empty || State == CellState.Ship;
		}
	}
}
