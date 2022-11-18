using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Language_manager : MonoBehaviour
{
    private JsonData Language_datas; //資料從 JsonDataBase 來
    private static Language_manager _instance;

    public static Language_manager GetInstance()
    {
        if (_instance == null)
        {
            GameObject temp = new GameObject("Language_manager");
            _instance = temp.AddComponent<Language_manager>();
            DontDestroyOnLoad(_instance);

			if (JsonDataBase.isInit == false) {
				Debug.LogWarning("JsonDataBase is not been Initialized");
			}
			else {
				_instance.updateLanguage_data();
			}
		}

        return _instance;
    }

    public void updateLanguage_data()
    {
        Language_datas = JsonDataBase.Language_datas;
    }

	/// <summary>
	/// 若不是使用quickTake方式拿取Language，請記得使用動態方式拿取，或自己監聽事件
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static string GetLanguage_value(string key)
    {
		return GetInstance()._GetLanguage_value(key);   
    }

	private string _GetLanguage_value(string key) {

		if (Language_datas == null || Language_datas.Count <= 0) return "";

		try {
			JsonData data = new JsonData();
			for (int i = 0; i < Language_datas.Count; i++) {
				if (Language_datas[i]["key"].ToString().Equals(key)) {
					data = Language_datas[i];
					break;
				}
			}

			return data.Count > 0 ? data["value"][languageManager.Language_index].ToString() : "";
		}
		catch (Exception e) {
			return "";
		}

	}
}
