using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameResult : UIPOPUP 
{
	System.Action restartFunc;

	public void init(System.Action restartFunc)
    {
		this.restartFunc = restartFunc;
        loadingManager.GetInstance().StartLoading();
        CoroutineHub.GetInstance().StartCoroutine(init_());
    }

    IEnumerator init_()
    {
		needLockClose = true;
		yield return 0;
		setting_btnLabel(Language_manager.GetLanguage_value("button_Restart"), Language_manager.GetLanguage_value("button_backMenu"));
        Setting_CB(null, null);
        popup();
        loadingManager.GetInstance().DoneLoading();
    }

    public void BackToMenu() {
        Close();
        sceneChangeManager.GetInstance().changeScene("Title");
    }

	public void Restart() {
		Close();
		restartFunc?.Invoke();
	}
}
