using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.Threading.Tasks;

public class JsonDataBase : MonoBehaviour
{
	public static bool isInit = false;
	public static JsonData Language_datas;
	public static JsonData Global_datas;

	public static void Init(Action<bool> callBack = null) {
		Task.Factory.StartNew(
			async delegate { await _Init(); },
			TaskCreationOptions.LongRunning
		).Wait();

		/*Task.Run(async delegate
		{
			// 執行非同步函式
			await _Init();
		}).Wait();*/
		callBack?.Invoke(true);
		Debug.Log("Done");
	}

	static async Task<bool> _Init() {
		await Task.Yield();
		Debug.Log("update_data_Language");
		bool r = await update_data_Language();
		//Language
		if (!r)
        {
			return false;
        }
		Debug.Log("update_data_Global");
		r = await update_data_Global();
		//Global
		if (!r)
		{
			return false;
		}
		Debug.Log("isInit");
		isInit = true;
		return true;
	}

	public static string GetPathByPlatform(string path) {
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX
		path = "file://" + path;
#endif
		return path;
	}


	//Language
	private static async Task<bool> update_data_Language() {
		UnityEngine.Networking.UnityWebRequest uwr = UnityEngine.Networking.UnityWebRequest.Get(GetPathByPlatform(Application.streamingAssetsPath + "/data/clientData/LanguageData.json"));

		await uwr.SendWebRequest();

		if (uwr.isNetworkError || uwr.isHttpError) {
			Debug.Log(uwr.error);
			Debug.LogWarning("Language data load fail!");
			return false;
		}

		string data_raw = uwr.downloadHandler.text;
		if (data_raw != null) {
			//data_raw = DeEncode.Decrypt(data_raw);
			Language_datas = JsonMapper.ToObject(data_raw);
		}
		else {
			Debug.LogWarning("Language data load fail!");
			return false;
		}

		return true;
	}

	//Global
	private static async Task<bool> update_data_Global() {
		UnityEngine.Networking.UnityWebRequest uwr = UnityEngine.Networking.UnityWebRequest.Get(GetPathByPlatform(Application.streamingAssetsPath + "/data/clientData/globalValue.json"));

		await uwr.SendWebRequest();

		if (uwr.isNetworkError || uwr.isHttpError) {
			Debug.Log(uwr.error);
			Debug.LogWarning("Global data load fail!");
			return false;
		}

		string data_raw = uwr.downloadHandler.text;
		if (data_raw != null) {
			//data_raw = DeEncode.Decrypt(data_raw);
			Global_datas = JsonMapper.ToObject(data_raw);			
		}
		else {
			Debug.LogWarning("Global data load fail!");
			return false;
		}

		return true;
	}

}