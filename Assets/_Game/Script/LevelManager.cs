using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
            }

            return instance;
        }
    }

    [SerializeField] GameObject player;

    [SerializeField] int numberOfPlayers = 4;

    private string[,] mapType;

    private GameObject startPoint;
    private GameObject finishBox;
    private GameObject map;

    public List<Color> colorList;
    public List<Floor> floorList;

    private void Awake()
    {
        colorList = new List<Color>() { Color.red, Color.blue, Color.green, Color.yellow };
    }

    void Start()
    {
        for (int i = 0; i < colorList.Count; i++)
        {
            floorList[0].colorList.Add(colorList[i]);
        }
        floorList[0].OnInit();
        StartCoroutine(floorList[0].RegenerateBrick());
        colorList.Clear();
    }

    void Update()
    {
        
    }

    public void CloseBridge(Bridge bridge)
    {
        Debug.Log("Closing");
        Floor currentFloor = bridge.GetComponentInParent<Floor>();
        colorList.Add(currentFloor.CloseBridge(bridge));
        int nextFloor = floorList.IndexOf(currentFloor);
        if (nextFloor < floorList.Count - 1)
        {
            nextFloor++;
            for (int i = 0; i < colorList.Count; i++)
            {
                floorList[nextFloor].colorList.Add(colorList[i]);
            }
            Debug.Log(floorList[nextFloor].colorList.Count);
            floorList[nextFloor].OnInit();
        }
        else
        {
            Debug.Log("Congrats!");
        }
    }
}
