using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool paused;

    private FadeInOutController _fadeInOutController;


    // Start is called before the first frame update
    void Awake()
    {
        var loadMessage = SaveSystem.Load();
        Debug.Log(loadMessage != "" ? loadMessage : "Game loaded successfully");
    }

    void Start()
    {
        _fadeInOutController = GameObject.FindGameObjectWithTag("FadeInOut").GetComponent<FadeInOutController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeChamber(int id)
    {
        _fadeInOutController.FadeOut();
        _fadeInOutController.FadeOutCompleted += () =>
        {
            var chamber = LocationManager.GetChamber(id);
            UnityEngine.SceneManagement.SceneManager.LoadScene(chamber.sceneName);
        };
    }
    
}
