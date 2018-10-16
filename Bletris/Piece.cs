using Bletris.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bletris
{
	public class Piece
	{
		public struct Point
		{
			public int x;
			public int y;
			public string Colour;
			public string Class;
			public Point(int px, int py, string clr = "", string cls = "")
			{
				x = px;
				y = py;
				Colour = clr;
				Class = cls;
			}
		}
		public string Name { get; set; }
		public Point Position { get { return _position; } }
		public bool Active { get; set; }
		public int Number { get; set; }
		public int Delay { get; set; }
		public int PieceNumber { set { Number = value; Name = PieceName(value); } }
		public int Rotation { get; set; }
		public Tetris Tetris { get { return _tetris; } }
		public List<Point> Map { get; set; }

		static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

		private Point _position = new Point(6, 1);
		private Tetris _tetris;

		public Piece(int number)
		{
			PieceNumber = number;
			_tetris = Tetris.FromNumber(number, 0);
			Task.Run(() => SetPosition(6, 1, 1));
		}

		public async Task<bool> SetPosition(int x, int y, int LastRow)
		{
			if (y + Tetris.GridHeight > LastRow) return false;
			if (x + Tetris.GridX < 3) return false;
			if (x + Tetris.GridWidth > 13) return false;

			if (Map != null)
			{
				foreach (var g in Tetris.Geos)
				{
					var hits = Map.Where(m => m.x == g.x + x && m.y == g.y + y);
					if (hits != null)
					{
						foreach (var hit in hits)
						{
							Console.WriteLine($"Point {g.x},{g.y} has hit point {hit.x},{hit.y}.");
							if (y == Position.y)
							{
								Console.WriteLine($"Piece cannot move sideways.");
								//Sideways movement so we can just say no
								return false;
							}
							Active = false;
							return false;
						}
					}
				}
			}

			await semaphoreSlim.WaitAsync();
			try
			{
				_position.x = x;
				_position.y = y;
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
			finally
			{
				semaphoreSlim.Release();
			}
		}

		public async Task<bool> SetTetris(Tetris tetris)
		{
			await semaphoreSlim.WaitAsync();
			try
			{
				_tetris = tetris;
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				return false;
			}
			finally
			{
				semaphoreSlim.Release();
			}
		}

		string PieceName(int number)
		{
			switch (number)
			{
				case 1:
					return "I";
				case 2:
					return "J";
				case 3:
					return "L";
				case 4:
					return "O";
				case 5:
					return "S";
				case 6:
					return "T";
				default:
					return "Z";
			}

		}

		(int x, int y) PieceSize(int number)
		{
			switch (number)
			{
				case 1:
					return (4, 1);
				case 2:
					return (3, 2);
				case 3:
					return (3, 2);
				case 4:
					return (2, 2);
				case 5:
					return (3, 2);
				case 6:
					return (3, 2);
				default:
					return (3, 2);
			}

		}

	}
}