using System;

namespace Bletris
{
	public class Piece
	{
		public string Name { get; set; }
		public (int x, int y) Position { get; set; }
		public (int x, int y) Size { get; set; }
		public bool Active { get; set; }
		public int Number { get; set; }
		public int PieceNumber { set { Number = value; Name = PieceName(value); Size = PieceSize(value); } }
		Random r = new Random();

		public Piece(int number)
		{
			PieceNumber = number;
			Position = (6, 1);
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