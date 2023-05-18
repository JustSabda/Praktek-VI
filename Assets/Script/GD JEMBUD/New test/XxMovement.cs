using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class XxMovement : NetworkBehaviour
{

    public Transform aim;

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    

    //TOP DONT CHANGE

    public static XxMovement Instance { get; private set; }

    [Header("Movement")]
    //[SerializeField] private float _curAcceleration = 2.0f;
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

    private Animator anim;


    //private InputAction jumpAction;

    [SerializeField] private GameObject indicator;

    [Header("Audio")]
    [SerializeField] private AudioSource audidRun;
    [SerializeField] private AudioSource audidWalk;

    [SerializeField] private AudioClip walkMap1, walkMap2;

    private AudioListener listener;

    private bool holdAim;

    private void Awake()
    {
        Instance = this;
    }



    private void Start()
    {
        if (IsOwner)
        {
            //AimCamera.Instance.playerInput = GetComponent<PlayerInput>();



        }
        rb = GetComponent<Rigidbody>();
        listener = GetComponent<AudioListener>();

        

        charging = false;
        tiredLife = false;

        anim = GetComponentInChildren<Animator>();
        indicator.SetActive(false);

        audidWalk.enabled = false;
        audidRun.enabled = false;

        if (SceneManager.GetActiveScene().name == ("Main"))
        {
            audidWalk.clip = walkMap1;
        }
        if (SceneManager.GetActiveScene().name == ("Main1"))
        {
            audidWalk.clip = walkMap2;
        }
    }

    private void Update()
    {
        //Network
        if (IsOwner)
        {
            //Camera
            XXThirdPersonCam.Instance.orientation = orientation.transform;
            XXThirdPersonCam.Instance.player = this.transform;

            //XXThirdPersonCam.Instance.rb = rb;
            XXThirdPersonCam.Instance.combatLookAt = aim;

            listener.enabled = true;


            //Debug.Log(CameraFollow.Instance);
            CameraFollow.Instance.player = this.transform;
            CameraFollow.Instance.aimPlayer = aim.transform;

            AimCamera.Instance.isAim = holdAim;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            holdAim = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            holdAim = false;
        }

        meeple = transform.Find("GO_Char_Telur_Basic_001");
        rb.freezeRotation = true;

        if (meeple == null)
        {
            hadEgg = false;
            
            indicator.SetActive(true);
            EggPanel.Instance.egg.SetActive(false);
            EggPanel.Instance.eggless.SetActive(true);
            tiredLife = true;
            //corner.enabled = false;
        }
        else
        {
            hadEgg = true;

            indicator.SetActive(false);
            //corner.enabled = true;

            EggPanel.Instance.egg.SetActive(true);
            EggPanel.Instance.eggless.SetActive(false);

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
            moveSpeed = _normalAcceleration;
        }
        else
        {
            moveSpeed = _tiredAcceleration;
        }

        //ground check IMPORTANT CHANGE
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        /*
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        */
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        if (verticalInput != 0 || horizontalInput != 0)
        {
            anim.SetBool("Jalan", true);
            audidWalk.enabled = true;
        }
        else
        {
            anim.SetBool("Jalan", false);
            audidWalk.enabled = false;
        }

        if (Input.GetKey(KeyCode.LeftShift) && !tiredLife)
        {
            rb.AddForce(transform.forward * dashPower, ForceMode.Impulse);

            //animation dash
            anim.SetBool("Dash", true);
            Timer += Time.deltaTime;
            audidRun.enabled = true;
            if (Timer >= delayCharging)
            {
                Timer = 0f;
                var egg = GetComponentInChildren<PlayerEgg>();

                egg.curPowerEgg--;
            }
        }
        else
        {
            anim.SetBool("Dash", false);
            audidRun.enabled = false;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Dead Wall") && hadEgg)
        {
            GetComponentInParent<ParentPlayer>().Destroyed = true;
        }

        if (other.gameObject.CompareTag("Charge Zone") && hadEgg)
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
            rb.AddForce(transform.forward * dashPower, ForceMode.Impulse);
            //_rb.AddExplosionForce(50, transform.position, 10f, 3.0F);
        }
    }


}
