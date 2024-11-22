namespace Source.Tools.Math
{
	public struct Border
	{
		public int X { get; }
		public int Y { get; }
		public int Z { get; }
		public int W { get; }

		public Border(int x, int y, int z, int w)
		{
			X = x;
			Y = y;
			Z = z;
			W = w;
		}
	}
}
