using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Threading.Tasks;

namespace BlazorFlying.Model
{
	public class FlyingModel : BlazorComponent
	{
		[Parameter]
		internal string Image { get; set; }
		[Parameter]
		internal bool Debug { get; set; }
		[Parameter]
		internal double X { get; set; }
		[Parameter]
		internal double Y { get; set; }
		[Parameter]
		internal double Speed { get; set; }
		[Parameter]
		internal double frameDelay { get; set; }

		internal (double x, double y) Position;
		internal (double x, double y) NewPosition;
		internal int direction;
		private Random random = new Random();
		private long lastTime;		

		private async Task<bool> StartAnimation()
		{

			lastTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
			while (true)
			{
				await Task.Delay((int)frameDelay);

				if (TuplesAreClose(Position, NewPosition))
				{
					NewPosition = NewRandomPosition();
					Console.WriteLine($"Aiming for {NewPosition.x:N2} , {NewPosition.y:N2}");
				}

				Update();

			}
		}

		protected override void OnInit()
		{
			base.OnInit();
			Position = (X, Y);
			NewPosition = Position;
			frameDelay = frameDelay == 0 ? 40 : frameDelay;
			StartAnimation().ConfigureAwait(false);
			return;
		}

		private (double x, double y) NewRandomPosition()
		{
			double rx = random.NextDouble() * 70;
			double ry = random.NextDouble() * 70;
			return (rx, ry);
		}

		private bool TuplesAreClose((double x, double y) A, (double x, double y) B)
		{
			bool close = Math.Abs(A.x - B.x) < 0.5 && Math.Abs(A.y - B.y) < 0.5;
			return close;
		}

		private void Update()
		{
			long thisTime = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
			double dT = (double)(thisTime - lastTime) / frameDelay;
			lastTime = thisTime;

			double rx = NewPosition.x - Position.x;
			if (Math.Abs(rx) < 0.5) rx = 0;
			//double xFix = Math.Abs(rx) < 5 ? (1 / (5 - Math.Abs(rx))) : 1;
			//double dx = Math.Max( Math.Min(Math.Sign(rx) * dT , xFix), Math.Sign(rx) * xFix);

			double ry = NewPosition.y - Position.y;
			if (Math.Abs(ry) < 0.5) ry = 0;
			//double yFix = Math.Abs(ry) < 5 ? (1 / (5 - Math.Abs(ry))) : 1;
			//double dy = Math.Max(Math.Min(Math.Sign(ry) * dT, yFix), Math.Sign(ry) * yFix);

			double dx = Speed * dT * Math.Sign(rx) / (1000 / frameDelay);
			double dy = Speed * dT * Math.Sign(ry) / (1000 / frameDelay);
			if (Math.Abs(rx) < 5) dx /= 3;
			if (Math.Abs(ry) < 5) dy /= 3;

			Position = (Position.x + dx, Position.y + dy);
			direction = rx < 0 ? -1 : 1;
			StateHasChanged();

		}

	}
}
