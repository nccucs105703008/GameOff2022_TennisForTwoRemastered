using System;
using UnityEngine;

namespace Tennis
{
	public class TennisCourt : MonoBehaviour
	{
		/// <summary>
		/// 碰到左邊地板
		/// </summary>
		public event Action OnFallLeftGround;
		/// <summary>
		/// 碰到右邊地板
		/// </summary>
		public event Action OnFallRightGround;
		/// <summary>
		/// 碰到網子
		/// </summary>
		public event Action OnTouchNet;
		/// <summary>
		/// 出界
		/// </summary>
		public event Action OnExitBoundary;

		[SerializeField]
		private CourtBoundarySensor _leftGround;
		[SerializeField]
		private CourtBoundarySensor _rightGround;
		[SerializeField]
		private CourtBoundarySensor _net;
		[SerializeField]
		private CourtBoundarySensor _boundary;
		[SerializeField]
		private Transform _leftServePoint;
		[SerializeField]
		private Transform _rightServePoint;
		public Vector3 LeftServePosition => _leftServePoint.position;
		public Vector3 RightServePosition => _rightServePoint.position;

		private void Awake()
		{
			_leftGround.OnBallEnterCollision += _onFallLeftGround;
			_rightGround.OnBallEnterCollision += _onFallRightGround;
			_net.OnBallEnterCollision += _onTouchNet;
			_boundary.OnBallExitTrigger += _onExitBoundary;
		}
		private void OnDestroy()
		{
			_leftGround.OnBallEnterCollision -= _onFallLeftGround;
			_rightGround.OnBallEnterCollision -= _onFallRightGround;
			_net.OnBallEnterCollision -= _onTouchNet;
			_boundary.OnBallExitTrigger -= _onExitBoundary;
		}
		private void _onFallLeftGround()
		{
			OnFallLeftGround?.Invoke();
		}
		private void _onFallRightGround()
		{
			OnFallRightGround?.Invoke();
		}
		private void _onTouchNet()
		{
			OnTouchNet?.Invoke();
		}
		private void _onExitBoundary()
		{
			OnExitBoundary?.Invoke();
		}
	}
}