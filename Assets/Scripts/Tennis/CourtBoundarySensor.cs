using System;
using UnityEngine;

namespace Tennis
{
	public class CourtBoundarySensor : MonoBehaviour
	{
		public event Action OnBallEnterCollision;
		public event Action OnBallExitTrigger;

        private void OnCollisionEnter2D(Collision2D collision)
        {
			if (string.Compare(collision.gameObject.tag, "TennisBall") == 0)
			{
				OnBallEnterCollision?.Invoke();
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			if (string.Compare(collision.gameObject.tag, "TennisBall") == 0)
			{
				OnBallExitTrigger?.Invoke();
			}
		}
	}
}