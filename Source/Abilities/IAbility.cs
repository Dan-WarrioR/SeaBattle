using Source.Tools.Math;

namespace Source.Abilities
{
	public interface IAbility
	{
		public bool IsInfinityUse { get; }

		public int UsesCount { get; }

		public void Apply(Vector2 position);

		public void Reset();
	}
}