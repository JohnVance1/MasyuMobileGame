using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    public HexagonGrid grid;
    public List<LevelBaseObject> levels;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }
        DontDestroyOnLoad(Instance);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        grid = GameObject.FindObjectOfType<HexagonGrid>();
        if(grid != null)
        {
            //if(LevelTransferScript.Instance.currentLevel != null)
            //{
            //    grid.LevelInfo = LevelTransferScript.Instance.currentLevel;
            //}
            //else
            //{
            //    grid.LevelInfo = levels[LevelTransferScript.Instance.LevelNum - 1];
            //}
            grid.LevelInfo = levels[LevelTransferScript.Instance.LevelNum - 1];
        }
    }

    public void SavesEdges()
    {
        grid.LevelInfo.Edges = grid.Edges;
    }
    
}
