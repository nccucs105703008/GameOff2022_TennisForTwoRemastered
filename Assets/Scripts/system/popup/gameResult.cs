using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameResult : UIPOPUP 
{

    public void init()
    {
        loadingManager.GetInstance().StartLoading();
        CoroutineHub.GetInstance().StartCoroutine(init_());
    }

    IEnumerator init_()
    {
		needLockClose = true;
		yield return 0;
		setting_btnLabel(Language_manager.GetLanguage_value("button_ok"));
        Setting_CB(null, null);
        popup();
        loadingManager.GetInstance().DoneLoading();
    }
}
