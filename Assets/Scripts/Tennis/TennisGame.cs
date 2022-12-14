using MoreMountains.Feedbacks;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

		[Header("Score")]
		public Text _leftScore;
		public Text _rightScore;

		[Header("Feedbacks")]
		public MMF_Player _leftScoreFeedback;
		public MMF_Player _rightScoreFeedback;

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
			OnGameSet += () => ui_manager.GetInstance().show_gameResult_window(restartFunc: Restart);
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
			_leftScore.text = LeftPoint.ToString();
			_rightScore.text = RightPoint.ToString();
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
			//???\????????
		}

		private void OnEnterRight()
		{
			_canLeftAttack = false;
			_canRightAttack = true;
			//???\?k??????
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
			//???i?s?????y
			if (_attacker != attacker)
			{
				_attacker = attacker;
				_ballInstance?.HitBall(force);
				AudioManager.PlaySE("HitBall");
			}
		}
		/// <summary>
		/// ?P?w?o??
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
					_leftScore.text = LeftPoint.ToString();
					_leftScoreFeedback.PlayFeedbacks();
					servePosition = _courtInstance.LeftServePosition;
					_servePlayer = scorer;
					AudioManager.PlaySE("Point");
					break;
				case Player.Right:
					RightPoint++;
					_rightScore.text = RightPoint.ToString();
					_rightScoreFeedback.PlayFeedbacks();
					servePosition = _courtInstance.RightServePosition;
					_servePlayer = scorer;
					AudioManager.PlaySE("Point");
					break;
			}
			
			ResetAttacker();

			if (LeftPoint >= GamePoint)
			{
				_isFighting = false;
				StartCoroutine(DelayGameSet(Player.Left));
			}
			else if (RightPoint >= GamePoint)
			{
				_isFighting = false;
				StartCoroutine(DelayGameSet(Player.Right));
			}
			else
			{
				_isFighting = false;
				_ballInstance.MoveTo(servePosition);
			}
		}
		private IEnumerator DelayGameSet(Player winner)
		{
			yield return new WaitForSeconds(0.4f);
			GameSet(winner);
		}
		private void GameSet(Player winner)
		{
			switch (winner)
			{
				case Player.Left:
					AudioManager.PlaySE("Win");
					break;
				case Player.Right:
					AudioManager.PlaySE("Lose");
					break;
			}
			OnGameSet?.Invoke();
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

        private void Update()
        {
#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.A))
			{
				Adjudicate(Player.Left);
			}
			else if (Input.GetKeyDown(KeyCode.S))
			{
				Adjudicate(Player.Right);
			}
			else if (Input.GetKeyDown(KeyCode.R))
			{
				Restart();
			}
#endif
		}
    }
}