using Source.Tools.Math;

namespace Source.Input
{
	public interface IInputHandler
	{
		Vector2 CurrentPosition { get; }

		bool IsConfirmed { get; }

		void UpdateInput();
	}
}