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

	public void OpenSetting() {
		//開設定畫面
		ui_manager.GetInstance().show_setting_window(btnLabel1:Language_manager.GetLanguage_value("menu_Setting"), btnLabel2: Language_manager.GetLanguage_value("menu_Exit"));
	}
	
	public void OpenAchievement() {
		//開成就視窗
		ui_manager.GetInstance().show_achievement_window();
	}

	public void OpenMainGame() {
		ui_manager.GetInstance().show_gameModeSelector_window();
		//ui_manager.GetInstance().show_pause_window();
		//ui_manager.GetInstance().show_gameResult_window();
	}
}
