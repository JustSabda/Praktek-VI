using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    /*
    public Transform target;
    public float rotationSpeed = 3f;
    */

    [SerializeField] private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position-transform.position), rotationSpeed*Time.deltaTime);
        
        Vector3 targetPosition = new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z);
        transform.LookAt(targetPosition);
        //transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);
        //this.gameObject.transform.rotation = Quaternion.Euler(transform.position.x + 90, transform.position.y, transform.position.z);

    }
}
