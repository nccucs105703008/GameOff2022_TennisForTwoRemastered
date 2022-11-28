using System;
using UnityEngine;

namespace Tennis
{
	public class AIPlayerController : MonoBehaviour, ITennisPlayerController
	{
		public event Action<Vector2> OnHitBall;
		public Vector2 HitForce { get; private set; } =  new Vector2(-900,900);

		private bool _canHitBall = true;
		public float IntervalTime = 0.75f;
		private float _swingProbability = 0.5f;
		public float SwingProbability 
		{ 
			get 
			{ 
				return _swingProbability;
			}  
			set
			{ if (value > 1)
				{
					_swingProbability = 1;
				}
				else if (value < 0)
				{
					_swingProbability = 0;
				}
				else
				{
					_swingProbability = value;
				}
			}
		}
		private float _actionTime;

		public void Initialize(float intervalTime, float swingProbability)
		{
			IntervalTime = intervalTime;
			SwingProbability = swingProbability;
		}
		public void SetActive(bool isAIActive)
		{
			_canHitBall = isAIActive;
		}

		private void Update()
		{
			if (_canHitBall)
			{
				_actionTime += Time.deltaTime;
				if (_actionTime >= IntervalTime)
				{
					_actionTime = 0;
					SwingRacquet(HitForce);
				}
			}
		}

		public void SwingRacquet(Vector2 force)
		{
			var p = UnityEngine.Random.Range(0.0f, 1.0f);
			if (p < SwingProbability)
			{
				OnHitBall?.Invoke(force);
				Debug.Log("AI HitBall");
			}
			else
			{
				Debug.Log("AI HitBall Fail");
			}

		}

	}
}