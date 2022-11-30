using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tennis
{
	public class TennisPlayerController : MonoBehaviour, ITennisPlayerController, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		private float MaxForce = 2500;
		/// <summary>
		/// 揮拍冷卻時間
		/// </summary>
		public float SwingCoolDown;
		private float _coolDownUpdateTime;
		/// <summary>
		/// 當前冷卻時間
		/// </summary>
		public float CoolDownTime => SwingCoolDown - _coolDownUpdateTime;

		private bool _canHitBall = true;

		private Vector2 _beginDrag;
		private float _dragTime;
		private bool _isDrag;

		public event Action<Vector2> OnHitBall;

        private void Update()
        {
			if (!_canHitBall)
			{
				_coolDownUpdateTime += Time.deltaTime;
				if (_coolDownUpdateTime > SwingCoolDown)
				{
					_coolDownUpdateTime = SwingCoolDown;
					_canHitBall = true;
				}
			}
			else if (_isDrag)
			{
				_dragTime += Time.deltaTime;
			}
        }

		public void SwingRacquet(Vector2 force)
		{
			if (!_canHitBall)
			{
				return;
			}
			if (force.magnitude > MaxForce)
			{
				force = force.normalized * MaxForce;
			}

			OnHitBall?.Invoke(force);
			SetSwingCoolDown();
		}

		private void SetSwingCoolDown()
		{
			_canHitBall = false;
			_coolDownUpdateTime = 0;
		}

		void IDragHandler.OnDrag(PointerEventData eventData)
		{
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (_canHitBall)
			{
				_beginDrag = eventData.position;
				_dragTime = 0;
				_isDrag = true;
			}
		}
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (_isDrag && _canHitBall)
			{
				var endDrag = eventData.position;
				var deltaPosition = endDrag - _beginDrag;
				if (_dragTime != 0)
				{
					deltaPosition = deltaPosition / _dragTime;
					_dragTime = 0;
					SwingRacquet(deltaPosition);
				}
				_isDrag = false;
			}
		}
	}
}