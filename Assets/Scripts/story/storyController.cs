using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class storyController : MonoBehaviour
{
	public Text dialog;
	public Image bgImage;
	public GameObject PauseObj;

	storyData.information data;
	Action<List<int>, bool> doneCallback;
	List<string> participateList;
	bool isWaitForClick = false;
	bool isPause = false;

	public void playStory(storyData.information data, List<string>participateList, Action<List<int>, bool> doneCallback) {
		dialog.text = "";
		bgImage.color = new Color32(255, 255, 255, 0);
		isPause = false;
		if (data == null) {
			doneCallback?.Invoke(null, false);
			close();
		}
		this.data = data;
		this.doneCallback = doneCallback;
		this.participateList = participateList;
		isWaitForClick = false;
		inputManager.GetInstance().ReigistInputMission(KeyCode.Escape, pause);
		CoroutineHub.GetInstance().StartCoroutine(_playStory(data));
	}

	void pause() {
		isPause = !isPause;
		if (PauseObj) {
			PauseObj.SetActive(isPause);
		}
		
		if (isPause) {
			Time.timeScale = 0;
		}
		else {
			Time.timeScale = 1;
		}
	}

	IEnumerator _playStory(storyData.information data) {
		gameObject.SetActive(true);
		foreach (storyData.lineData line in data.storyList) {
			preSetLine(line);

			//Parse line
			IEnumerator e = parseLine(line);
			while (e.MoveNext())
			{
				yield return e.Current;
			}

			yield return null;
		}
		AudioManager.StopBGM();
		AudioManager.StopSound();
		inputManager.GetInstance().RemoveInputMission(KeyCode.Escape, pause);
		doneCallback?.Invoke(data.effect, true);
		yield return null;
		close();
	}

	IEnumerator parseLine(storyData.lineData line) {
		//<t=3.5> 等3.5S
		int index = 0;
		string nowText = "";
		dialog.text = "";
		line.line[languageManager.Language_index] = line.line[languageManager.Language_index].Replace("\\n", "\n");
		while (index < line.line[languageManager.Language_index].Length)
        {
			//處理指令
            if (line.line[languageManager.Language_index].Substring(index, 1) == "<")
            {
				index++;
				int cmdEndIndex = line.line[languageManager.Language_index].IndexOf('=', index);
				string cmdType = line.line[languageManager.Language_index].Substring(index, cmdEndIndex - index);
				index = cmdEndIndex + 1;
				switch (cmdType) {
					case "t": {
						int endIndex = line.line[languageManager.Language_index].IndexOf('>', index);
						string timeRaw = line.line[languageManager.Language_index].Substring(index, endIndex - index);
						float time = 0;
						if (float.TryParse(timeRaw, out time)) {
							yield return new WaitForSeconds(time);
						}
						index = endIndex + 1; //跳過>
						break;
					}
					case "bgm": {
						int endIndex = line.line[languageManager.Language_index].IndexOf('>', index);
						string name = line.line[languageManager.Language_index].Substring(index, endIndex - index);
						if (string.IsNullOrEmpty(name) == false) {
							AudioManager.PlayBGM(name);
						}
						index = endIndex + 1; //跳過>
						break;
					}
	
					case "sound": {
						int endIndex = line.line[languageManager.Language_index].IndexOf('>', index);
						string name = line.line[languageManager.Language_index].Substring(index, endIndex - index);
						if (string.IsNullOrEmpty(name) == false) {
							AudioManager.PlaySound(name);
						}
						index = endIndex + 1; //跳過>
						break;
					}						
					case "se": {
						int endIndex = line.line[languageManager.Language_index].IndexOf('>', index);
						string name = line.line[languageManager.Language_index].Substring(index, endIndex - index);
						if (string.IsNullOrEmpty(name) == false) {
							AudioManager.PlaySE(name);
						}
						index = endIndex + 1; //跳過>
						break;
					}
					case "p": {
						int endIndex = line.line[languageManager.Language_index].IndexOf('>', index);
						string pindexRaw = line.line[languageManager.Language_index].Substring(index, endIndex - index);
						int pIndex;
						if (Int32.TryParse(pindexRaw, out pIndex)) {
							if (pIndex < 0) {
								pIndex = participateList.Count - 1;
							}
						}
						else {
							pIndex = participateList.Count - 1;
						}
						for(int i = 0; i <= pIndex; i++) {
							string name = participateList[i];
							for (int y = 0; y < name.Length; y++) {
								nowText += name.Substring(y, 1);
								dialog.text = nowText;
								yield return null;
							}
							if(i != pIndex) {
								nowText += ",";
								dialog.text = nowText;
								yield return null;
							}
						}
						index = endIndex + 1; //跳過>
						break;
					}
					default:
						index = line.line[languageManager.Language_index].IndexOf('>', index) + 1;
						break;
				}
            }
            else 
            {
				nowText += line.line[languageManager.Language_index].Substring(index, 1);
				index++;
				yield return new WaitForSeconds(GlobalValueManager.GetInstance().Get_value<float>("storyCharacterWaitTime", 0.05f));
			}
			dialog.text = nowText;
			yield return null;
		}

        if (line.waitForClick) {
			isWaitForClick = true;
			yield return new WaitWhile(() => isWaitForClick);
        }
        else
        {
			yield return new WaitForSeconds(GlobalValueManager.GetInstance().Get_value<float>("storyLineWaitTime", 0.2f) * dialog.text.Length);
		}
	}

    void preSetLine(storyData.lineData line) {
		if (string.IsNullOrEmpty(line.bgImg) == false)
		{
			Sprite bg = FileManager.LoadSprite("BG", line.bgImg);
			if (bg != null)
			{
				bgImage.sprite = bg;
				bgImage.color = new Color32(255, 255, 255, 255);
			}
		}
		if (string.IsNullOrEmpty(line.bgm) == false)
		{
			if (line.bgm == "stopBGM")
			{
				AudioManager.StopBGM();
			}
			else
			{
				AudioManager.PlayBGM(line.bgm);
			}

		}
		if (string.IsNullOrEmpty(line.sound) == false)
		{
			if (line.bgm == "stopSound")
			{
				AudioManager.StopSound();
			}
			else
			{
				AudioManager.PlaySound(line.sound);
			}

		}
		if (string.IsNullOrEmpty(line.sfx) == false)
		{
			AudioManager.PlaySE(line.sfx);
		}
	}


	void close() {
		gameObject.SetActive(false);
	}

	public void Click()
    {
        if (isWaitForClick == true) {
			isWaitForClick = false;
		}
    }
}
