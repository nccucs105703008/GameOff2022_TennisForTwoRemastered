using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class achievementWindow : UIPOPUP 
{

    public void init()
    {
        loadingManager.GetInstance().StartLoading();
        CoroutineHub.GetInstance().StartCoroutine(init_());
    }

    IEnumerator init_()
    {
        yield return 0;

        setting_btnLabel(label2:Language_manager.GetLanguage_value("button_close"));
        Setting_CB(null, null);
        popup();
        loadingManager.GetInstance().DoneLoading();
    }
}
