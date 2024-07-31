using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectManagerScript : MonoBehaviour
{
    public List<GameObject> LevelButtons = new List<GameObject>();

    

    void Start()
    {
        LevelBaseObject level = LevelTransferScript.Instance.currentLevel;

        if (level != null)
        {
            if (level.IsComplete)
            {
                LevelButtons[LevelTransferScript.Instance.LevelNum-1].GetComponent<Button>().interactable = false;
            }
        }
    }

    private void OnEnable()
    {
        for(int i = 0; i < LevelButtons.Count; i++)
        {
            if (GameManager.Instance.levels[i].IsComplete)
            {
                LevelButtons[i].GetComponent<Button>().interactable = false;
            }
        }
        
    }


}
