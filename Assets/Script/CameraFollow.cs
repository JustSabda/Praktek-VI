using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance { get; private set; }

    public Transform player;
    public Transform aimPlayer;

    public Vector3 offset;
    [SerializeField] float smoothTime;

    public bool x;

    public bool done;

    [SerializeField] private CinemachineFreeLook cine1;
    [SerializeField] private CinemachineFreeLook cine2;

    private CinemachineBrain cineBrain;

    private XXThirdPersonCam thirdPersonCam;

    private AudioListener listener;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        x = false;
        done = false;
        cineBrain = GetComponent<CinemachineBrain>();
        thirdPersonCam = GetComponent<XXThirdPersonCam>();
        listener = GetComponent<AudioListener>();


        if (cineBrain != null)
        cineBrain.enabled = true;

        if (SceneManager.GetActiveScene().name == ("Main"))
        {
            //Debug.Log("Play Music");
            AudioManager.Instance.PlayMusic("Music 1");
        }
        if (SceneManager.GetActiveScene().name == ("Main 1"))
        {
            AudioManager.Instance.PlayMusic("Music 2");
        }
        if (SceneManager.GetActiveScene().name == ("Main2"))
        {
            AudioManager.Instance.PlayMusic("Music 3");
        }
        if (SceneManager.GetActiveScene().name == ("MainMenu"))
        {
            AudioManager.Instance.PlayMusic("Main Menu");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (player != null && aimPlayer != null)
        {

            listener.enabled = false;
            cine1.Follow = player;
            cine1.LookAt = aimPlayer;
            cine2.Follow = player;
            cine2.LookAt = aimPlayer;

            //Vector3 desiredPos = player.position + offset;
            //Vector3 smoothedPos = Vector3.Lerp(this.transform.position, desiredPos, smoothTime);
            //this.transform.position = smoothedPos;
        }

        if (x && !done)
        {
            cineBrain.enabled = false;
            player = null;
            aimPlayer = null;
            thirdPersonCam.enabled = false;
            listener.enabled = true;

            StartCoroutine(ChangeCam());
            //transform.Rotate(88.989f, 0, 0 , Space.Self);
            
            
        }
    }

    IEnumerator ChangeCam()
    {
        yield return new WaitForSeconds(2f);
        transform.position = new Vector3(7.3f, 63.3f, -1.2f);
        transform.eulerAngles = new Vector3(88.989f, 0, 0);

    }
}
