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

		private Player _servePlayer;
		private Player _attacker;
		private Player _defender
		{
			get
			{
				if (_attacker == Player.Left)
				{
					return Player.Right;
				}
				else if (_attacker == Player.Right)
				{
					return Player.Left;
				}
				return Player.None;
			}
		}


		private bool _canLeftAttack;
		private bool _canRightAttack;
		private bool _isFighting;

		public int GamePoint { get; private set; }
		public int LeftPoint { get; private set; }
		public int RightPoint { get; private set; }

		public event Action OnGameSet;

		private void Awake()
		{
			GamePoint = GlobalValueManager.Get_value<int>("GamePoint", 10);
			Restart();
		}
		private void OnDestroy()
		{
			Dispose();
		}
		public void Restart()
		{
			var court = _courtInstance != null ? _courtInstance : Instantiate<TennisCourt>(_court, gameObject.transform);
			var ball = _ballInstance != null ? _ballInstance : Instantiate<TennisBallController>(_ball, court.LeftServePosition, new Quaternion(), gameObject.transform);
			var player1 = _player1Instance != null ? _player1Instance : Instantiate<TennisPlayerController>(_player, gameObject.transform);
			var player2 = _player2Instance != null ? _player2Instance : Instantiate<AIPlayerController>(_aiPlayer, gameObject.transform);
			if (player1 is TennisPlayerController player)
			{
				var swingCoolDown = GlobalValueManager.Get_value<float>("PlayerSwingCoolDown", 1f);
				player.Initialize(swingCoolDown);
			}
			if (player2 is AIPlayerController aIPlayer)
			{
				var aiObserver = court.GetComponentInChildren<AIObserver>();
				aiObserver.Initialize(ball);
				aIPlayer.Initialize(aiObserver, 0.75f, 0.5f);
			}

			InitializeGame(court, player1, player2, ball);
		}
		public void InitializeGame(TennisCourt court, ITennisPlayerController player1, ITennisPlayerController player2, TennisBallController tennisBallController)
		{
			Clear();
			_player1Instance = player1;
			_player2Instance = player2;
			_ballInstance = tennisBallController;
			_courtInstance = court;

			_player1Instance.OnHitBall += OnPlayer1HitBall;
			_player2Instance.OnHitBall += OnPlayer2HitBall;
			_ballInstance.OnMoveDone += OnBallMoveDone;

			_courtInstance.OnFallLeftGround += OnFallLeftGround;
			_courtInstance.OnFallRightGround += OnFallRightGround;
			_courtInstance.OnTouchNet += OnTouchNet;
			_courtInstance.OnExitBoundary += OnExitBoundary;

			_courtInstance.OnEnterLeft += OnEnterLeft;
			_courtInstance.OnEnterRight += OnEnterRight;

			_ballInstance.MoveTo(court.LeftServePosition);
		}

		private void Clear()
		{
			if (_player1Instance != null)
			{
				_player1Instance.OnHitBall -= OnPlayer1HitBall;
			}
			if (_player2Instance != null)
			{
				_player2Instance.OnHitBall -= OnPlayer2HitBall;
			}
			if (_ballInstance != null)
			{
				_ballInstance.OnMoveDone -= OnBallMoveDone;
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

			_servePlayer = Player.Left;
			_attacker = Player.None;
			_canLeftAttack = false;
			_canRightAttack = false;
			_isFighting = false;

			LeftPoint = 0;
			RightPoint = 0;
		}

		public void Dispose()
		{
			Clear();
		}

		private void OnFallLeftGround()
		{
			AudioManager.PlaySE("TennisBall");
			if (_attacker != Player.Left)
			{
				Adjudicate(_attacker);
			}
			else
			{
				Adjudicate(_defender);
			}
		}

		private void OnFallRightGround()
		{
			AudioManager.PlaySE("TennisBall");
			if (_attacker != Player.Right)
			{
				Adjudicate(_attacker);
			}
			else
			{
				Adjudicate(_defender);
			}
		}

		private void OnTouchNet()
		{
		}

		private void OnExitBoundary()
		{
			Adjudicate(_defender);
		}

		private void OnEnterLeft()
		{
			_canLeftAttack = true;
			_canRightAttack = false;
			//允許左邊攻擊
		}

		private void OnEnterRight()
		{
			_canLeftAttack = false;
			_canRightAttack = true;
			//允許右邊攻擊
		}
		private void OnPlayer1HitBall(Vector2 force)
		{
			if (_canLeftAttack)
			{
				PlayerHitBall(Player.Left, force);
			}
		}
		
		private void OnPlayer2HitBall(Vector2 force)
		{
			if (_canRightAttack)
			{
				PlayerHitBall(Player.Right, force);
			}
		}
		private void OnBallMoveDone()
		{
			_isFighting = true;
		}
		private void PlayerHitBall(Player attacker, Vector2 force)
		{
			//不可連續擊球
			if (_attacker != attacker)
			{
				_attacker = attacker;
				_ballInstance?.HitBall(force);
				AudioManager.PlaySE("HitBall");
			}
		}
		/// <summary>
		/// 判定得分
		/// </summary>
		private void Adjudicate(Player scorer)
		{
			if (!_isFighting)
			{
				return;
			}
			Vector3 servePosition = _servePlayer == Player.Left ? _courtInstance.LeftServePosition : _courtInstance.RightServePosition;

			switch (scorer)
			{
				case Player.Left:
					LeftPoint++;
					servePosition = _courtInstance.LeftServePosition;
					_servePlayer = scorer;
					break;
				case Player.Right:
					RightPoint++;
					servePosition = _courtInstance.RightServePosition;
					_servePlayer = scorer;
					break;
			}

			Debug.Log($"CurrentPoint: {LeftPoint}:{RightPoint}");
			
			ResetAttacker();

			if (LeftPoint >= GamePoint)
			{
				OnGameSet?.Invoke();
			}
			else if (RightPoint >= GamePoint)
			{
				OnGameSet?.Invoke();
			}
			else
			{
				_isFighting = false;
				_ballInstance.MoveTo(servePosition);
			}
		}

		private void ResetAttacker()
		{
			if (_attacker == Player.None)
			{
				return;
			}
			_canLeftAttack = false;
			_canRightAttack = false;
			_attacker = Player.None;
		}
    }
}