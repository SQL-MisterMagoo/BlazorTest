using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bletris.Model
{
	public class Tetris
	{
		public string Name { get; }
		public List<(int x, int y)> Geos { get; }
		public string Colour { get; private set; }
		public int Width { get; }
		public int Height { get; }
		public int GridWidth { get; }
		public int GridHeight { get; }
		public int GridX { get; }
		public int GridY { get; }
		public string Colour1 { get => Colour; set => Colour = value; }

		public Tetris(string name, string colour, List<(int x, int y)> geos)
		{
			Name = name;
			Colour = colour;
			Geos = geos;
			GridWidth = 1 + Geos.Max(g => g.x);
			GridHeight = 1 + Geos.Max(g => g.y);
			GridX = Geos.Min(g => g.x);
			GridY = Geos.Min(g => g.y);
			Width = 32 * GridWidth;
			Height = 32 * GridHeight;
		}

		public static Tetris FromName(string name, int rotation)
		{
			switch (name)
			{
				case "I":
					switch (rotation)
					{
						case 0:
							return new Tetris("I", "cyan", new List<(int x, int y)>() { (0, 1), (1, 1), (2, 1), (3, 1) });
						case 90:
							return new Tetris("I", "cyan", new List<(int x, int y)>() { (2, 0), (2, 1), (2, 2), (2, 3) });
						case 180:
							return new Tetris("I", "cyan", new List<(int x, int y)>() { (0, 2), (1, 2), (2, 2), (3, 2) });
						default:
							return new Tetris("I", "cyan", new List<(int x, int y)>() { (1, 0), (1, 1), (1, 2), (1, 3) });
					}
				case "J":
					switch (rotation)
					{
						case 0:
							return new Tetris("J", "blue", new List<(int x, int y)>() { (0, 0), (0, 1), (1, 1), (2, 1) });
						case 90:
							return new Tetris("J", "blue", new List<(int x, int y)>() { (1, 0), (2, 0), (1, 1), (1, 2) });
						case 180:
							return new Tetris("J", "blue", new List<(int x, int y)>() { (2, 2), (0, 1), (1, 1), (2, 1) });
						default:
							return new Tetris("J", "blue", new List<(int x, int y)>() { (1, 0), (0, 2), (1, 1), (1, 2) });
					}
				case "L":
					switch (rotation)
					{
						case 0:
							return new Tetris("L", "orange", new List<(int x, int y)>() { (2, 0), (0, 1), (1, 1), (2, 1) });
						case 90:
							return new Tetris("L", "orange", new List<(int x, int y)>() { (1, 0), (2, 2), (1, 1), (1, 2) });
						case 180:
							return new Tetris("L", "orange", new List<(int x, int y)>() { (0, 2), (0, 1), (1, 1), (2, 1) });
						default:
							return new Tetris("L", "orange", new List<(int x, int y)>() { (1, 0), (0, 0), (1, 1), (1, 2) });
					}
				case "O":
					return new Tetris("O", "yellow", new List<(int x, int y)>() { (1, 0), (2, 0), (1, 1), (2, 1) });
				case "S":
					switch (rotation)
					{
						case 0:
							return new Tetris("S", "lightgreen", new List<(int x, int y)>() { (1, 0), (2, 0), (0, 1), (1, 1) });
						case 90:
							return new Tetris("S", "lightgreen", new List<(int x, int y)>() { (1, 0), (2, 2), (2, 1), (1, 1) });
						case 180:
							return new Tetris("S", "lightgreen", new List<(int x, int y)>() { (1, 1), (2, 1), (0, 2), (1, 2) });
						default:
							return new Tetris("S", "lightgreen", new List<(int x, int y)>() { (0, 0), (1, 2), (0, 1), (1, 1) });
					}
				case "T":
					switch (rotation)
					{
						case 0:
							return new Tetris("T", "purple", new List<(int x, int y)>() { (1, 0), (0, 1), (1, 1), (2, 1) });
						case 90:
							return new Tetris("T", "purple", new List<(int x, int y)>() { (1, 0), (1, 2), (1, 1), (2, 1) });
						case 180:
							return new Tetris("T", "purple", new List<(int x, int y)>() { (1, 2), (0, 1), (1, 1), (2, 1) });
						default:
							return new Tetris("T", "purple", new List<(int x, int y)>() { (1, 0), (0, 1), (1, 1), (1, 2) });
					}
				case "Z":
				default:
					switch (rotation)
					{
						case 0:
							return new Tetris("Z", "red", new List<(int x, int y)>() { (0, 0), (1, 0), (1, 1), (2, 1) });
						case 90:
							return new Tetris("Z", "red", new List<(int x, int y)>() { (1, 2), (2, 0), (1, 1), (2, 1) });
						case 180:
							return new Tetris("Z", "red", new List<(int x, int y)>() { (0, 1), (1, 2), (1, 1), (2, 2) });
						default:
							return new Tetris("Z", "red", new List<(int x, int y)>() { (0, 2), (1, 0), (0, 1), (1, 1) });
					}
			}
		}

		public static Tetris FromNumber(int number, int rotation)
		{
			switch (number)
			{
				case 1:
					return Tetris.FromName("I", rotation);
				case 2:
					return Tetris.FromName("J", rotation);
				case 3:
					return Tetris.FromName("L", rotation);
				case 4:
					return Tetris.FromName("O", rotation);
				case 5:
					return Tetris.FromName("S", rotation);
				case 6:
					return Tetris.FromName("T", rotation);
				case 7:
				default:
					return Tetris.FromName("Z", rotation);
			}
		}

		public static class WallKick
		{
			public static List<(int x, int y)> GetTests(string name, int from, int to)
			{
				if (name == "J" || name == "L" || name == "S" || name == "T" || name == "Z")
				{
					switch (10 * from + to)
					{
						case 1: return new List<(int x, int y)>() { (0, 0), (-1, 0), (-1, 1), (0, -2), (-1, -2) };
						case 10: return new List<(int x, int y)>() { (0, 0), (1, 0), (1, -1), (0, 2), (1, 2) };
						case 12: return new List<(int x, int y)>() { (0, 0), (1, 0), (1, -1), (0, 2), (1, 2) };
						case 21: return new List<(int x, int y)>() { (0, 0), (-1, 0), (-1, 1), (0, -2), (-1, -2) };
						case 23: return new List<(int x, int y)>() { (0, 0), (1, 0), (1, 1), (0, -2), (1, -2) };
						case 32: return new List<(int x, int y)>() { (0, 0), (-1, 0), (-1, -1), (0, 2), (-1, 2) };
						case 30: return new List<(int x, int y)>() { (0, 0), (-1, 0), (-1, -1), (0, 2), (-1, 2) };
						case 3: return new List<(int x, int y)>() { (0, 0), (1, 0), (1, 1), (0, -2), (1, -2) };
					}
				}
				else if (name=="I")
				{
					switch (10 * from + to)
					{
						case 1: return new List<(int x, int y)>() { (0, 0), (-2, 0), (1, 0), (-2, -1), (1, 2) };
						case 10: return new List<(int x, int y)>() { (0, 0), (2, 0), (-1, 0), (2, 1), (-1, -2) };
						case 12: return new List<(int x, int y)>() { (0, 0), (-1, 0), (2, 0), (-1, 2), (2, -1) };
						case 21: return new List<(int x, int y)>() { (0, 0), (1, 0), (-2, 0), (1, -2), (-2, 1) };
						case 23: return new List<(int x, int y)>() { (0, 0), (2, 0), (-1, 0), (2, 1), (-1, -2) };
						case 32: return new List<(int x, int y)>() { (0, 0), (-2, 0), (1, 0), (-2, -1), (1, 2) };
						case 30: return new List<(int x, int y)>() { (0, 0), (1, 0), (-2, 0), (1, -2), (-2, 1) };
						case 3: return new List<(int x, int y)>() { (0, 0), (-1, 0), (2, 0), (-1, 2), (2, -1) };
					}
				}
				return null;
			}
		}
	}
}
