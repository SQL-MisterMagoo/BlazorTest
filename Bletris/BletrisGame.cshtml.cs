using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bletris
{
	public abstract class BletrisGameModel : BlazorComponent
	{
		/*
		 * TODO: 
		 *	Tidy up / refactor
		 *	Scoring
		 *	Speed up
		 *	Collision detection
		 *	Remove completed lines
		 *	Implement Next Piece properly
		 *	Find copyright free game music?
		 *	Game Over
		 *	New Game
		 *	High Scores
		 *	Fluxor ?
		*/
		[Parameter] protected int InitialDelay { get; set; }

		public int NextPieceId { get; set; }
		public bool IsPaused { get; set; }

		public int Score { get; private set; }
		internal List<Piece> Pieces { get; private set; }
		internal BletrisPiece ActivePiece { get; set; }
		internal BletrisPiece NextPieceRef { get; set; }
		internal List<Piece.Point> UsedPoints { get; private set; }

		Random r = new Random();
		Task engine;
		public string BtnValue="Pause";

		public BletrisGameModel()
		{
		}

		protected override void OnInit()
		{
			if (InitialDelay < 50) InitialDelay = 200;
			engine = RunGame();
			UsedPoints = new List<Piece.Point>();
		}

		private async Task RunGame()
		{
			Score = 0;
			Pieces = new List<Piece>();

			try
			{
				do
				{
					if (!IsPaused)
					{
						{
							Pieces.Add(new Piece(Math.Max(NextPiece, 1))
							{
								Active = true,
								Delay = InitialDelay,
								Map = UsedPoints
							});
						}
						Refresh();
						while (Pieces.Any(p => p.Active))
						{
							await Task.Delay(100);
						}
					}
					else
					{
						await Task.Delay(100);
					}
				} while (!Pieces.Any(p => p.Position.y == 1 && p.Active == false));

			}
			catch (Exception ex)
			{
				Console.WriteLine();
				Console.WriteLine("******** GAME LOOP ERROR **********");
				Console.WriteLine(ex);
				throw;
			}
		}

		public int NextPiece
		{
			get
			{
				int pieceid = NextPieceId;
				NextPieceId = r.Next(1, 7);
				return pieceid;
			}
		}

		public virtual void PauseGame(UIFocusEventArgs args)
		{
			IsPaused = true;
		}

		public virtual void ResumeGame(UIFocusEventArgs args)
		{
			IsPaused = false;
		}

		public virtual void BtnPause(UIMouseEventArgs args)
		{
			if (BtnValue=="Pause")
			{
				BletrisInterop.SetFocus("bletris_delay");
				IsPaused = true;
				BtnValue = "Resume";
			} else
			{
				BtnValue = "Pause";
			}
		}

		protected void PieceDeActivate(Piece piece)
		{
			UpdateMap(piece);
			ScorePoints();
			Refresh();
		}

		protected void Refresh()
		{
			StateHasChanged();
		}

		void UpdateMap(Piece piece)
		{
			var points = new List<Piece.Point>();
			if (!piece.Active)
			{
				UsedPoints.AddRange(piece.Tetris.Geos.Select(g => new Piece.Point(g.x + piece.Position.x, g.y + piece.Position.y,piece.Tetris.Colour)));
			}
			UsedPoints.OrderBy(p => p.y*10 + p.x).ToList().ForEach(p => Console.WriteLine($"Map: {p.x},{p.y}"));
		}

		async Task ScorePoints()
		{
			//40 * (n + 1)	100 * (n + 1)	300 * (n + 1)	1200 * (n + 1)
			// n= Level
			int lineCount = 0;
			List<int> lines = new List<int>();
			//Go down the lines
			for (int i = 1; i < 21; i++)
			{
				int hitCount = UsedPoints.Where(x => x.y == i).Count();
				if (hitCount == 10) //Number of columns
				{
					lines.Add(i);
					UsedPoints = UsedPoints.Select(p => new Piece.Point(p.x, p.y, p.Colour, p.y == i ? "dying" : "")).ToList();
				}
			}
			StateHasChanged();
			await Task.Delay(650);
			UsedPoints.RemoveAll(p => p.Class == "dying");
			foreach (var item in lines)
			{
					UsedPoints = UsedPoints.Select(p => new Piece.Point(p.x, p.y + (p.y < item ? 1 : 0), p.Colour)).ToList();
			}
			lineCount = lines.Count;
			if (lineCount > 0)
			{
				Score += lineCount == 1 ? 40 : lineCount == 2 ? 100 : lineCount == 3 ? 300 : 1200;
			}
		}
	}

}
