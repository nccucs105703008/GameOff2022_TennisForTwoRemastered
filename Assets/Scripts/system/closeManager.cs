using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeManager : MonoBehaviour
{
    static closeManager instance;

    List<Action> close_mission;
    int closeLock; // > 0 會鎖定

    public static closeManager GetInstance()
    {
        if (instance == null)
        {
            GameObject temp = new GameObject("closeManager");
            instance = temp.AddComponent<closeManager>();
            //event_manager.AddListener(event_manager.EventType.change_scene, instance.initClose);
            instance.initClose();
            DontDestroyOnLoad(instance);
        }

        return instance;
    }

    public void initClose()
    {
        close_mission = new List<Action>();
        close_mission.Clear();
        closeLock = 0;
		inputManager.GetInstance().ReigistInputMission(KeyCode.Escape, UpdateMisson, inputManager.InputType.KeyDown);
	}

	private void OnDisable() {
		inputManager.GetInstance().RemoveInputMission(KeyCode.Escape, UpdateMisson, inputManager.InputType.KeyDown);
	}

	public void AddMission(Action mission)
    {
        closeManager.GetInstance().lockClose(true);
        if (close_mission == null) close_mission = new List<Action>();
        close_mission.Add(mission);
    }

    public void RemoveMission(Action mission)
    {
        if (close_mission == null) return;
        close_mission.Remove(mission);
    }

	//註冊給Input Manager
    private void UpdateMisson()
    {
        if (close_mission == null || closeLock > 0 || close_mission.Count <= 0) return;

		lockClose(true);
		close_mission[close_mission.Count - 1]?.Invoke();
		//Close_fromCloseManager()
	}

	public void lockClose(bool blockAwhile = false)
    {
        closeLock++;
        if(blockAwhile) Invoke("unlockClose", 0.25f);
    }

    public void unlockClose()
    {
        closeLock--;
    }


    /// <summary>
    /// all windows
    /// </summary>

    public void closeAllWindows()
    {
        ui_manager.GetInstance().closeAllUI();
    }

}
