using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
using Unity.Services.Lobbies.Models;
using Unity;

public class PlayerEgg : NetworkBehaviour
{
    public static PlayerEgg Instance { get; private set; }

    Rigidbody rb;
    MeshCollider capsule;
    public enum state { DEFAULT, GONE, PICKUP, CHARGING , POWEROUT }
    public state _state = state.DEFAULT;

    /*
    [Header("DownTime")]
    [SerializeField] float currentTime;
    [SerializeField] float startingTime;
    */

    
    public TMP_Text powerText;
    public Color color;

    public Transform core;
    //public GameObject fixCore;

    public Vector3 eggPlacement;


    public float curPowerEgg;
    public float maxpowerEgg;


    private bool x;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<MeshCollider>();
        rb.isKinematic = true;
        //powerText.text = curPowerEgg.ToString();

    }

    private void Awake()
    {

    }


    void Update()
    {
        if (!IsOwner) return;

        //this.gameObject.transform.SetParent(this.transform);

        if (_state == state.GONE)
        {
            //PlayerController.Instance.hadEgg = false;


            /*
            if (done)
            {
                currentTime = startingTime;
                done = false;
            }
            
            currentTime -= 1 * Time.deltaTime;
            countdownText.text = currentTime.ToString("0");
            
            if (currentTime <= 0)
            {
                currentTime = 0;
                _state = state.PICKUP;

            }
            */
        }

        if (Input.GetKey(KeyCode.E))
        {
            StartCoroutine(Pickup());
        }

        if (curPowerEgg >= maxpowerEgg)
        {
            curPowerEgg = maxpowerEgg;
        }

        

        if(curPowerEgg <= 0)
        {
            _state = state.POWEROUT;
            curPowerEgg = 0;
        }
        else
        {
            _state = state.DEFAULT;
        }

        powerText.text = curPowerEgg + "";
    }

    IEnumerator Pickup()
    {
        x = true;
        yield return new WaitForSeconds(0.5f);
        x = false;
    }

    public void Damaged()
    {
        //if (_state != state.GONE || _state != state.PICKUP)


        backParent(core);
        rb.isKinematic = false;
        rb.AddForce(transform.forward * -10f, ForceMode.Impulse);
        _state = state.GONE;
        //done = true;
        capsule.isTrigger = false;


    }
    public void backParent(Transform parent)
    {
        this.gameObject.transform.SetParent(parent , true);
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (IsOwner)
        {
            

            if (collision.gameObject.tag == "Dead Wall")
            {
                GetComponentInParent<ParentPlayer>().Destroyed = true;
            }

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (IsOwner)
        {
            if (collision.gameObject.tag == "Player" && x == true) //_state == state.PICKUP)
            {
                this.gameObject.transform.SetParent(collision.transform, false);

                EggTimeServerRpc();
                EggTime();

                rb.isKinematic = true;
                //countdownText.text = currentTime.ToString("3");
            }
        }
    }

    [ServerRpc]
    private void EggTimeServerRpc()
    {
        EggTimeClientRpc();
    }

    [ClientRpc]
    private void EggTimeClientRpc()
    {
        if (!IsOwner) EggTime();
    }


    private void EggTime()
    {
        transform.localPosition = eggPlacement;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
    }

}
