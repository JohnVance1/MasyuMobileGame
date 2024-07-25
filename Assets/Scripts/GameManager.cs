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
        SetCameraPositions();
    }

    public void SetCameraPositions()
    {
        float width, height;

        width = (((grid.width * 2) - 1) * (grid.nodeWidth / 2)) / 2;
        height = ((grid.height - 1) * grid.nodeHeight * .75f) / 2;

        cam.transform.position = new Vector3(width, height, -10);

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
