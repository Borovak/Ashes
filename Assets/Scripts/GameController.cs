using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
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
    public static ChamberController currentChamber = null;
    public GameObject playerPrefab;
    public GameStates gameState;

    private FadeInOutController _fadeInOutController;
    private CinemachineVirtualCamera _virtualCamera;

    // Start is called before the first frame update
    void Awake()
    {
        if (!init)
        {
            init = true;
            LocationInformation.Init();
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
        SpawnPlayer(out var playerTransform);
        _fadeInOutController = GameObject.FindGameObjectWithTag("FadeInOut").GetComponent<FadeInOutController>();
        _fadeInOutController.FadeIn();
        _virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        _virtualCamera.Follow = playerTransform;
        _virtualCamera.LookAt = playerTransform;
    }

    private void SpawnPlayer(out Transform playerTransform)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        var playerSpawnPosition = Vector3.zero;
        if (SaveData.workingData.SavePointGuid != string.Empty)
        {   
            foreach (var savePointGameObject in GameObject.FindGameObjectsWithTag("SavePoint"))
            {
                var savePointController = savePointGameObject.GetComponent<SavePointController>();
                if (savePointController.guid != SaveData.workingData.SavePointGuid) continue;
                playerSpawnPosition = savePointGameObject.transform.position;
                break;
            }
        }
        if (playerSpawnPosition == Vector3.zero)
        {
            playerSpawnPosition = new Vector3(163f, 78f, 0f);
        }
        playerTransform = GameObject.Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState != GameStates.Paused)
        {
            SaveData.workingData.GameTime += Time.deltaTime;
        }
    }

    public void GoToSavePoint(string savePointGuid)
    {
        _fadeInOutController.FadeOutCompleted += () =>
        {
            var savePoint = LocationInformation.SavePoints[savePointGuid];
            Debug.Log($"Changing location for chamber {savePoint.Chamber.Name}");
            //var chamber = LocationManager.GetChamber(id);
            //UnityEngine.SceneManagement.SceneManager.LoadScene(chamber.sceneName);
        };
        _fadeInOutController.FadeOut();
    }

}
