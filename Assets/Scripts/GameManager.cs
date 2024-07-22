using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    public TouchManager touchManager;
    public HexagonGrid grid;
    public List<LevelBaseObject> levels;
    public Camera cam;

    private void Awake()
    {
        grid.LevelInfo = levels[/*LevelTransferScript.Instance.LevelNum - 1*/0];
    }

    private void Start()
    {
        cam.transform.position = new Vector3(grid.transform.position.x, grid.transform.position.y, -10);
    }

    private void Update()
    {
        Debug.Log(grid.transform.localPosition);
        Debug.Log(grid.transform.position);


    }


    private void OnEnable()
    {
        TouchManager.addEdge += grid.AddAnEdge;
        TouchManager.removeEdge += grid.RemoveAnEdge;

    }

    private void OnDisable()
    {
        TouchManager.addEdge -= grid.AddAnEdge;
        TouchManager.removeEdge -= grid.RemoveAnEdge;

    }
}
