using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class testRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        /*var t = Task.Factory.StartNew(
            () => test(1),
            TaskCreationOptions.LongRunning
        );
        bool r = t.Result.Result;
        Debug.Log("XD");*/

        Debug.Log("XD");
        Debug.Log(Language_manager.GetLanguage_value("none"));
    }

    async Task<bool> test(int testV)
    {
        Debug.Log("wait");
        await Task.Delay(5000);
        Debug.Log("wait done");
        MethodAsync();
        return true;
    }

    // Asynchronous method
    public async void MethodAsync()
    {
        await Task.Delay(5000);
        Debug.Log("wait done2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
