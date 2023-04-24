using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPZone : MonoBehaviour
{
    public bool deadZone;
    MeshRenderer skin;

    private void Awake()
    {
        skin = GetComponent<MeshRenderer>();
        StartCoroutine(zoneActive());
        
    }

    private void Update()
    {
        if (deadZone)
        {
            skin.enabled = true;
        }
        else
        {
            skin.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerEgg>() && deadZone)
        {
            other.GetComponent<PlayerEgg>().Damaged();
            StartCoroutine(zoneActive());
        }
    }
    IEnumerator zoneActive()
    {
        deadZone = false;
        yield return new WaitForSeconds(10f);
        deadZone = true;
    }
}
