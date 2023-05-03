using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Just a crappy character controller for the video
/// </summary>
public class PlayerController : NetworkBehaviour 
{
    public static PlayerController Instance { get; private set; }

    public BoxCollider corner;
    public GameObject egg;
    public Vector3 eggPlace;

    public Transform meeple;

    public bool hadEgg;

    [Header("Dash")]
    [SerializeField] private float dashPower = 24f;


    [SerializeField]private Animator anim;

    [Header("Charger")]
    protected float Timer;
    public float delayCharging;
    [SerializeField] bool charging;


    [Header("Movement")]
    [SerializeField] private float _curAcceleration = 80;
    [SerializeField] private float _normalAcceleration = 80;
    [SerializeField] private float _tiredAcceleration = 80;
    [SerializeField] private float _maxVelocity = 10;
    [SerializeField] public bool tiredLife;
    private Vector3 _input;
    private Rigidbody _rb;

    [SerializeField] private GameObject indicator;

    [SerializeField] private AudioSource audidRun;
    [SerializeField] private AudioSource audidWalk;

    [SerializeField] private AudioClip walkMap1, walkMap2;
   

    private void Awake()
    {
        Instance = this;
        _rb = GetComponent<Rigidbody>();
       
        corner = GetComponent<BoxCollider>();
        anim = GetComponentInChildren<Animator>();
        //audidRun = GetComponent<AudioSource>();

        indicator.SetActive(false);

        charging = false;
        tiredLife = false;


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

    private void Start()
    {


    }

    private void Update()
    {

        if (IsOwner)
        {
            //Debug.Log(CameraFollow.Instance);
            CameraFollow.Instance.player = this.transform;

        }

        _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        meeple = transform.Find("GO_Char_Telur_Basic_001");

        if (meeple == null)
        {
            hadEgg = false;
            indicator.SetActive(true);
            tiredLife = true;
            //corner.enabled = false;
        }
        else
        {
            hadEgg = true;
            indicator.SetActive(false);
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
        //HandleMovement();
        //HandleRotation();

    }

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            HandleMovement();
            HandleRotation();
        }
    }


    private void HandleMovement()
    {

        _rb.velocity += _input.normalized * (_curAcceleration * Time.deltaTime);
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxVelocity);

        if(_input.normalized != new Vector3(0,0,0))
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
            _rb.AddForce(transform.forward * dashPower, ForceMode.Impulse);

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

    

    #region Rotation

    [SerializeField] private float _rotationSpeed = 450;
    private Plane _groundPlane = new Plane(Vector3.up, Vector3.zero);
    [SerializeField]private Camera _cam;

    private void HandleRotation()
    {
        _cam = Camera.main;

        var ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (_groundPlane.Raycast(ray, out var enter))
        {
            var hitPoint = ray.GetPoint(enter);

            var dir = hitPoint - transform.position;
            dir.y = 0;
            var rot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _rotationSpeed * Time.deltaTime);
        }
    }

    #endregion

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
        //Collider myCollider = collision.GetContact(0).thisCollider;
        /*
        if (myCollider.gameObject.tag == "Dead Wall")
        {
            GetComponentInParent<ParentPlayer>().Destroyed = true;
        }
        
        /*
        if(collision.gameObject.transform.GetChild(0).tag == "Dead Wall")
        {
            //GetComponentInParent<ParentPlayer>().Destroyed = true;
            Debug.Log("WOW");
        }
        */


        //Debug.Log("Collider: " + collision.contacts[0].thisCollider.name);

    }

    private void OnCollisionStay(Collision collision)
    {
   
    }
}