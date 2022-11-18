using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_manager : MonoBehaviour
{
    static ui_manager instance;
    private GameObject UILayer;

    public List<baseUIView> popup_UI_window;

	public static ui_manager GetInstance()
    {
        if (instance == null)
        {
            GameObject temp = new GameObject("ui_manager");
            instance = temp.AddComponent<ui_manager>();
            
            instance.popup_UI_window = new List<baseUIView>();
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

	public void closeAllUI()
    {
		for (int i = popup_UI_window.Count - 1; i >= 0; i--) {
			if (popup_UI_window[i] != null) {
				popup_UI_window.RemoveAt(i);
			}
		}

		for (int i = Get_UILayer().childCount - 1; i >= 0; i--) {
			if (Get_UILayer().GetChild(i) != null) {
				Get_UILayer().GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}
