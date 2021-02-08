using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Static;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum GameStates
    {
        Init,
        TransitionIn,
        Paused,
        Running,
        TransitionOut,
    }
    public static GameStates gameState
    {
        get => _gameState;
        set
        {
            _gameState = value;
            GameStateChanged?.Invoke(value);
            //Debug.Log($"Game state changed to {value}");
        }
    }

    public static bool loadedAlone;
    public static ChamberController currentChamber = null;
    public static event Action<GameObject> PlayerSpawned;
    public static event Action<GameStates> GameStateChanged;
    public static float gameTime;

    public GameObject playerPrefab;
    public GameObject gameUiGameObject;

    private Animator _animator;
    private CinemachineVirtualCamera _virtualCamera;
    private static GameController _instance;
    private static GameStates _gameState = GameStates.Init;

    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        _animator = GetComponent<Animator>();
        KillPlayer();
        gameUiGameObject.SetActive(true);
        GameOptionsManager.Init();
        DataHandling.Init();
        LocationInformation.Init(out _);
        if (!SaveSystem.Load(out SaveSystem.LastLoadedSave, out var errorMessage))
        {
            Debug.Log(errorMessage);
            var saveSuccess = SaveSystem.SaveVirgin(out var saveErrorMessage);
            Debug.Log(saveSuccess ? $"Game saved" : $"Game save unsuccessful : {saveErrorMessage}");
            if (saveSuccess)
            {
                SaveSystem.Load(out SaveSystem.LastLoadedSave, out errorMessage);
            }
        }
        gameTime = SaveSystem.LastLoadedSave.GameTime;
        DropController.Init();
    }

    void Start()
    {
        if (SaveSystem.LastLoadedSave.SavePointGuid == string.Empty)
        {
            CutsceneManager.Play(CutsceneManager.Cutscenes.Intro, SpawnPlayer);
        }
        else
        {
            _animator.SetTrigger("StartGame");
            SpawnPlayer();
        }
    }

    private void KillPlayer()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Destroy(player);
        }
    }

    private void SpawnPlayer()
    {
        //Debug.Log("Spawn");
        KillPlayer();
        DeathScreenController.Hide();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameStates.Running)
        {
            gameTime += Time.deltaTime;
        }
    }

    public static void Spawn()
    {
        _instance.SpawnPlayer();
    }

}
