using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    public Transform player;
    public Vector3 offset;
    [SerializeField] float smoothTime;

    public bool x;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        x = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 desiredPos = player.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(this.transform.position, desiredPos, smoothTime);
            this.transform.position = smoothedPos;
        }

        if (x)
        {
            player = null;
            transform.position = new Vector3(7.3f, 63.3f, -1.2f);
        }
    }
}
