using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XXThirdPersonCam : MonoBehaviour
{
    public static XXThirdPersonCam Instance { get; private set; }

    [Header("References")]
    public Transform orientation;
    public Transform player;
    //public Transform playerObj;
    //public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;


    private void Start()
    {
        Instance = this;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x,transform.position.y,transform.position.z);
        orientation.forward = viewDir.normalized;

        Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
        orientation.forward = dirToCombatLookAt.normalized;

        player.forward = dirToCombatLookAt.normalized;

        //playerObj.forward = dirToCombatLookAt.normalized;
    }
}
