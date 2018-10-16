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
			Score += 50;
			UpdateMap();
			Refresh();
		}

		protected void Refresh()
		{
			StateHasChanged();
		}

		void UpdateMap()
		{
			var points = new List<Piece.Point>();
			Pieces.ForEach(p => 
			{
				if (!p.Active)
				{
					points.AddRange(p.Tetris.Geos.Select(g => new Piece.Point(g.x + p.Position.x, g.y + p.Position.y)));
				}
			});
			UsedPoints = points.Distinct().ToList();
			UsedPoints.OrderBy(p => p.y*10 + p.x).ToList().ForEach(p => Console.WriteLine($"Map: {p.x},{p.y}"));

		}
	}

}
