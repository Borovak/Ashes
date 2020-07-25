using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EntrancePoint : MonoBehaviour
{

    public enum EntranceTypes
    {
        Error,
        Inner,
        Left,
        Top,
        Right,
        Bottom
    }

    public enum VerticalSpawnSides
    {
        Left = 0,
        Right = 1,
    }

    private const float width = 3f;

    public int id;
    public int linkChamberId;
    public int linkEntranceId;
    public int size = 3;
    public EntranceTypes entranceType;
    public VerticalSpawnSides verticalSpawnSide;
    public Vector3 cubeSize;
    public Vector3 cubeCenter;
    public bool isNumb;
    public Vector3 entranceMovement;
    public Vector3 exitMovement;

    private GameController _gameController;
    private ChamberController _chamber;
    private BoxCollider2D _collider;
    private Vector3 _previousPosition;

    void Awake()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _chamber = transform.parent.GetComponent<ChamberController>();
        _collider = GetComponent<BoxCollider2D>();
        if (Application.isPlaying)
        {
            DetermineDestinationMovement();
            SpawnOutsideFloor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isPlaying) return;
        if (transform.localPosition == _previousPosition) return;
        _previousPosition = transform.localPosition;
        DetermineType();
        DetermineSize();
        DeterminePosition();
        DetermineDestinationMovement();
        SetCollider();
    }

    private void DetermineType()
    {
        if (transform.localPosition.x == 0)
        {
            entranceType = EntranceTypes.Left;
        }
        else if (transform.localPosition.x == _chamber.size.x)
        {
            entranceType = EntranceTypes.Right;
        }
        else if (transform.localPosition.y == 0)
        {
            entranceType = EntranceTypes.Bottom;
        }
        else if (transform.localPosition.y == _chamber.size.y)
        {
            entranceType = EntranceTypes.Top;
        }
        else if (transform.localPosition.x < 0 || transform.localPosition.x > _chamber.size.x || transform.localPosition.y < 0 || transform.localPosition.y > _chamber.size.y)
        {
            entranceType = EntranceTypes.Error;
        }
        else
        {
            entranceType = EntranceTypes.Inner;
        }
    }

    private void DetermineSize()
    {
        cubeSize = Vector3.zero;
        switch (entranceType)
        {
            case EntranceTypes.Inner:
            case EntranceTypes.Error:
            case EntranceTypes.Left:
            case EntranceTypes.Right:
                cubeSize.x = width;
                cubeSize.y = size;
                break;
            case EntranceTypes.Top:
            case EntranceTypes.Bottom:
                cubeSize.x = size;
                cubeSize.y = width;
                break;
        }
    }

    private void DeterminePosition()
    {
        cubeCenter = Vector3.zero;
        switch (entranceType)
        {
            case EntranceTypes.Inner:
            case EntranceTypes.Error:
            case EntranceTypes.Left:
                cubeCenter.x = transform.localPosition.x + width / 2f;
                cubeCenter.y = transform.localPosition.y + size / 2f;
                break;
            case EntranceTypes.Right:
                cubeCenter.x = transform.localPosition.x - width / 2f;
                cubeCenter.y = transform.localPosition.y + size / 2f;
                break;
            case EntranceTypes.Top:
                cubeCenter.x = transform.localPosition.x + size / 2f;
                cubeCenter.y = transform.localPosition.y + width / 2f;
                break;
            case EntranceTypes.Bottom:
                cubeCenter.x = transform.localPosition.x + size / 2f;
                cubeCenter.y = transform.localPosition.y - width / 2f;
                break;
        }
    }

    private void DetermineDestinationMovement()
    {
        switch (entranceType)
        {
            case EntranceTypes.Inner:
            case EntranceTypes.Error:
                entranceMovement = Vector3.zero;
                exitMovement = Vector3.zero;
                break;
            case EntranceTypes.Left:
                entranceMovement = Vector3.right;
                exitMovement = Vector3.left;
                break;
            case EntranceTypes.Right:
                entranceMovement = Vector3.left;
                exitMovement = Vector3.right;
                break;
            case EntranceTypes.Top:
                entranceMovement = Vector3.zero;
                exitMovement = Vector3.up;
                break;
            case EntranceTypes.Bottom:
                entranceMovement = verticalSpawnSide == VerticalSpawnSides.Left ? Vector3.left : Vector3.right;
                exitMovement = Vector3.down;
                break;
        }
    }

    private void SpawnOutsideFloor()
    {
        var collider = gameObject.AddComponent<EdgeCollider2D>();
        var points = new List<Vector2>();
    }

    private void SetCollider()
    {
        _collider.size = cubeSize;
        _collider.offset = cubeCenter - transform.localPosition;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (linkChamberId == -1 || isNumb || col.gameObject.tag != "Player") return;
        Debug.Log($"Entrance point touched entered: {id}");
        PlayerPlatformerController.transitionMovement = exitMovement;
        _gameController.gameState = GameController.GameStates.TransitionOut;
        _gameController.ChangeChamber(linkChamberId, linkEntranceId, entranceType);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player") return;
        isNumb = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = entranceType == EntranceTypes.Error ? Color.red : Color.green;
        Gizmos.DrawCube(cubeCenter, cubeSize);
    }

}
