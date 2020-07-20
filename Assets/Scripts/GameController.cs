using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameStates
    {
        TransitionIn,
        Paused,
        Running,
        TransitionOut,
    }

    public static bool init;
    public static bool loadedAlone;
    public GameObject playerPrefab;
    public GameStates gameState;

    private FadeInOutController _fadeInOutController;


    // Start is called before the first frame update
    void Awake()
    {
        if (!init)
        {
            init = true;
            LocationManager.Load();
        }
        //Loading default save if none loaded
        if (SaveData.workingData == null)
        {
            Debug.Log($"Save data not present on start");
            var loadSuccessful = SaveSystem.Load(out _, out var errorMessage);
            if (!loadSuccessful)
            {
                Debug.Log(errorMessage);
                var saveSuccess = SaveSystem.Save(out var saveErrorMessage);
                Debug.Log(saveSuccess ? $"Game saved" : $"Game save unsuccessful : {saveErrorMessage}");
            }
        }
    }

    void Start()
    {
        _fadeInOutController = GameObject.FindGameObjectWithTag("FadeInOut").GetComponent<FadeInOutController>();
        SpawnPlayer();
    }

    private void SpawnPlayer(){
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        var playerSpawnPosition = Vector3.zero;
        if (SaveData.workingData.SavePointId == -1)
        {
            var savePoint = GameObject.FindGameObjectWithTag("SavePoint");
            playerSpawnPosition = savePoint.transform.position;
        }
        else
        {
            var entrancePoints = GameObject.FindGameObjectsWithTag("EntrancePoint");
            foreach (var entrancePointObject in entrancePoints)
            {
                var entrancePoint = entrancePointObject.GetComponent<EntrancePoint>();
                if (entrancePoint.id != SaveData.workingData.SavePointId) continue;
                playerSpawnPosition = entrancePoint.transform.position;
                PlayerPlatformerController.transitionMovement = entrancePoint.entranceMovement;
                entrancePoint.isNumb = true;
                Debug.Log($"Entrance point detected");
                break;
            }
        }
        GameObject.Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != GameStates.Paused)
        {
            SaveData.workingData.GameTime += Time.deltaTime;
        }
    }

    public void ChangeChamber(int id, int entranceId)
    {
        SaveData.workingData.SavePointId = entranceId;
        _fadeInOutController.FadeOut();
        _fadeInOutController.FadeOutCompleted += () =>
        {
            Debug.Log($"Changing location for chamber {id}");
            var chamber = LocationManager.GetChamber(id);
            UnityEngine.SceneManagement.SceneManager.LoadScene(chamber.sceneName);
        };
    }

}
