using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] GameObject brickPrefab;
    [SerializeField] GameObject entranceGate;
    [SerializeField] GameObject exitGate;

    [SerializeField] private int length = 15;

    public List<GameObject> brickList; 

    private void Start()
    {
        OnInit();
    }

    public void OnInit()
    {
        entranceGate.SetActive(false);

        brickList = new List<GameObject>();

        Vector3 startPosition = new Vector3(0f, 0.4f, 0.25f);
        for (int i = 0; i < length; i++)
        {
            Vector3 position = startPosition + new Vector3(0, 0.2f, 0.5f) * i;
            GameObject brick = Instantiate(brickPrefab, transform.position, Quaternion.identity, gameObject.transform);
            brick.transform.localPosition = position;
            brickList.Add(brick);
        }
    }

    public Color CloseBridge()
    {
        entranceGate.SetActive(true);
        return exitGate.GetComponent<MeshRenderer>().material.color;
    }
}
