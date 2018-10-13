using Bletris.Model;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bletris
{
	public class BletrisPieceModel : BlazorComponent
	{

		[Parameter] protected int Number { get; set; }
		[Parameter] protected bool IsActive { get; set; }
		[Parameter] protected Piece Piece { get; set; }
		[Parameter] protected int Delay { get; set; }
		[Parameter] protected bool IsPaused { get; set; }

		public int PositionX => Piece.Position.x;
		public int PositionY => Piece.Position.y;
		public int GridX => tetris.GridX;
		public int GridY => tetris.GridY;
		public int Width => tetris.GridWidth;
		public int Height => tetris.GridHeight;

		internal string Id;
		internal int Rotation;
		internal Task engine;
		public Tetris tetris;

		protected override void OnInit()
		{
			Id = $"BL{DateTime.Now.Ticks}";

			if (Delay == 0)
				Delay = 1000;

			tetris = Tetris.FromNumber(Number, Rotation);

			if (IsActive)
				engine = Engine();
		}

		async Task Engine()
		{
			while (IsActive && Piece != null)
			{
				await Task.Delay(Delay);
				if (!IsPaused)
				{
					lock (Piece)
					{
						Piece.Position = (Piece.Position.x, Piece.Position.y + 1);

						if (Piece.Position.y >= 20 - tetris.GridHeight)
						{
							Piece.Active = false;
							IsActive = false;
						}
					}
				}
			}
		}

		protected override async Task OnAfterRenderAsync()
		{
			if (IsActive && !IsPaused)
			{
				await BletrisInterop.SetFocus(Id);
			}
		}

		protected async Task<bool> KeyHandler(UIKeyboardEventArgs args)
		{
			await Task.Run(() =>
			{
				int dx = 0;
				switch (args.Key)
				{
					case "ArrowLeft":
					case "A":
					case "a":
						dx = -1;
						break;
					case "ArrowRight":
					case "D":
					case "d":
						dx = 1;
						break;
					case "ArrowDown":
					case "S":
					case "s":
						Delay = 10;
						break;
					case "ArrowUp":
					case "W":
					case "w":
						Rotate(1);
						break;
					default:
						Console.WriteLine($"Key {args.Key} Code {args.Code}");
						break;
				};
				lock (Piece)
				{
					//if (Piece.Position.x + dx + tetris.GridX < 1 || Piece.Position.x + dx + tetris.GridWidth > 10)
					//{
					//	dx = 0;
					//}
					Piece.Position = (Piece.Position.x + dx, Piece.Position.y);
				}
			});
			return false;
		}
		void Rotate(int rotation)
		{
			if (rotation == 0) return;
			Rotation = (Rotation + 90) % 360;
			tetris = Tetris.FromNumber(Number, Rotation);
		}
	}
}
