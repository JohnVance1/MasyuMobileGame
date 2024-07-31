using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TouchManager touchManager;
    public HexagonGrid grid;
    public Camera cam;



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
