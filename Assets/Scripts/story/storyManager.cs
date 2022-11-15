using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storyManager : MonoBehaviour
{
	#region Static
	static storyManager instance;
	public static storyManager GetInstance() {
		if (instance == null) {
			instance = new GameObject("storyManager").AddComponent<storyManager>();
		}
		return instance;
	}

	#endregion

	storyController controller;

	/// <summary>
	/// Callback 要使用success 處理失敗或成功狀況
	/// </summary>
	/// <param name="ID"></param>
	/// <param name="doneBack"></param>
	public void playStory(int ID, List<string> participateList, Action<List<int>, bool> doneBack) {
		CoroutineHub.GetInstance().StartCoroutine(_playStory(ID, participateList, doneBack));
	}

	IEnumerator _playStory(int ID, List<string> participateList, Action<List<int>, bool> doneBack) {
		loadingManager.GetInstance().StartLoading();
		bool isLoading = true;
		storyData.information story = null;
		storyData.loadStory(ID, (data)=> {
			story = data;
			isLoading = false;
		});
		yield return new WaitWhile( ()=> isLoading == true );
		if (story == null) {
			doneBack?.Invoke(null, false);
			loadingManager.GetInstance().DoneLoading();
			yield break;
		}

		if (controller == null) {
			controller = Instantiate(FileManager.LoadPrefab("storyController"), popup_manager.GetInstance().Get_popLayer()).GetComponent<storyController>();
		}
		else {
			int count = popup_manager.GetInstance().Get_popLayer().childCount;
			controller.transform.SetSiblingIndex(count - 1);
		}

		//random
		for (int i = 0; i < participateList.Count; i++) {			
			int index = UnityEngine.Random.Range(0, participateList.Count);
			string temp = participateList[0];
			participateList[0] = participateList[index];
			participateList[index] = temp;
		}

		controller.playStory(story, participateList, doneBack);
		loadingManager.GetInstance().DoneLoading();
	}
}
