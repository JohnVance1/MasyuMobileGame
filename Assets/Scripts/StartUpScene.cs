using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartUpScene : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                SceneManager.LoadScene(0);
            }

            
        }
        else if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(0);
        }
    }
}
