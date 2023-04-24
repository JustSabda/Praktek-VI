using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnPoint : NetworkBehaviour
{

	[SerializeField] GameObject graphics;

	void Awake()
	{
		graphics.SetActive(false);
	}
}
