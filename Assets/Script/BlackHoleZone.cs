using System.Collections;
using System.Collections.Generic;
using CircularGravityForce;
using UnityEngine;

public class BlackHoleZone : MonoBehaviour
{
    public bool blackHole;
    MeshRenderer skin;
    CGF cGF;
    

    private void Awake()
    {
        cGF = GetComponent<CGF>();
        skin = GetComponent<MeshRenderer>();
        StartCoroutine(zoneActive());
        cGF.enabled = false;

    }

    private void Update()
    {
        if (cGF.enabled == true)
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
        if (other.GetComponent<PlayerController>() && cGF.enabled == true)
        {
            StartCoroutine(zoneActive());
        }
    }
    IEnumerator zoneActive()
    {
        yield return new WaitForSeconds(3f);
        cGF.enabled = false;
        yield return new WaitForSeconds(10f);
        cGF.enabled = true;
    }

}
