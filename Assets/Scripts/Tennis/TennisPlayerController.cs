using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Tennis
{
	public class TennisPlayerController : MonoBehaviour, ITennisPlayerController
	{
		//要有一個Tracker，檢測球是否可擊球
		[SerializeField]
		private Button SwingBtn;
		[SerializeField]
		private UI_Knob DirectionKnob;

		public float HitDirection { get; private set; }
		public float HitForce { get; private set; } = 1000;
		
		public event Action<float, float> OnHitBall;

		//可能不用
		private TennisObserver _tenisObserver;

		private void Awake()
		{
			SwingBtn.onClick.AddListener(OnClickSwing);
			DirectionKnob.OnValueChanged.AddListener(OnChangeDirectionKnob);
		}
		private void OnDestroy()
		{
			SwingBtn.onClick.RemoveListener(OnClickSwing);
			DirectionKnob.OnValueChanged.RemoveListener(OnChangeDirectionKnob);
		}
		private void OnClickSwing()
		{
			//檢查現在能否擊球
			if (true)
			{
				SwingRacquet(HitDirection, HitForce);
			}
		}
		private void OnChangeDirectionKnob(float direction)
		{
			HitDirection = DirectionKnob.transform.rotation.eulerAngles.z;
		}

		public void SwingRacquet(float direction, float force)
		{
			OnHitBall?.Invoke(direction, force);
		}
    }
}