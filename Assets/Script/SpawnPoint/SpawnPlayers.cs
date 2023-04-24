using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;

    /*
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    */

    private void Start()
    {
        /*
        Vector3 randomPosition = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
        */
        

        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();
        Vector3 position123 = new Vector3(spawnpoint.position.x, 0, spawnpoint.position.z);
        //PhotonNetwork.Instantiate(playerPrefab.name, position123 , Quaternion.identity);
        
    }


}
