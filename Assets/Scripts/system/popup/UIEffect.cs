using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEffect : MonoBehaviour
{
	[Header("音效")]
	public AudioClip openSE;
	public AudioClip closeSE;
	public AudioClip warnSE;
	public AudioClip okSE; 
	public AudioClip disableSE;

	 [Header("旋轉效果")]
	public bool rotateSelf = false;
	public Vector3 speed = Vector3.zero;

	private void Update() {
		if (rotateSelf) {
			transform.Rotate(speed);
		}
	}

	public void PlayOpenSE() {
		if (openSE != null) {
			AudioManager.PlaySE(openSE);
		}
		else {
			AudioManager.PlaySE("open");
		}
	}
	public void PlayCloseSE() {
		if (openSE != null) {
			AudioManager.PlaySE(closeSE);
		}
		else {
			AudioManager.PlaySE("cancel");
		}
	}
	public void PlaWarnSE() {
		if (openSE != null) {
			AudioManager.PlaySE(warnSE);
		}
		else {
			AudioManager.PlaySE("warning");
		}
	}
	public void PlayOKSE() {
		if (openSE != null) {
			AudioManager.PlaySE(okSE);
		}
		else {
			AudioManager.PlaySE("ok");
		}
	}

	public void PlayDisableSE() {
		if (openSE != null) {
			AudioManager.PlaySE(disableSE);
		}
		else {
			AudioManager.PlaySE("disable");
		}
	}

}
