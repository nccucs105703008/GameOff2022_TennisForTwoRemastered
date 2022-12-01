using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_manager : MonoBehaviour
{
    static ui_manager instance;
    private GameObject UILayer;

    //public List<baseUIView> popup_UI_window;
	public Dictionary<string, List<baseUIView>> UI_windows;

	public static ui_manager GetInstance()
    {
        if (instance == null)
        {
            GameObject temp = new GameObject("ui_manager");
            instance = temp.AddComponent<ui_manager>();

			instance.UI_windows = new Dictionary<string, List<baseUIView>>();
            DontDestroyOnLoad(instance);
            //DontDestroyOnLoad(instance.popLayer);
        }

        return instance;
    }

    public Transform Get_UILayer()
    {
		if (UILayer == null) {
			instance.UILayer = Instantiate(FileManager.LoadPrefab("UI/UILayer"));
			DontDestroyOnLoad(instance.UILayer);
		}
        //instance.popLayer.GetComponent<setMainCamera>().setCamera();
        return UILayer.transform;
    }

    //yes no type
    public void show_normal_window(string title, string content, Action yesCB, Action noCB, Action openCB = null, Action closeCB = null, bool onlyOpenCB = false, bool onlyCloseCB = false, baseUIView.popType popType = baseUIView.popType.normal, string btnLabel1 = "", string btnLabel2 = "")
    {
		if (!UI_windows.ContainsKey("normal_window")) {
			UI_windows.Add("normal_window", new List<baseUIView>());
		}
		List<baseUIView> popup_UI_window = UI_windows["normal_window"];

		for (int i = popup_UI_window.Count - 1; i >= 0; i--) if (popup_UI_window[i] == null) popup_UI_window.RemoveAt(i);

        if (popup_UI_window.Count == 0)
        {
            GameObject temp = Instantiate(FileManager.LoadPrefab("UI/UIPopUp"), Get_UILayer());
			baseUIView baseUIView = null;
			try {
				baseUIView = temp.GetComponent<baseUIView>();
			}
			catch(Exception e) {
				Debug.LogError("[UI] " + e.Message);
				return ;
			}
			popup_UI_window.Add(baseUIView);

            temp.SetActive(false);
			UIPOPUP popup = baseUIView as UIPOPUP;

			popup.setting_btnType(UIPOPUP.btnType.yes_no);
			popup.setting_popType(popType);
			popup.setting_btnLabel(btnLabel1, btnLabel2);
			popup.Setting_Title_content(title, content);
			popup.Setting_BtnCB(yesCB, noCB);
			popup.Setting_CB(openCB, closeCB);
			popup.Setting_onlyCB(onlyOpenCB, onlyCloseCB);

			popup.popup();
        }else if (popup_UI_window.Count > 0 && popup_UI_window[popup_UI_window.Count - 1].gameObject.activeSelf)
        {
			GameObject temp_popup_normal_window = Instantiate(FileManager.LoadPrefab("UI/UIPopUp"), Get_UILayer());
			baseUIView baseUIView = null;
			try {
				baseUIView = temp_popup_normal_window.GetComponent<baseUIView>();
			}
			catch (Exception e) {
				Debug.LogError("[UI] " + e.Message);
				return;
			}
			popup_UI_window.Add(baseUIView);
			temp_popup_normal_window.SetActive(false);
			UIPOPUP popup = baseUIView as UIPOPUP;
			popup.setting_btnType(UIPOPUP.btnType.yes_no);
			popup.setting_popType(popType);
			popup.setting_btnLabel(btnLabel1, btnLabel2);
			popup.Setting_Title_content(title, content);
			popup.Setting_BtnCB(yesCB, noCB);
			popup.Setting_onlyCB(onlyOpenCB, onlyCloseCB);
			popup.Setting_closeAndDestory(true);
			popup.popup();
        }
        else
        {
            int count = ui_manager.GetInstance().Get_UILayer().childCount;
			popup_UI_window[0].transform.SetSiblingIndex(count - 1);

			popup_UI_window[0].gameObject.SetActive(false);

			UIPOPUP baseUIView = popup_UI_window[0] as UIPOPUP;
            baseUIView.setting_btnType(UIPOPUP.btnType.yes_no);
			baseUIView.setting_popType(popType);
			baseUIView.setting_btnLabel(btnLabel1, btnLabel2);
            baseUIView.Setting_Title_content(title, content);
            baseUIView.Setting_BtnCB(yesCB, noCB);
            baseUIView.Setting_CB(openCB, closeCB);
            baseUIView.Setting_onlyCB(onlyOpenCB, onlyCloseCB);

            baseUIView.popup();
        }
    }

    //OK type
    public void show_normal_window(string title, string content, Action okCB, Action openCB = null, Action closeCB = null, bool onlyOpenCB = false, bool onlyCloseCB = false, baseUIView.popType popType = baseUIView.popType.normal, string btnLabel1 = "", string btnLabel2 = "")
    {
		if (!UI_windows.ContainsKey("normal_window")) {
			UI_windows.Add("normal_window", new List<baseUIView>());
		}
		List<baseUIView> popup_UI_window = UI_windows["normal_window"];

		for (int i = popup_UI_window.Count - 1; i >= 0; i--) if (popup_UI_window[i] == null) popup_UI_window.RemoveAt(i);

		if (popup_UI_window.Count == 0) {
			GameObject temp = Instantiate(FileManager.LoadPrefab("UI/UIPopUp"), Get_UILayer());
			baseUIView baseUIView_ = null;
			try {
				baseUIView_ = temp.GetComponent<baseUIView>();
			}
			catch (Exception e) {
				Debug.LogError("[UI] " + e.Message);
				return;
			}
			popup_UI_window.Add(baseUIView_);

			temp.SetActive(false);
			UIPOPUP baseUIView = baseUIView_ as UIPOPUP;

			baseUIView.setting_btnType(UIPOPUP.btnType.ok);
			baseUIView.setting_popType(popType);
			baseUIView.setting_btnLabel(btnLabel1, btnLabel2);
			baseUIView.Setting_Title_content(title, content);
			baseUIView.Setting_BtnCB(okCB, null);
			baseUIView.Setting_CB(openCB, closeCB);
			baseUIView.Setting_onlyCB(onlyOpenCB, onlyCloseCB);

			baseUIView.popup();
		}
		else if (popup_UI_window.Count > 0 && popup_UI_window[popup_UI_window.Count - 1].gameObject.activeSelf) {
			GameObject temp_popup_normal_window = Instantiate(FileManager.LoadPrefab("UI/UIPopUp"), Get_UILayer());
			baseUIView baseUIView_ = null;
			try {
				baseUIView_ = temp_popup_normal_window.GetComponent<baseUIView>();
			}
			catch (Exception e) {
				Debug.LogError("[UI] " + e.Message);
				return;
			}
			popup_UI_window.Add(baseUIView_);
			temp_popup_normal_window.SetActive(false);
			UIPOPUP baseUIView = baseUIView_ as UIPOPUP;
			baseUIView.setting_btnType(UIPOPUP.btnType.ok);
			baseUIView.setting_popType(popType);
			baseUIView.setting_btnLabel(btnLabel1, btnLabel2);
			baseUIView.Setting_Title_content(title, content);
			baseUIView.Setting_BtnCB(okCB, null);
			baseUIView.Setting_CB(openCB, closeCB);
			baseUIView.Setting_onlyCB(onlyOpenCB, onlyCloseCB);
			baseUIView.Setting_closeAndDestory(true);
			baseUIView.popup();
		}
		else {
			int count = ui_manager.GetInstance().Get_UILayer().childCount;
			popup_UI_window[0].transform.SetSiblingIndex(count - 1);

			popup_UI_window[0].gameObject.SetActive(false);

			UIPOPUP baseUIView = popup_UI_window[0] as UIPOPUP;
			baseUIView.setting_btnType(UIPOPUP.btnType.ok);
			baseUIView.setting_popType(popType);
			baseUIView.setting_btnLabel(btnLabel1, btnLabel2);
			baseUIView.Setting_Title_content(title, content);
			baseUIView.Setting_BtnCB(okCB, null);
			baseUIView.Setting_CB(openCB, closeCB);
			baseUIView.Setting_onlyCB(onlyOpenCB, onlyCloseCB);

			baseUIView.popup();
		}
    }
	public void show_setting_window(Action openCB = null, Action closeCB = null, bool onlyOpenCB = false, bool onlyCloseCB = false, string btnLabel1 = "", string btnLabel2 = "") {
		if (!UI_windows.ContainsKey("setting_window")) {
			UI_windows.Add("setting_window", new List<baseUIView>());
		}
		List<baseUIView> baseUIs = UI_windows["setting_window"];

		if (baseUIs.Count == 0) {
			baseUIs.Add(Instantiate(FileManager.LoadPrefab("setting_window"), Get_UILayer()).GetComponent<baseUIView>());
		}
		else {
			int count = ui_manager.GetInstance().Get_UILayer().childCount;
			baseUIs[0].transform.SetSiblingIndex(count - 1);
		}
		settingWindow setting_window = baseUIs[0] as settingWindow;


		setting_window.gameObject.SetActive(false);

		setting_window.setting_btnLabel(btnLabel1, btnLabel2);
		setting_window.Setting_CB(openCB, closeCB);
		setting_window.Setting_onlyCB(onlyOpenCB, onlyCloseCB);

		setting_window.init();
	}
	public void show_achievement_window(Action openCB = null, Action closeCB = null, bool onlyOpenCB = false, bool onlyCloseCB = false, string btnLabel1 = "", string btnLabel2 = "") {
		if (!UI_windows.ContainsKey("achievementWindow")) {
			UI_windows.Add("achievementWindow", new List<baseUIView>());
		}
		List<baseUIView> baseUIs = UI_windows["achievementWindow"];

		if (baseUIs.Count == 0) {
			baseUIs.Add(Instantiate(FileManager.LoadPrefab("achievementWindow"), Get_UILayer()).GetComponent<baseUIView>());
		}
		else {
			int count = ui_manager.GetInstance().Get_UILayer().childCount;
			baseUIs[0].transform.SetSiblingIndex(count - 1);
		}
		achievementWindow achievement_window = baseUIs[0] as achievementWindow;


		achievement_window.gameObject.SetActive(false);

		achievement_window.setting_btnLabel(btnLabel1, btnLabel2);
		achievement_window.Setting_CB(openCB, closeCB);
		achievement_window.Setting_onlyCB(onlyOpenCB, onlyCloseCB);

		achievement_window.init();
	}

	public void show_gameModeSelector_window(Action openCB = null, Action closeCB = null, bool onlyOpenCB = false, bool onlyCloseCB = false, string btnLabel1 = "", string btnLabel2 = "") {
		if (!UI_windows.ContainsKey("gameModeSelector")) {
			UI_windows.Add("gameModeSelector", new List<baseUIView>());
		}
		List<baseUIView> baseUIs = UI_windows["gameModeSelector"];

		if (baseUIs.Count == 0) {
			baseUIs.Add(Instantiate(FileManager.LoadPrefab("gameModeSelector"), Get_UILayer()).GetComponent<baseUIView>());
		}
		else {
			int count = ui_manager.GetInstance().Get_UILayer().childCount;
			baseUIs[0].transform.SetSiblingIndex(count - 1);
		}
		gameModeSelector gameModeSelector = baseUIs[0] as gameModeSelector;


		gameModeSelector.gameObject.SetActive(false);

		gameModeSelector.setting_btnLabel(btnLabel1, btnLabel2);
		gameModeSelector.Setting_CB(openCB, closeCB);
		gameModeSelector.Setting_onlyCB(onlyOpenCB, onlyCloseCB);

		gameModeSelector.init();
	}

	public void show_pause_window(Action openCB = null, Action closeCB = null, bool onlyOpenCB = false, bool onlyCloseCB = false) {
		if (!UI_windows.ContainsKey("pauseWindow")) {
			UI_windows.Add("pauseWindow", new List<baseUIView>());
		}
		List<baseUIView> baseUIs = UI_windows["pauseWindow"];

		if (baseUIs.Count == 0) {
			baseUIs.Add(Instantiate(FileManager.LoadPrefab("pauseWindow"), Get_UILayer()).GetComponent<baseUIView>());
		}
		else {
			int count = ui_manager.GetInstance().Get_UILayer().childCount;
			baseUIs[0].transform.SetSiblingIndex(count - 1);
		}
		pauseWindow pauseWindow = baseUIs[0] as pauseWindow;


		pauseWindow.gameObject.SetActive(false);

		pauseWindow.Setting_CB(openCB, closeCB);
		pauseWindow.Setting_onlyCB(onlyOpenCB, onlyCloseCB);

		pauseWindow.init();
	}

	public void show_gameResult_window(Action openCB = null, Action closeCB = null, bool onlyOpenCB = false, bool onlyCloseCB = false, Action restartFunc = null){
		if (!UI_windows.ContainsKey("GameResultWindow")) {
			UI_windows.Add("GameResultWindow", new List<baseUIView>());
		}
		List<baseUIView> baseUIs = UI_windows["GameResultWindow"];

		if (baseUIs.Count == 0) {
			baseUIs.Add(Instantiate(FileManager.LoadPrefab("GameResultWindow"), Get_UILayer()).GetComponent<baseUIView>());
		}
		else {
			int count = ui_manager.GetInstance().Get_UILayer().childCount;
			baseUIs[0].transform.SetSiblingIndex(count - 1);
		}
		gameResult gameResult = baseUIs[0] as gameResult;


		gameResult.gameObject.SetActive(false);

		
		gameResult.Setting_CB(openCB, closeCB);
		gameResult.Setting_onlyCB(onlyOpenCB, onlyCloseCB);

		gameResult.init(restartFunc);
	}
	public void closeAllUI()
    {

		foreach (KeyValuePair<string, List<baseUIView>> valuePair in UI_windows) {
			for (int i = valuePair.Value.Count - 1; i >= 0; i--) {
				if (valuePair.Value[i] != null) {
					valuePair.Value[i].gameObject.SetActive(false);
				}
			}
		}

		for (int i = Get_UILayer().childCount - 1; i >= 0; i--) {
			if (Get_UILayer().GetChild(i) != null) {
				Get_UILayer().GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}
