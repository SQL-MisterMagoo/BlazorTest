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
		[Parameter] protected Piece Piece { get; set; }
		[Parameter] protected int Delay { get; set; }
		[Parameter] protected bool IsPaused { get; set; }
		[Parameter] protected int LastRow { get; set; }
		[Parameter] protected Action<Piece> DeActivate { get; set; }
		[Parameter] protected Action Refresh { get; set; }

		public bool IsActive => Piece?.Active ?? false;
		public int PositionX => Piece?.Position.x ?? 0;
		public int PositionY => Piece?.Position.y ?? 0;
		public int GridX => tetris?.GridX ?? 0;
		public int GridY => tetris?.GridY ?? 0;
		public int Width => tetris?.GridWidth ?? 0;
		public int Height => tetris?.GridHeight ?? 0;

		internal string Id;
		internal Task engine;
		public Tetris tetris;

		protected override void OnInit()
		{
			Id = $"BL{DateTime.Now.Ticks}";

			if (Delay == 0)
				Delay = 1000;

			if (LastRow == 0)
				LastRow = 20;

			tetris = Tetris.FromNumber(Number, Piece?.Rotation ?? 0);

			if (IsActive)
			{
				engine = Task.Factory.StartNew(Engine, TaskCreationOptions.LongRunning);
			}
			else
			{
				Task.Delay(4);
			}
		}

		protected override void OnParametersSet()
		{
			base.OnParametersSet();
			try
			{
				if (Piece == null)
				{
					tetris = Tetris.FromNumber(Number, Piece?.Rotation ?? 0);
					StateHasChanged();
				}
			}
			catch { }
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
						StateHasChanged();
						Refresh?.Invoke(); // not required for gameplay - just debugging
						if (Piece.Position.y >= LastRow - tetris.GridHeight)
						{
							Piece.Active = false;
							break;
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
						Delay = 100;
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
					if (Piece.Position.x + dx + tetris.GridX < 3 || Piece.Position.x + dx + tetris.GridWidth > 12)
					{
						dx = 0;
					}
					Piece.Position = (Piece.Position.x + dx, Piece.Position.y);
				}
			});
			return false;
		}
		void Rotate(int rotation)
		{
			if (rotation == 0) return;
			int newRotation = (Piece.Rotation + 90) % 360;
			Tetris newTetris = Tetris.FromNumber(Number, newRotation);
			int dx = 0;
			lock (Piece)
			{
				if (Piece.Position.x + newTetris.GridX < 3)
				{
					dx = 3 - (Piece.Position.x + newTetris.GridX);
				}
				else if (Piece.Position.x + newTetris.GridWidth > 12)
				{
					dx = 12 - (Piece.Position.x + newTetris.GridX);
				}
				Piece.Rotation = newRotation;
				Piece.Position = (Piece.Position.x + dx, Piece.Position.y);
			}
			tetris = newTetris;
			StateHasChanged();
		}
	}
}
