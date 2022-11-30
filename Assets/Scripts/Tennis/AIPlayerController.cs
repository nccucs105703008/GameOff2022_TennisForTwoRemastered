using System;
using UnityEngine;

namespace Tennis
{
	public class AIPlayerController : MonoBehaviour, ITennisPlayerController
	{
		public event Action<Vector2> OnHitBall;

		public float MaxForce = 4000;
		public Vector2 HitForce { get; private set; } =  new Vector2(-900,900);

		private AIObserver _aiObserver;
		private bool _canHitBall;
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

		public void Initialize(AIObserver aiObserver, float intervalTime, float swingProbability)
		{
			Clear();
			_aiObserver = aiObserver;
			_aiObserver.OnBallEnter += OnBallEnterTrigger;
			_aiObserver.OnBallExit += OnBallExitTrigger;

			IntervalTime = intervalTime;
			SwingProbability = swingProbability;
		}
		private void Clear()
		{
			if (_aiObserver != null)
			{
				_aiObserver.OnBallEnter -= OnBallEnterTrigger;
				_aiObserver.OnBallExit -= OnBallExitTrigger;
			}
		}

		private void OnBallEnterTrigger()
		{
			_canHitBall = true;
		}

		private void OnBallExitTrigger()
		{
			_canHitBall = false;
		}

		private void Update()
		{
			if (_canHitBall)
			{
				_actionTime += Time.deltaTime;
				if (_actionTime >= IntervalTime)
				{
					_actionTime = 0;
					var hitVector = _aiObserver.GetHitVector();
					var ballSpeed = _aiObserver.GetBallSpeed();
					var force = 1000;
					if (ballSpeed > 100)
					{
						force = 3000;
					}
					else if (ballSpeed > 50)
					{
						force = 2500;
					}
					else if (ballSpeed > 20)
					{
						force = 1250;
					}

					SwingRacquet(hitVector * force);
				}
			}
		}

		public void SwingRacquet(Vector2 force)
		{
			if (!_canHitBall)
			{
				return;
			}
			var p = UnityEngine.Random.Range(0.0f, 1.0f);
			if (p > SwingProbability)
			{
				return;
			}

			if (force.magnitude > MaxForce)
			{
				force = force.normalized * MaxForce;
			}

			OnHitBall?.Invoke(force);
		}
	}
}