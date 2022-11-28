using System;
using UnityEngine;

namespace Tennis
{
	public interface ITennisPlayerController
	{
		event Action<Vector2> OnHitBall;
		void SwingRacquet(Vector2 force);
	}
}