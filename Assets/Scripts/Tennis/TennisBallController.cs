using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tennis
{
	public class TennisBallController : MonoBehaviour
	{
		public float ReturnSpeed;
		[SerializeField]
		private Collider2D _ballCollider;
		[SerializeField]
		private Rigidbody2D _rigibody;

		private bool _isMoving;
		private Vector3 _targetPosition;

		public event Action OnMoveDone;

		private void Start()
		{
			_rigibody = gameObject.GetComponent<Rigidbody2D>();
		}
        private void Update()
        {
			if (_isMoving)
			{
				float step = ReturnSpeed * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
				//_targetPosition = location;
				if (Vector3.Distance(transform.position, _targetPosition) < 0.001f)
				{
					//_ballCollider.enabled = true;
					_rigibody.simulated = true;
					_isMoving = false;
					OnMoveDone?.Invoke();
				}
			}
		}
        public void HitBall(float direction, float force)
		{
			var x = Mathf.Sin(direction / 180 * Mathf.PI) * force * -1;
			var y = Mathf.Cos(direction / 180 * Mathf.PI) * force;
			var addForce = new Vector2(x,y);
			_rigibody.AddForce(addForce, ForceMode2D.Impulse);
			Debug.Log($"HitBall: {direction}, direction: {force}, addForce: {addForce}");
		}
		public void HitBall(Vector2 force)
		{
			var addForce = force;
			_rigibody.AddForce(addForce, ForceMode2D.Impulse);
			Debug.Log($"HitBall");
		}
		public void MoveTo(Vector3 location, bool closeSimulated = true)
		{
			//不需要Z軸的資料
			location.z = transform.position.z;
			//_ballCollider.enabled = false;
			_rigibody.simulated = !closeSimulated;
			_isMoving = true;
			_targetPosition = location;
			_rigibody.velocity = new Vector2(0, 0);
			_rigibody.angularVelocity = 0f;
			Debug.Log($"MoveTo: {location}");
		}
		public void Pause()
		{
			Debug.Log($"Pause");
		}
		public void Continue()
		{
			Debug.Log($"Continue");
		}
	}
}