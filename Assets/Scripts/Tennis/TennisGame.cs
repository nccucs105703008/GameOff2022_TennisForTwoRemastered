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
		private TennisBallController _ball;


		private ITennisPlayerController _player1Instance;
		private ITennisPlayerController _player2Instance;
		private TennisBallController _ballInstance;
		private TennisCourt _courtInstance;

		private ITennisPlayerController _lastAttacker;

		private void Awake()
		{
			var court = Instantiate<TennisCourt>(_court, gameObject.transform);
			var player1 = Instantiate<TennisPlayerController>(_player, gameObject.transform);
			var player2 = Instantiate<TennisPlayerController>(_player, gameObject.transform);
			player2.gameObject.SetActive(false);//for debug
			var ball = Instantiate<TennisBallController>(_ball, court.LeftServePosition, new Quaternion(), gameObject.transform);
			InitializeGame(court, player1, player2/*Al COntroller*/, ball);
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

			_lastAttacker = player1;

			_player1Instance.OnHitBall += OnPlayer1HitBall;
			_player2Instance.OnHitBall += OnPlayer2HitBall;
			_courtInstance.OnFallLeftGround += OnFallLeftGround;
			_courtInstance.OnFallRightGround += OnFallRightGround;
			_courtInstance.OnTouchNet += OnTouchNet;
			_courtInstance.OnExitBoundary += OnExitBoundary;
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
			}
		}

		public void Dispose()
		{
			Clear();
		}

		private void OnFallLeftGround()
		{
			Debug.Log($"OnFallLeftGround");
			//JudgeTest();
		}

		private void OnFallRightGround()
		{
			Debug.Log($"OnFallRightGround");
			//JudgeTest();
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
		private void OnPlayer1HitBall(float direction, float force)
		{
			PlayerHitBall(_player1Instance, direction, force);
		}
		private void OnPlayer2HitBall(float direction, float force)
		{
			PlayerHitBall(_player2Instance, direction, force);
		}
		private void PlayerHitBall(ITennisPlayerController player, float direction, float force)
		{
			//切換攻擊方資料
			_lastAttacker = player;
			//擊球
			_ballInstance?.HitBall(direction, force);
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
			_ballInstance.MoveTo(servePosition);
		}
	}
}