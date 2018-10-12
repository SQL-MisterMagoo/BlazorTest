using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bletris
{
	public abstract class BletrisModel : BlazorComponent
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
		[Parameter] protected int Delay { get; set; }

		public int Score { get; private set; }
		internal List<Piece> Pieces { get; private set; }
		Random r = new Random();
		Task engine;
		public string BtnValue="Pause";

		public BletrisModel()
		{
		}

		protected override void OnInit()
		{
			engine = RunGame();
			if (Delay < 50) Delay = 200;
		}

		private async Task RunGame()
		{
			Score = 0;
			Pieces = new List<Piece>();

			do
			{
				if (!IsPaused)
				{
					Pieces.Add(new Piece(Math.Max(NextPiece,1))
					{
						Active = true,
					});
					Score += 50;
					Refresh();
					while (Pieces.Any(p => p.Active))
					{
						await Task.Delay(500);
					}
				}
				else
				{
					await Task.Delay(500);
				}
			} while (!Pieces.Any(p => p.Position.y == 1 && p.Active == false));
		}

		public abstract void Refresh();

		public int NextPiece
		{
			get
			{
				int pieceid = NextPieceId;
				NextPieceId = r.Next(1, 7);
				Refresh();
				return pieceid;
			}
		}

		public bool IsPaused { get; private set; }
		public int NextPieceId { get; private set; }

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
	}

}
