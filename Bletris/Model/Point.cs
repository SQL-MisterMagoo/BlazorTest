namespace Bletris
{
	public class Point
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

}