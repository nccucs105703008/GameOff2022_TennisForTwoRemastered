using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tennis
{
	public class TennisBallController : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody2D _rigibody;

		private void Start()
		{
			_rigibody = gameObject.GetComponent<Rigidbody2D>();
		}
		public void HitBall(float direction, float force)
		{
			//y = cos
			//x = sin * -1
			var x = Mathf.Sin(direction / 180 * Mathf.PI) * force * -1;
			var y = Mathf.Cos(direction / 180 * Mathf.PI) * force;
			var addForce = new Vector2(x,y);
			_rigibody.AddForce(addForce, ForceMode2D.Impulse);
			Debug.Log($"HitBall: {direction}, direction: {force}, addForce: {addForce}");
		}
		public void MoveTo(Vector3 location)
		{
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