using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideFilteredPanel : MonoBehaviour
{
    [SerializeField] GameObject panel;
    int counter;
    

    public void ShowHidePanel()
    {
        counter++;
        if (counter % 2 == 1)
        {
            panel.gameObject.SetActive(false);
        }
        else
        {
            panel.gameObject.SetActive(true);
        }
    }


}
