using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		loadingManager.GetInstance().initLoad();
		closeManager.GetInstance().initClose();
		loadingManager.GetInstance().StartLoading();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
#if !UNITY_EDITOR
		Debug.unityLogger.logEnabled = false;
#endif
		IEnumerator_init();
	}

	private void IEnumerator_init() {
		CoroutineHub.GetInstance().StartCoroutine(IEnumerator_init_());
	}

	IEnumerator IEnumerator_init_() {
		AudioManager.initSetting();
		languageManager.initLanguage();
		resolution_manager.initResolution();
		AchievementManager.initAchievement();

		bool isInit = true;
		string err = "";
		JsonDataBase.Init((err_) => { err = err_; isInit = false; });
		yield return new WaitWhile(() => isInit);

		if (err != "") {
			sceneChangeManager.GetInstance().changeScene("preInit");
			yield break;
		}

		//system_data.instance.init(); 目前主選單不太有需要紀錄的東西

		sceneChangeManager.GetInstance().changeScene("title");
		loadingManager.GetInstance().DoneLoading();
	}
}
