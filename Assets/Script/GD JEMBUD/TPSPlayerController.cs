using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class TPSPlayerController : NetworkBehaviour
{
    public static TPSPlayerController Instance { get; private set; }
    [Header("Movement")]
    [SerializeField] private float _curAcceleration = 2.0f;
    [SerializeField] private float _normalAcceleration = 30;
    [SerializeField] private float _tiredAcceleration = 20;
    [SerializeField] public bool tiredLife;

    [Header("Dash")]
    [SerializeField] private float dashPower = 24f;

    [Header("Egg")]
    public GameObject egg;
    public Vector3 eggPlace;

    public Transform meeple;
    public bool hadEgg;

    [Header("Charger")]
    protected float Timer;
    public float delayCharging;
    [SerializeField] bool charging;

    //[SerializeField] private float jumpHeight = 1.0f;
    //[SerializeField] private float gravityValue = -9.81f;
    [Header("test")]
    float moveSpeed = 2;
    float rotationSpeed = 4;
    float runningSpeed;
    float vaxis, haxis;
    public bool isJumping, isJumpingAlt, isGrounded = false;
    Vector3 movement;



    private Rigidbody _rb;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
    private Animator anim;


    //private InputAction jumpAction;

    [SerializeField] private GameObject indicator;

    [Header("Audio")]
    [SerializeField] private AudioSource audidRun;
    [SerializeField] private AudioSource audidWalk;

    [SerializeField] private AudioClip walkMap1, walkMap2;

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        //indicator.SetActive(false);



        charging = false;
        tiredLife = false;

        //audidWalk.enabled = false;
        //audidRun.enabled = false;

        if (SceneManager.GetActiveScene().name == ("Main"))
        {
            audidWalk.clip = walkMap1;
        }
        if (SceneManager.GetActiveScene().name == ("Main1"))
        {
            audidWalk.clip = walkMap2;
        }

    }
    private void Start()
    {

        cameraTransform = Camera.main.transform;
        //jumpAction = playerInput.actions["Jump"];
    }

    private void Update()
    {

        if (IsOwner)
        {
            //Debug.Log(CameraFollow.Instance);
            CameraFollow.Instance.player = this.transform;
        }

        meeple = transform.Find("GO_Char_Telur_Basic_001");

        if (meeple == null)
        {
            hadEgg = false;
            //indicator.SetActive(true);
            tiredLife = true;
            //corner.enabled = false;
        }
        else
        {
            hadEgg = true;
            //indicator.SetActive(false);
            //corner.enabled = true;

        }

        if (charging)
        {
            Timer += Time.deltaTime;

            if (Timer >= delayCharging)
            {
                Timer = 0f;
                var egg = GetComponentInChildren<PlayerEgg>();

                egg.curPowerEgg++;
            }
        }

        if (!tiredLife)
        {
            _curAcceleration = _normalAcceleration;
        }
        else
        {
            _curAcceleration = _tiredAcceleration;
        }
    }

    private void FixedUpdate()
    {
        /*  Controller Mappings */
        vaxis = Input.GetAxis("Vertical");
        haxis = Input.GetAxis("Horizontal");
        //isJumping = Input.GetButton("Jump");
        //isJumpingAlt = Input.GetKey(KeyCode.Joystick1Button0);

        //Simplified...
        runningSpeed = vaxis;


        if (isGrounded)
        {
            movement = new Vector3(0, 0f, runningSpeed * 8);        // Multiplier of 8 seems to work well with Rigidbody Mass of 1.
            movement = transform.TransformDirection(movement);      // transform correction A.K.A. "Move the way we are facing"
        }
        else
        {
            movement *= 0.70f;                                      // Dampen the movement vector while mid-air
        }

        GetComponent<Rigidbody>().AddForce(movement * moveSpeed);   // Movement Force

        /*
        if ((isJumping || isJumpingAlt) && isGrounded)
        {
            Debug.Log(this.ToString() + " isJumping = " + isJumping);
            GetComponent<Rigidbody>().AddForce(Vector3.up * 150);
        }
        */


        if ((Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f) && !isJumping && isGrounded)
        {
            if (Input.GetAxis("Vertical") >= 0)
                transform.Rotate(new Vector3(0, haxis * rotationSpeed, 0));
            else
                transform.Rotate(new Vector3(0, -haxis * rotationSpeed, 0));

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dead Wall") && hadEgg)
        {
            GetComponentInParent<ParentPlayer>().Destroyed = true;
        }

        if(other.gameObject.CompareTag("Charge Zone") && hadEgg)
        {
            charging = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Charge Zone") && hadEgg)
        {
            charging = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall") && hadEgg)
        {
            _rb.AddForce(transform.forward * dashPower, ForceMode.Impulse);
            //_rb.AddExplosionForce(50, transform.position, 10f, 3.0F);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("Entered");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
      //  Debug.Log("Exited");
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}