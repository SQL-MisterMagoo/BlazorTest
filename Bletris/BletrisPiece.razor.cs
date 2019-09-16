using Bletris.Model;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Bletris
{
    public class BletrisPieceModel : ComponentBase
	{

		[Parameter] public int Number { get; set; }
		[Parameter] public Piece Piece { get; set; }
		[Parameter] public bool IsPaused { get; set; }
		[Parameter] public int LastRow { get; set; }
		[Parameter] public Action<Piece> DeActivate { get; set; }
		[Parameter] public Action Refresh { get; set; }
		[Parameter] public bool DisplayOnly { get; set; }

		public bool IsActive => Piece?.Active ?? false;
		public int PositionX => Piece?.Position.x ?? 0;
		public int PositionY => Piece?.Position.y ?? 0;
		public int GridX => Piece?.Tetris?.GridX ?? 0;
		public int GridY => Piece?.Tetris?.GridY ?? 0;
		public int Width => Piece?.Tetris?.GridWidth ?? 0;
		public int Height => Piece?.Tetris?.GridHeight ?? 0;

		internal string Id;
		internal Task engine;

		protected override void OnInitialized()
		{
			Id = $"BL{DateTime.Now.Ticks}";

			if (LastRow == 0)
				LastRow = 20;

			if (IsActive)
			{
				engine = Engine();
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
				if (DisplayOnly)
				{
					Piece = new Piece(Number) { Active = false };
					StateHasChanged();
				}
			}
			catch { }
		}

		async Task Engine()
		{
			try
			{
				while (IsActive && Piece != null)
				{
					await Task.Delay(Piece.Delay);
					if (!IsPaused)
					{
						await Piece.SetPosition(Piece.Position.x, Piece.Position.y + 1, LastRow);
						if (Piece.Position.y + Piece.Tetris.GridHeight >= LastRow || !Piece.Active)
						{
							Console.WriteLine($"Piece has reached Row {Piece.Position.y + Piece.Tetris.GridHeight}");
							Piece.Active = false;
							DeActivate?.Invoke(Piece);
						}
						Refresh?.Invoke();
					}
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("******** ENGINE ERROR **********");
				Console.WriteLine(ex);
				throw;
			}
		}

		public async Task<bool> ProcessKeyEvent(KeyboardEventArgs args)
		{
			await KeyHandler(args);
			return true;
		}

		protected async Task<bool> KeyHandler(KeyboardEventArgs args)
		{
			try
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
						Piece.Delay = 100;
						break;
					case "ArrowUp":
					case "W":
					case "w":
						await Rotate(1);
						break;
					default:
						Console.WriteLine($"Key {args.Key} Code {args.Code}");
						break;
				};
				await Piece.SetPosition(Piece.Position.x + dx, Piece.Position.y, LastRow);

			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("******** MOVEMENT ERROR **********");
				Console.WriteLine(ex);
				throw;
			}
			return false;
		}

		async Task Rotate(int rotation)
		{
			try
			{
				if (rotation == 0 || !Piece.Active) return;
				Console.Write($"Starting Rotation {rotation}");

				int newRotation = (Piece.Rotation + 90) % 360;
				Console.Write($":{newRotation}deg");

				Tetris newTetris = Tetris.FromNumber(Number, newRotation);
				Console.Write($":Piece Pos X {Piece.Position.x}:Grid X {newTetris.GridX}: Grid H {newTetris.GridHeight}");

				int dx = 0;

				if (Piece.Position.x + newTetris.GridX < 3)
				{
					dx = 3 - (Piece.Position.x + newTetris.GridX);
				}
				else if (Piece.Position.x + newTetris.GridWidth > 13)
				{
					dx = 13 - (Piece.Position.x + newTetris.GridX);
				}
				if (Piece.Position.x + newTetris.GridHeight >= LastRow)
				{
					Console.WriteLine($"rotation not possible : Line {Piece.Position.x + newTetris.GridHeight}");
				}
				Console.Write($":dx {dx}");

				Piece.Rotation = newRotation;
				await Piece.SetTetris(newTetris);
				await Piece.SetPosition(Piece.Position.x + dx, Piece.Position.y, LastRow);

				Console.WriteLine();

			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("******** ROTATION ERROR **********");
				Console.WriteLine(ex);
				throw;
			}
		}
	}
}
