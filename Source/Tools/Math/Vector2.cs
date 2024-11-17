namespace Source.Tools.Math
{
    public struct Vector2
    {
        public static Vector2 Zero => new(0, 0);
        public static Vector2 One => new(1, 1);

        public int X;
        public int Y;

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

		public static Vector2 operator +(Vector2 first, Vector2 second)
		{
			return new Vector2(first.X + second.X, first.Y + second.Y);
		}

		public static Vector2 operator -(Vector2 first, Vector2 second)
		{
			return new Vector2(first.X - second.X, first.Y - second.Y);
		}

		public static Vector2 operator +(Vector2 first, (int X, int Y) tuple)
		{
			return new Vector2(first.X + tuple.X, first.Y + tuple.Y);
		}
	}
}