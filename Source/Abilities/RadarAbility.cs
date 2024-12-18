﻿using Source.MapGeneration;
using Source.Tools.Math;

namespace Source.Abilities
{
	public class RadarAbility : BaseAbility
	{
		private const int ScanSquareSize = 4;

		public bool IsInfinityUse { get; } = false;

		public int UsesCount { get; private set; } = 1;

		private Vector2 _playerPosition;

		private Map _map;

		public RadarAbility(Map map)
		{
			_map = map;
		}

		public override void Apply(Vector2 position)
		{
			if (!IsInfinityUse && UsesCount <= 0)
			{
				return;
			}

			UsesCount--;

			_playerPosition = position;

			ChangeCellVisibility(true);
		}

		public override void Reset()
		{
			ChangeCellVisibility(false);
		}	

		public void ChangeCellVisibility(bool isScanned)
		{
			int scanSqauareSize = Math.Clamp(ScanSquareSize - 2, 0, _map.Size);

			for (int x = -scanSqauareSize; x <= scanSqauareSize; x++)
			{
				for (int y = -scanSqauareSize; y <= scanSqauareSize; y++)
				{
					var position = new Vector2(_playerPosition.X + x, _playerPosition.Y + y);

					if (_map.TryGetCell(position.X, position.Y, out Cell cell))
					{
						cell.IsScaned = isScanned;
					}
				}
			}
		}
	}
}
