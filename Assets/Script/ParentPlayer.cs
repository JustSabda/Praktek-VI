using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ParentPlayer : NetworkBehaviour
{

    [HideInInspector] public bool Destroyed;
    [SerializeField]private GameObject player;
    [SerializeField]private GameObject egg;

    //[SerializeField] private PlayerEgg _egg;

    [SerializeField] private float positionRange = 5f;

    private void Awake()
    {
        Destroyed = false;
        /*
        var projectile = Instantiate(egg, new Vector3(1, 2, 2), Quaternion.identity, this.transform);
        projectile.GetComponent<PlayerEgg>().core = this.transform;
        */
    }

    public override void OnNetworkSpawn()
    {
        /*
        if (IsOwner && IsClient)
        {
            //Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
            //transform.position = new Vector3(spawnpoint.position.x, 0, spawnpoint.position.z);
            //transform.position = new Vector3(Random.Range(positionRange, -positionRange), 0, Random.Range(positionRange, -positionRange));
        }
        */
    }


    void Update()
    {
        if (IsOwner && IsClient)
        {
       

            if (Destroyed)
            {
                //Destroy(this.gameObject);
                //CameraFollow.Instance.player = null;
                //CameraFollow.Instance.player.position += new Vector3(0, 0, 0) ;

                CameraFollow.Instance.x = true;

                MeledukServerRpc();
                MeledukReal();

            }
        }
    }

    [ServerRpc]
    private void MeledukServerRpc()
    {
        MeledukClientRpc();
    }

    [ClientRpc]
    private void MeledukClientRpc()
    {
        if (!IsOwner) MeledukReal();
    }


    private void MeledukReal ()
    {
        player.SetActive(false);
        egg.SetActive(false);
    }


}
