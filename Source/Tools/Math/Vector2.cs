namespace Source.Tools.Math
{
    public struct Vector2
    {
        public static Vector2 Zero => new(0, 0);
        public static Vector2 One => new(1, 1);

        public int X { get; }
        public int Y { get; }

		public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

		public static Vector2 operator +(Vector2 first, Vector2 second)
		{
			return new (first.X + second.X, first.Y + second.Y);
		}

		public static Vector2 operator -(Vector2 first, Vector2 second)
		{
			return new (first.X - second.X, first.Y - second.Y);
		}

		public static explicit operator Vector2((int X, int Y) tuple)
		{
			return new (tuple.X, tuple.Y);
		}
	}
}