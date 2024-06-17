using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private Vector2 worldPoint;
    public Tilemap currentMap;

    // Update is called once per frame
    void Update()
    {

        worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int tpos = currentMap.WorldToCell(worldPoint);
            var tile = currentMap.GetTile(tpos);

            if (tile)
            {
                Debug.Log("click on " + tile.name);
                Debug.Log(tpos);
            }
           
        
   
                
        }
    }
}
