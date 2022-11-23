using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
	private static Title _instance;

	public static Title GetInstance() {
		return _instance;
	}

	private void Awake() {
		_instance = this;
		init();
	}

	void init() {
		AudioManager.PlayBGM("title");
		closeManager.GetInstance().closeAllWindows();
		loadingManager.GetInstance().closeblockScreen();

		closeManager.GetInstance().AddMission(this.Close);
	}

	public void Close() //若在menu按下右鍵，詢問是否離開遊戲
	{
		ui_manager.GetInstance().show_normal_window(Language_manager.GetLanguage_value("popup_exitTitle"),
			Language_manager.GetLanguage_value("popup_exitContent"),
			yesCB:() => {
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
			},
			()=> { },
			popType: baseUIView.popType.warning
		);


	}
}
