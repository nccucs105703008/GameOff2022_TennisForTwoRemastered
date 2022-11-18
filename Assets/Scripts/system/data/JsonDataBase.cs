using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class JsonDataBase : MonoBehaviour
{
	public static bool isInit = false;
	public static JsonData Language_datas;
	public static JsonData Global_datas;

	public static void Init(Action<string> callBack = null) {
		CoroutineHub.GetInstance().StartCoroutine(_Init(callBack));
	}

	static IEnumerator _Init(Action<string> callBack) {
		string err = "";
		bool isLoading = false;
		yield return 0;

		//Language
		isLoading = true;
		CoroutineHub.GetInstance().StartCoroutine(update_data_Language((err_) => { err = err_; isLoading = false; }));
		yield return new WaitWhile(() => isInit);
		if (err != "") {
			callBack?.Invoke("error");
			yield break;
		}

		//Global
		isLoading = true;
		CoroutineHub.GetInstance().StartCoroutine(update_data_Global((err_) => { err = err_; isLoading = false; }));
		yield return new WaitWhile(() => isInit);
		if (err != "") {
			callBack?.Invoke("error");
			yield break;
		}

		isInit = true;

		yield return 0;
		callBack?.Invoke("");
	}

	public static string GetPathByPlatform(string path) {
#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX
		path = "file://" + path;
#endif
		return path;
	}


	//Language
	private static IEnumerator update_data_Language(Action<string> callBack) {
		UnityEngine.Networking.UnityWebRequest uwr = UnityEngine.Networking.UnityWebRequest.Get(GetPathByPlatform(Application.streamingAssetsPath + "/data/clientData/LanguageData.json"));

		yield return uwr.SendWebRequest();

		if (uwr.isNetworkError || uwr.isHttpError) {
			Debug.Log(uwr.error);
			Debug.LogWarning("Language data load fail!");
			callBack?.Invoke("error");
			yield break;
		}

		string data_raw = uwr.downloadHandler.text;
		if (data_raw != null) {
			//data_raw = DeEncode.Decrypt(data_raw);
			Language_datas = JsonMapper.ToObject(data_raw);
		}
		else {
			Debug.LogWarning("Language data load fail!");
			callBack?.Invoke("error");
			yield break;
		}

		callBack?.Invoke("");
	}

	//Global
	private static IEnumerator update_data_Global(Action<string> callBack) {
		UnityEngine.Networking.UnityWebRequest uwr = UnityEngine.Networking.UnityWebRequest.Get(GetPathByPlatform(Application.streamingAssetsPath + "/data/clientData/globalValue.json"));

		yield return uwr.SendWebRequest();

		if (uwr.isNetworkError || uwr.isHttpError) {
			Debug.Log(uwr.error);
			Debug.LogWarning("Global data load fail!");
			callBack?.Invoke("error");
			yield break;
		}

		string data_raw = uwr.downloadHandler.text;
		if (data_raw != null) {
			//data_raw = DeEncode.Decrypt(data_raw);
			Global_datas = JsonMapper.ToObject(data_raw);			
		}
		else {
			Debug.LogWarning("Global data load fail!");
			callBack?.Invoke("error");
			yield break;
		}

		callBack?.Invoke("");
	}

}