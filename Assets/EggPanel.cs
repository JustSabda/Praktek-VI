using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggPanel : MonoBehaviour
{
    public static EggPanel Instance { get; private set; }

    public GameObject egg;
    public GameObject eggless;
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
