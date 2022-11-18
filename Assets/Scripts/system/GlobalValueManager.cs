using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LitJson;
public class GlobalValueManager : MonoBehaviour
{
    private JsonData datas; //資料從 JsonDataBase 來
	private static GlobalValueManager _instance;

    public static GlobalValueManager GetInstance()
    {
        if (_instance == null)
        {
            GameObject temp = new GameObject("GlobalValueManager");
            _instance = temp.AddComponent<GlobalValueManager>();
            DontDestroyOnLoad(_instance);

			if (JsonDataBase.isInit == false) {
				Debug.LogWarning("JsonDataBase is not been Initialized");
			}
			else {
				_instance.update_data();
			}
		}

        return _instance;
    }
    public void update_data()
    {
        //StartCoroutine(update_data_(callBack));
        datas = JsonDataBase.Global_datas;
    }

    //若不是使用quickTake方式拿取TC，請記得使用動態方式拿取，或自己監聽事件
    public static T Get_value<T>(string key, T defaultValue)
    {
		return GetInstance()._Get_value<T>(key, defaultValue);
    }

	private T _Get_value<T>(string key, T defaultValue) {
		if (datas == null || datas.Count <= 0) return defaultValue;

		try {
			JsonData data = null;
			for (int i = 0; i < datas.Count; i++) {
				if (datas[i]["key"].ToString().Equals(key)) {
					data = datas[i];
					break;
				}
			}

			if (data != null /*&& (typeof(T) == typeof(int) || typeof(T) == typeof(string))*/) {
				Debug.Log(string.Format("[GlobalValue] Found : {0} = {1}", key, Convert.ChangeType(data["value"].ToString(), typeof(T))));
				return (T)Convert.ChangeType(data["value"].ToString(), typeof(T));
			}
			else {
				Debug.Log(string.Format("[GlobalValue] Not Found : {0}", key));
				return defaultValue;
			}


		}
		catch (Exception e) {
			Debug.Log(string.Format("[GlobalValue] ERROR : {0}", key));
			return defaultValue;
		}

	}
}
