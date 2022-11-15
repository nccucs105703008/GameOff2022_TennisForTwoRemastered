using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class storyData : MonoBehaviour
{
	[Serializable]
	public class information
	{
		public int ID;
		public List<int> effect = new List<int>();
		public List<lineData> storyList = new List<lineData>();
	}


	[Serializable]
	public class lineData {
		public string bgm = "";
		public string sound = "";
		public string sfx = "";
		public string bgImg = "";
		public string fx = ""; //目前未用
		public bool waitForClick = false;
		public List<string> line = new List<string>();
	}

	static Dictionary<int, storyData.information> stories;

	/// <summary>
	/// return true if load success
	/// </summary>
	/// <param name="ID"></param>
	/// <param name="callBack"></param>
	public static void loadStory(int ID, Action<storyData.information> callBack) {
		if (stories == null) {
			stories = new Dictionary<int, storyData.information>();
		}
		if (stories.ContainsKey(ID) && stories[ID] != null) {
			callBack?.Invoke(stories[ID]);
		}
		else {
			CoroutineHub.GetInstance().StartCoroutine(load_data_story(ID,
				(data) => {
					if (data != null) {
						stories.Add(ID, data);
					}
					callBack?.Invoke(data);
				}
			));
		}
	}

	static IEnumerator load_data_story(int ID, Action<storyData.information> callBack) {
		UnityEngine.Networking.UnityWebRequest uwr = UnityEngine.Networking.UnityWebRequest.Get(JsonDataBase.GetPathByPlatform(string.Format(Application.streamingAssetsPath + "/data/clientData/story/story_{0}.json", ID)));

		yield return uwr.SendWebRequest();

		if (uwr.isNetworkError || uwr.isHttpError) {
			Debug.Log(uwr.error);
			Debug.LogWarning("story data load fail! | ID: " + ID);
			callBack?.Invoke(null);
			yield break;
		}
		storyData.information data = null;
		string data_raw = uwr.downloadHandler.text;
		if (data_raw != null) {
			//data_raw = DeEncode.Decrypt(data_raw);
			data = LitJson.JsonMapper.ToObject<storyData.information>(data_raw);
		}
		else {
			Debug.LogWarning("story data load fail! | ID: " + ID);
			callBack?.Invoke(null);
			yield break;
		}

		callBack?.Invoke(data);
	}
}
