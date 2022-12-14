using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingManager : MonoBehaviour
{
    GameObject loadOBJ_instance;
    GameObject blockOBJ_instance;
    static loadingManager instance;

    int loading_Call;

    public static loadingManager GetInstance()
    {
        if (instance == null)
        {
            GameObject temp = new GameObject("loading_manager");
            instance = temp.AddComponent<loadingManager>();
            //event_manager.AddListener(event_manager.EventType.change_scene, instance.initLoad);
            DontDestroyOnLoad(instance);
        }
        return instance;
    }

    public void initLoad()
    {
        if(loadOBJ_instance != null) loadOBJ_instance.SetActive(false);
        if (blockOBJ_instance != null) blockOBJ_instance.SetActive(false);
        loading_Call = 0;
    }

    public void DoneLoading()
    {
        loading_Call--;
        if (loading_Call <= 0)
        {
            loadOBJ_instance.SetActive(false);
            closeblockScreen();
            loading_Call = 0;
        }
        closeManager.GetInstance().unlockClose();
    }

    public void StartLoading(bool needBlackBlock = false)
    {
        closeManager.GetInstance().lockClose();

        if (needBlackBlock) blockScreen(true);

        if (loadOBJ_instance == null)
        {
            loadOBJ_instance = Instantiate(FileManager.LoadPrefab("loadingOBJ"), ui_manager.GetInstance().Get_UILayer());
        }
        else
        {
            int count = ui_manager.GetInstance().Get_UILayer().childCount;
            loadOBJ_instance.transform.SetSiblingIndex(count - 1);
        }


        loading_Call++;
        if (loading_Call > 0)
        {
            loadOBJ_instance.SetActive(true);
        }
    }

    public void blockScreen(bool needBlackBlock = false, string label = "")
    {
        if (blockOBJ_instance == null)
        {
            blockOBJ_instance = Instantiate(FileManager.LoadPrefab("blockOBJ"), ui_manager.GetInstance().Get_UILayer());
        }
        else
        {
            int count = ui_manager.GetInstance().Get_UILayer().childCount;
            blockOBJ_instance.transform.SetSiblingIndex(count - 1);
        }

        blockOBJ_instance.GetComponent<Image>().color = new Color(0,0,0, needBlackBlock? 1:0);
		Transform labelObj = blockOBJ_instance.transform.GetChild(0);
		if (labelObj != null && labelObj.GetComponent<Text>() != null) {
			labelObj.GetComponent<Text>().text = label;
		}
		blockOBJ_instance.SetActive(true);
    }

    public void closeblockScreen()
    {
        if(blockOBJ_instance != null) blockOBJ_instance.SetActive(false);
    }

}
