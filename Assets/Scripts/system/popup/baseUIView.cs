using Cysharp.Threading.Tasks;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class baseUIView : MonoBehaviour
{
	protected Action openCallBack = null;
	protected bool onlyOpenCallBack = false;
	protected  Action closeCallBack = null;
	protected bool onlyCloseCallBack = false;

    protected bool closeAndDestory = false;

    public Text title;
	protected popType pop_Type = popType.normal;
	protected bool needLockClose = false;

	public AudioClip openSE;
	public AudioClip closeSE;
	public bool disableOpenSE =false ;
	public bool disableCloseSE = false ;

	[Header("Feedbacks")]
	public MMF_Player OpenFeedback;
	public MMF_Player CloseFeedback;

	public enum popType
	{
		normal,
		warning
	}

    public void Setting_CB(Action open, Action close)
    {
        openCallBack += open;
        closeCallBack += close;
    }

	public void Setting_Title(string title_text) {
		if (title != null) {
			title.text = title_text;
			title.text = title.text.Replace("\\n", "\n");
		}
	}


	public void setting_popType(popType type = popType.normal) {
		pop_Type = type;
	}



    public void Setting_onlyCB(bool onlyOpenCallBack, bool onlyCloseCallBack)
    {
        this.onlyOpenCallBack = onlyOpenCallBack;
        this.onlyCloseCallBack = onlyCloseCallBack;
    }

    public void Setting_closeAndDestory(bool closeAndDestory)
    {
        this.closeAndDestory = closeAndDestory;
    }

    public void popup()
    {	
		openCallBack?.Invoke();

		if (onlyOpenCallBack) {
			return;
		}
        gameObject.SetActive(true);

		if (!disableOpenSE) {
			if (openSE != null) {
				AudioManager.PlaySE(openSE);
			}
			else {
				switch (pop_Type) {
					case popType.normal:
						AudioManager.PlaySE("open");
						break;
					case popType.warning:
						AudioManager.PlaySE("warning");
						break;
				}
			}			
		}

		if (OpenFeedback != null) {
			OpenFeedback.PlayFeedbacks();
		}

	}
    
    public virtual async void Close()
    {
		if (!disableCloseSE) {
			if (closeSE != null) {
				AudioManager.PlaySE(closeSE);
			}
			else {
				AudioManager.PlaySE("cancel");
			}
		}

		if (CloseFeedback != null) {
			CloseFeedback.PlayFeedbacks();
			await UniTask.DelayFrame(20);
		}

		 closeCallBack?.Invoke();

		if (onlyCloseCallBack) {
			return;
		} else {
			gameObject.SetActive(false);
		}	

	}

	protected virtual void OnEnable()
    {
        addToCloseManager();
    }

    protected virtual void OnDisable()
    {
        openCallBack = null;
        closeCallBack = null;

        removeFromCloseManager();

        if (closeAndDestory)
        {
			foreach (KeyValuePair<string, List<baseUIView>> baseUIs in ui_manager.GetInstance().UI_windows) {
				if (baseUIs.Value.Contains(this)){
					baseUIs.Value.Remove(this);
				}
			}
            Destroy(gameObject);
        }
    }

    void addToCloseManager()
    {
		if (needLockClose) {
			closeManager.GetInstance().AddMission(this.DoNothing);
		}
		else {
			closeManager.GetInstance().AddMission(this.Close_fromCloseManager);
		}
    }

    /// <summary>
    /// 僅給予CloseManager使用，請不要亂call它
    /// needSE_fromCloseManager 可以設定使用右鍵關閉是否要音效
    /// </summary>
    public void Close_fromCloseManager()
    {
        Close();
    }

    void DoNothing(){}

    void removeFromCloseManager()
    {
		if (needLockClose) { closeManager.GetInstance().RemoveMission(this.DoNothing);
		} else {
			closeManager.GetInstance().RemoveMission(this.Close_fromCloseManager);
		}
    }


}
