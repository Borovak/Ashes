using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void ExitApplication()
    {
        Application.Quit();
    }

    public void ExitToMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void DeleteSaveGame(int index)
    {
        SaveSystem.Delete(index);
    }
    
    public void SetCanvasTrigger(string triggerName)
    {
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<Animator>().SetTrigger(triggerName);
    }
    
    public void SetLoadCreateTrigger(string triggerName)
    {
        GameObject.FindGameObjectWithTag("LoadCreatePanel").GetComponent<Animator>().SetTrigger(triggerName);
    }
}
