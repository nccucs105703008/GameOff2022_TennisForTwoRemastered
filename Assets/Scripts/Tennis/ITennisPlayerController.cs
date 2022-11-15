using System;

namespace Tennis
{
	public interface ITennisPlayerController
	{
		event Action<float, float> OnHitBall;
		void SwingRacquet(float direction, float force);
	}
}