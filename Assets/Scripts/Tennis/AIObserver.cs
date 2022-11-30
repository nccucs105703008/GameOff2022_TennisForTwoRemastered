using System;
using UnityEngine;

namespace Tennis
{
	public class AIObserver : MonoBehaviour
	{
		[SerializeField]
		private CourtBoundarySensor _lowSpace;
		[SerializeField]
		private CourtBoundarySensor _highSpace;
		[SerializeField]
		private Collider2D _lowTargetSpace;
		[SerializeField]
		private Collider2D _highTargetSpace;
		private TennisBallController _tennisBall;
		private bool _isInLowSpace;
		private bool _isInHighSpace;

		public event Action OnBallEnter;
		public event Action OnBallExit;

        private void Awake()
        {
			_lowSpace.OnBallEnterTrigger += OnBallEnterLowSpace;
			_lowSpace.OnBallExitTrigger += OnBallExitLowSpace;
			_highSpace.OnBallEnterTrigger += OnBallEnterHighSpace;
			_highSpace.OnBallExitTrigger += OnBallExitHighSpace;
		}
        private void OnDestroy()
        {
			_lowSpace.OnBallEnterTrigger -= OnBallEnterLowSpace;
			_lowSpace.OnBallExitTrigger -= OnBallExitLowSpace;
			_highSpace.OnBallEnterTrigger -= OnBallEnterHighSpace;
			_highSpace.OnBallExitTrigger -= OnBallExitHighSpace;
		}

        private void OnBallEnterLowSpace()
        {
			_isInLowSpace = true;
			if (!_isInHighSpace)
			{
				OnBallEnter?.Invoke();
			}
		}

        private void OnBallExitLowSpace()
        {
			_isInLowSpace = false;
			if (!_isInHighSpace)
			{
				OnBallExit?.Invoke();
			}
		}

        private void OnBallEnterHighSpace()
        {
			_isInHighSpace = true;
			if (!_isInLowSpace)
			{
				OnBallEnter?.Invoke();
			}
		}

        private void OnBallExitHighSpace()
        {
			_isInHighSpace = false;
			if (!_isInLowSpace)
			{
				OnBallExit?.Invoke();
			}
		}

        public void Initialize(TennisBallController tennisBall)
		{
			_tennisBall = tennisBall;
		}

		public Vector3 GetHitVector()
		{
			var ballPosition = _tennisBall.transform.position;
			//Debug.Log($"ballPosition: {ballPosition}");
			var targetPosition = GetRandomTarget();
			//Debug.Log($"targetPosition: {targetPosition}");
			var hitVector = (targetPosition - ballPosition).normalized;
			//Debug.Log($"hitVertor: {hitVector}");

			return hitVector;
		}
		private Vector3 GetRandomTarget()
		{
			var targetSpace = _isInLowSpace ? _lowTargetSpace : _highTargetSpace;

			if (targetSpace != null)
			{
				var bounds = targetSpace.bounds;
				return new Vector3(
	UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
	UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
	UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
			}

			return new Vector3(0, 0, 0);
		}

		public float GetBallSpeed()
		{
			var rigibody = _tennisBall.GetComponent<Rigidbody2D>();

			return rigibody.velocity.magnitude;
		}
    }
}