using System;
using UnityEngine;

namespace Tennis
{
	public class TennisGame : MonoBehaviour, IDisposable
	{
		[SerializeField]
		private TennisCourt _court;
		[SerializeField]
		private TennisPlayerController _player;
		[SerializeField]
		private AIPlayerController _aiPlayer;
		[SerializeField]
		private TennisBallController _ball;


		private ITennisPlayerController _player1Instance;
		private ITennisPlayerController _player2Instance;
		private TennisBallController _ballInstance;
		private TennisCourt _courtInstance;

		private ITennisPlayerController _lastAttacker;
		private ITennisPlayerController _servePlayer;

		private bool _canLeftAttack;
		private bool _canRightAttack;

		private void Awake()
		{
			var court = Instantiate<TennisCourt>(_court, gameObject.transform);
			var ball = Instantiate<TennisBallController>(_ball, court.LeftServePosition, new Quaternion(), gameObject.transform);
			var player2 = Instantiate<AIPlayerController>(_aiPlayer, gameObject.transform);
			var player1 = Instantiate<TennisPlayerController>(_player, gameObject.transform);

			InitializeGame(court, player1, player2, ball);
		}
		private void OnDestroy()
		{
			Dispose();
		}

		public void InitializeGame(TennisCourt court, ITennisPlayerController player1, ITennisPlayerController player2, TennisBallController tennisBallController)
		{
			Clear();
			_player1Instance = player1;
			_player2Instance = player2;
			_ballInstance = tennisBallController;
			_courtInstance = court;

			_lastAttacker = null;

			_player1Instance.OnHitBall += OnPlayer1HitBall;
			_player2Instance.OnHitBall += OnPlayer2HitBall;
			_courtInstance.OnFallLeftGround += OnFallLeftGround;
			_courtInstance.OnFallRightGround += OnFallRightGround;
			_courtInstance.OnTouchNet += OnTouchNet;
			_courtInstance.OnExitBoundary += OnExitBoundary;

			_courtInstance.OnEnterLeft += OnEnterLeft;
			_courtInstance.OnEnterRight += OnEnterRight;
		}

		private void Clear()
		{
			if (_player1Instance != null)
			{
				_player1Instance.OnHitBall -= OnPlayer1HitBall;
			}
			if (_player1Instance != null)
			{
				_player2Instance.OnHitBall -= OnPlayer2HitBall;
			}
			if (_courtInstance != null)
			{
				_courtInstance.OnFallLeftGround -= OnFallLeftGround;
				_courtInstance.OnFallRightGround -= OnFallRightGround;
				_courtInstance.OnTouchNet -= OnTouchNet;
				_courtInstance.OnExitBoundary -= OnExitBoundary;

				_courtInstance.OnEnterLeft -= OnEnterLeft;
				_courtInstance.OnEnterRight -= OnEnterRight;
			}
		}

        public void Dispose()
		{
			Clear();
		}

		private void OnFallLeftGround()
		{
			Debug.Log($"OnFallLeftGround");
			if (_lastAttacker != null)
			{
				JudgeTest();
			}
		}

		private void OnFallRightGround()
		{
			Debug.Log($"OnFallRightGround");

			if (_lastAttacker != null)
			{
				JudgeTest();
			}
		}

		private void OnTouchNet()
		{
			Debug.Log($"OnTouchNet");
		}

		private void OnExitBoundary()
		{
			Debug.Log($"OnExitBoundary");
			JudgeTest();
		}

		private void OnEnterLeft()
		{
			_canLeftAttack = true;
			_canRightAttack = false;
			if (_player2Instance is AIPlayerController aiPlayer)
			{
				aiPlayer.SetActive(false);
			}
			//允許左邊攻擊
			Debug.Log($"OnEnterLeft");
		}

		private void OnEnterRight()
		{
			_canLeftAttack = false;
			_canRightAttack = true;
			if (_player2Instance is AIPlayerController aiPlayer)
			{
				aiPlayer.SetActive(true);
			}
			//允許右邊攻擊
			Debug.Log($"OnEnterRight");
		}
		private void OnPlayer1HitBall(Vector2 force)
		{
			if (_canLeftAttack)
			{
				PlayerHitBall(_player1Instance, force);
			}
		}
		
		private void OnPlayer2HitBall(Vector2 force)
		{
			if (_canRightAttack)
			{
				PlayerHitBall(_player2Instance, force);
			}
		}
		private void PlayerHitBall(ITennisPlayerController player, Vector2 force)
		{
			//不可連續擊球
			if (_lastAttacker != player)
			{
				//切換攻擊方資料
				_lastAttacker = player;
				//擊球
				_ballInstance?.HitBall(force);
			}
		}
		private void JudgeTest()
		{
			Vector3 servePosition = new Vector3(0, 0, 0);
			if (_lastAttacker == _player1Instance)
			{
				servePosition = _courtInstance.RightServePosition;
			}
			else if (_lastAttacker == _player2Instance)
			{
				servePosition = _courtInstance.LeftServePosition;
			}
			if (_lastAttacker == null)
			{
				if (_servePlayer == _player1Instance)
				{
					servePosition = _courtInstance.RightServePosition;
				}
				else
				{
					servePosition = _courtInstance.RightServePosition;
				}
			}
			_canLeftAttack = false;
			_canRightAttack = false;
			_lastAttacker = null;
			_ballInstance.MoveTo(servePosition);
		}
	}
}