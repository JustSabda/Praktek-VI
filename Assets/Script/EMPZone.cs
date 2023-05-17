using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPZone : MonoBehaviour
{
    public bool deadZone;
    MeshRenderer skin;
    [SerializeField] private GameObject Tai;

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
            Tai.SetActive (true);
        }
        else
        {
            skin.enabled = false;
            Tai.SetActive(false);
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
