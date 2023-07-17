using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] GameObject brickPrefab;
    [SerializeField] GameObject entranceGate;
    [SerializeField] GameObject exitGate;


    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        entranceGate.SetActive(false);
    }

    public Color CloseBridge()
    {
        entranceGate.SetActive(true);
        return exitGate.GetComponent<MeshRenderer>().material.color;
    }
}
