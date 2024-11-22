using Source.Tools.Math;

namespace Source.Input
{
	public interface IInputHandler
	{
		public Vector2? CurrentPosition { get; }

		public Vector2 FuturePosition { get; }

		public void UpdateInput();

		public void ResetInput();
	}
}