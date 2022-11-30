using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseWindow : UIPOPUP 
{

    public void init()
    {
        loadingManager.GetInstance().StartLoading();
        CoroutineHub.GetInstance().StartCoroutine(init_());
    }

    IEnumerator init_()
    {

		yield return 0;
        Setting_CB(null, null);
        popup();
        loadingManager.GetInstance().DoneLoading();
    }

	public void Resume() {
		Close();
	}
	public void OpenSetting() {
		ui_manager.GetInstance().show_setting_window(btnLabel1: Language_manager.GetLanguage_value("menu_Setting"), btnLabel2: Language_manager.GetLanguage_value("menu_Exit"));
	}

	protected override void OnEnable() {
		Time.timeScale = 0;
		base.OnEnable();
	}

	protected override void OnDisable() {
		Time.timeScale = 1;
		base.OnDisable();
	}

	public override void Close() {
		base.Close();
	}
}
