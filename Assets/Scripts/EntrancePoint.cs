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

    private const float width = 3f;

    public int id;
    public int size = 3;
    public EntranceTypes entranceType;
    public Vector3 cubeSize;
    public Vector3 cubeCenter;

    private GameController _gameController;
    private ChamberController _chamber;
    private Vector3 _previousPosition;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _chamber = transform.parent.GetComponent<ChamberController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition == _previousPosition) return;
        _previousPosition = transform.localPosition;
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
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag != "Player") return;
        Debug.Log($"Entrance point touched entered: {id}");
        _gameController.ChangeChamber(id);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = entranceType == EntranceTypes.Error ? Color.red : Color.green;
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
        Gizmos.DrawCube(cubeCenter, cubeSize);
    }

}
