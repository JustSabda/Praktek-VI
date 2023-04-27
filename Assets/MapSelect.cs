using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelect : MonoBehaviour
{

    public GameObject panelMap;

    private bool opened;

    private void Start()
    {
        panelMap.SetActive(false);
        opened = false;
    }

    public void openPanel()
    {
        opened = !opened;

        if (opened)
        {
            panelMap.SetActive(true);
        }
        else
        {
            panelMap.SetActive(false);
        }
    }
    public void MapPick(string map)
    {
        ServerManager.Instance.map = map;
    }

}
