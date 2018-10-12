using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bletris.Model
{
	public class Tetris
	{
		string _name;
		List<(int x, int y)> _geos;
		private int _gridHeight;
		private int _gridX;
		private int _gridY;
		private int _gridWidth;
		string _colour;
		int _width;
		int _height;

		public string Name => _name;
		public List<(int x, int y)> Geos => _geos;
		public string Colour => _colour;
		public int Width => _width;
		public int Height => _height;
		public int GridWidth => _gridWidth;
		public int GridHeight => _gridHeight;
		public int GridX => _gridX;
		public int GridY => _gridY;

		public Tetris(string name, string colour, List<(int x, int y)> geos)
		{
			_name = name;
			_colour = colour;
			_geos = geos;
			_gridWidth = 1+Geos.Max(g => g.x);
			_gridHeight = 1+Geos.Max(g => g.y);
			_gridX = Geos.Min(g => g.x);
			_gridY = Geos.Min(g => g.y);
			_width = 32 * _gridWidth;
			_height = 32 * _gridHeight;
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
							return new Tetris("L", "orange", new List<(int x, int y)>() { (2, 0), (0, 1), (1, 1), (2, 1) });
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
					return Tetris.FromName("I",rotation);
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

	}
}
