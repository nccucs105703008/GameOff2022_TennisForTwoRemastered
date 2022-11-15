using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tennis
{
	public class TennisGame : MonoBehaviour, IDisposable
	{
		[SerializeField]
		private TennisPlayerController _player;

		private ITennisPlayerController _player1;
		private ITennisPlayerController _player2;
		[SerializeField]
		private TennisBallController _ball;

		private void Awake()
		{
			var player1 = Instantiate<TennisPlayerController>(_player, gameObject.transform);
			var player2 = Instantiate<TennisPlayerController>(_player, gameObject.transform);
			player2.gameObject.SetActive(false);//for debug
			 _ball = Instantiate<TennisBallController>(_ball, gameObject.transform);
			InitializeGame(player1, player2/*Al COntroller*/, _ball);
		}
		private void OnDestroy()
		{
			Dispose();
		}

		public void InitializeGame(ITennisPlayerController player1, ITennisPlayerController player2, TennisBallController tennisBallController)
		{
			_player1 = player1;
			_player2 = player2;

			_player1.OnHitBall += OnPlayer1HitBall;
			_player2.OnHitBall += OnPlayer2HitBall;
		}
		public void Dispose()
		{
			_player1.OnHitBall -= OnPlayer1HitBall;
			_player2.OnHitBall -= OnPlayer2HitBall;
		}

		private void OnPlayer1HitBall(float direction, float force)
		{
			PlayerHitBall(_player1, direction, force);
		}
		private void OnPlayer2HitBall(float direction, float force)
		{
			PlayerHitBall(_player2, direction, force);
		}
		private void PlayerHitBall(ITennisPlayerController player, float direction, float force)
		{
			//切換攻擊方資料
			
			//擊球
			_ball?.HitBall(direction, force);
		}
	}
}