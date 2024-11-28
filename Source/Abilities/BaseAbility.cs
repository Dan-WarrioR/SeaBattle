using Source.Tools.Math;

namespace Source.Abilities
{
	public abstract class BaseAbility
	{
		public abstract void Apply(Vector2 position);

		public abstract void Reset();
	}
}