using System;

namespace Bletris
{
	public class Piece
	{
		public string Url { get; set; }
		public (int x, int y) Position { get; set; }
		public (int x, int y) Size { get; set; }
		public bool Active { get; set; }
		public int PieceNumber { set { Url = PieceUrl(value); Size = PieceSize(value); } }
		public Uri Tetris_I { get; set; }
		public Uri Tetris_J { get; set; }
		public Uri Tetris_L { get; set; }
		public Uri Tetris_O { get; set; }
		public Uri Tetris_S { get; set; }
		public Uri Tetris_T { get; set; }
		public Uri Tetris_Z { get; set; }
		Random r = new Random();

		public Piece(int number)
		{
			Tetris_I = new Uri("https://upload.wikimedia.org/wikipedia/commons/f/f4/Tetris_I.svg");
			Tetris_J = new Uri("https://upload.wikimedia.org/wikipedia/commons/2/25/Tetris_J.svg");
			Tetris_L = new Uri("https://upload.wikimedia.org/wikipedia/commons/8/81/Tetris_L.svg");
			Tetris_O = new Uri("https://upload.wikimedia.org/wikipedia/commons/8/82/Tetris_O.svg");
			Tetris_S = new Uri("https://upload.wikimedia.org/wikipedia/commons/7/7b/Tetris_S.svg");
			Tetris_T = new Uri("https://upload.wikimedia.org/wikipedia/commons/9/91/Tetris_T.svg");
			Tetris_Z = new Uri("https://upload.wikimedia.org/wikipedia/commons/3/33/Tetris_Z.svg");
			PieceNumber = number;
			Position = (r.Next(1, 10 - Size.x), 1);
		}

		string PieceUrl(int number)
		{
			switch (number)
			{
				case 1:
					return Tetris_I.AbsoluteUri;
				case 2:
					return Tetris_J.AbsoluteUri;
				case 3:
					return Tetris_L.AbsoluteUri;
				case 4:
					return Tetris_O.AbsoluteUri;
				case 5:
					return Tetris_S.AbsoluteUri;
				case 6:
					return Tetris_T.AbsoluteUri;
				default:
					return Tetris_Z.AbsoluteUri;
			}

		}

		(int x, int y) PieceSize(int number)
		{
			switch (number)
			{
				case 1:
					return (4, 1);
				case 2:
					return (4, 2);
				case 3:
					return (4, 2);
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