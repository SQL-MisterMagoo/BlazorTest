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
		[Parameter] protected Uri Tetris_I { get; set; }
		[Parameter] protected Uri Tetris_J { get; set; }
		[Parameter] protected Uri Tetris_L { get; set; }
		[Parameter] protected Uri Tetris_O { get; set; }
		[Parameter] protected Uri Tetris_S { get; set; }
		[Parameter] protected Uri Tetris_T { get; set; }
		[Parameter] protected Uri Tetris_Z { get; set; }
		[Parameter] protected int Delay { get; set; }

		public int Score { get; private set; }
		internal List<Piece> Pieces { get; private set; }
		Random r = new Random();
		Task engine;
		public string BtnValue="Pause";

		public BletrisModel()
		{
			Tetris_I = new Uri("https://upload.wikimedia.org/wikipedia/commons/f/f4/Tetris_I.svg");
			Tetris_J = new Uri("https://upload.wikimedia.org/wikipedia/commons/2/25/Tetris_J.svg");
			Tetris_L = new Uri("https://upload.wikimedia.org/wikipedia/commons/8/81/Tetris_L.svg");
			Tetris_O = new Uri("https://upload.wikimedia.org/wikipedia/commons/8/82/Tetris_O.svg");
			Tetris_S = new Uri("https://upload.wikimedia.org/wikipedia/commons/7/7b/Tetris_S.svg");
			Tetris_T = new Uri("https://upload.wikimedia.org/wikipedia/commons/9/91/Tetris_T.svg");
			Tetris_Z = new Uri("https://upload.wikimedia.org/wikipedia/commons/3/33/Tetris_Z.svg");
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
					Pieces.Add(new Piece(Math.Max(NextPieceId,1))
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

		public string NextPiece
		{
			get
			{
				NextPieceId = r.Next(1, 7);
				switch (NextPieceId)
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
