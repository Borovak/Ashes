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
        ActionMenu,
        TransitionOut,
    }

    public static bool loadedAlone;
    public static ChamberController currentChamber = null;
    public static event Action<GameObject> PlayerSpawned;
    public GameObject playerPrefab;
    public GameStates gameState;
    public float gameTime;

    private FadeInOutController _fadeInOutController;
    private CinemachineVirtualCamera _virtualCamera;

    // Start is called before the first frame update
    void Awake()
    {
        _fadeInOutController = GameObject.FindGameObjectWithTag("FadeInOut").GetComponent<FadeInOutController>();
        _fadeInOutController.FadeOutCompleted += SpawnPlayer;
        DataHandling.Init();
        LocationInformation.Init(out _);
        var loadSuccessful = SaveSystem.Load(out SaveSystem.LastLoadedSave, out var errorMessage);
        if (!loadSuccessful)
        {
            Debug.Log(errorMessage);
            var saveSuccess = SaveSystem.SaveVirgin(out var saveErrorMessage);
            Debug.Log(saveSuccess ? $"Game saved" : $"Game save unsuccessful : {saveErrorMessage}");
            if (saveSuccess){
                SaveSystem.Load(out SaveSystem.LastLoadedSave, out errorMessage);
            }
        }
        DropController.Init();
    }

    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
        var playerSpawnPosition = Vector3.zero;
        if (SaveSystem.LastLoadedSave.SavePointGuid != string.Empty)
        {
            foreach (var savePointGameObject in GameObject.FindGameObjectsWithTag("SavePoint"))
            {
                var savePointController = savePointGameObject.GetComponent<SavePointController>();
                if (savePointController.guid != SaveSystem.LastLoadedSave.SavePointGuid) continue;
                playerSpawnPosition = savePointGameObject.transform.position;
                break;
            }
        }
        if (playerSpawnPosition == Vector3.zero)
        {
            playerSpawnPosition = new Vector3(163f, 78f, 0f);
        }
        var playerTransform = GameObject.Instantiate(playerPrefab, playerSpawnPosition, Quaternion.identity).transform;
        PlayerSpawned?.Invoke(playerTransform.gameObject);
        _virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        _virtualCamera.Follow = playerTransform;
        _virtualCamera.LookAt = playerTransform;
        _fadeInOutController.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameStates.Running)
        {
            gameTime += Time.deltaTime;
        }
    }

}
