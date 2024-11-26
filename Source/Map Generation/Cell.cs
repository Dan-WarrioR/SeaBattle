namespace Source.MapGeneration
{
	public class Cell
	{
		public bool IsShip { get; private set; } = false;
		public bool IsDestroyed { get; private set; } = false;

		public bool IsScaned { get; set; } = false;

		public event Action<Cell> OnBombed;

		public void PlaceShip()
		{
			IsShip = true;
		}
		
		public void BombCell()
		{
			if (IsDestroyed)
			{
				return;
			}

			IsDestroyed = true;

			OnBombed?.Invoke(this);
		}

		public bool CanBombCell()
		{
			return !IsDestroyed;
		}
	}
}