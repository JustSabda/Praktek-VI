using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterSpawner : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterDatabase characterDatabase;

    public override void OnNetworkSpawn()
    {
       if (!IsServer) { return; }


        Debug.Log(ServerManager.Instance.ClientData.Count);

        foreach (var client in ServerManager.Instance.ClientData)
        {
            Debug.Log(client.Value.clientId);
            var character = characterDatabase.GetCharacterById(client.Value.characterId);
            if (character != null)
            {
                Debug.Log(client.Value.clientId);
                var spawnPos = new Vector3(Random.Range(-3f, 3f), 3f, Random.Range(-3f, 3f));
                var characterInstance = Instantiate(character.GameplayPrefab, spawnPos, Quaternion.identity);
                characterInstance.SpawnAsPlayerObject(client.Value.clientId);
                
            }
        }
        
    }
}
