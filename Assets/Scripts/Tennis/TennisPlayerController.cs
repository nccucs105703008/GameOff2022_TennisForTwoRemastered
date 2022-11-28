using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Tennis
{
	public class TennisPlayerController : MonoBehaviour, ITennisPlayerController, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		private float MaxForce = 2500;
		
		public event Action<Vector2> OnHitBall;

		private Vector2 _beginDrag;
		private float _deltaTime;
		private bool _isDrag;
		private bool _canHitBall = true;

		//可能不用
		private TennisObserver _tenisObserver;

        private void Update()
        {
			if (_isDrag)
			{
				_deltaTime += Time.deltaTime;
			}
        }

		public void SwingRacquet(Vector2 force)
		{
			if (force.magnitude > MaxForce)
			{
				force = force.normalized * MaxForce;
			}

			OnHitBall?.Invoke(force);
		}

		public void OnDrag(PointerEventData eventData)
		{
		}

		void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
		{
			if (_canHitBall)
			{
				_beginDrag = eventData.position;
				_deltaTime = 0;
				_isDrag = true;
			}
		}
		void IEndDragHandler.OnEndDrag(PointerEventData eventData)
		{
			if (_isDrag)
			{
				var endDrag = eventData.position;
				var deltaPosition = endDrag - _beginDrag;
				if (_deltaTime != 0)
				{
					deltaPosition = deltaPosition / _deltaTime;
				}
				_deltaTime = 0;
				_isDrag = false;


				Debug.Log($"{deltaPosition} delta dragged");

				//如果可以擊球
				if (true)
				{
					SwingRacquet(deltaPosition);
				}

				_deltaTime = 0;
			}
			
		}
	}
}