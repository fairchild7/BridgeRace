using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private string[,] mapType;


    [SerializeField] GameObject startPoint;
    [SerializeField] GameObject finishBox;
    private GameObject map;

    public List<Color> colorList;
    public List<Floor> floorList;
    public List<Enemy> enemies;

    private void Awake()
    {
        colorList = new List<Color>() { Color.red, Color.blue, Color.green, Color.yellow };
    }

    void Start()
    {
        
    }

    void Update()
    {
        foreach (Enemy e in enemies)
        {
            if (e.currentState != null)
            {
                e.currentState.OnExecute(e);
            }
        }
    }

    public void StartLevel()
    {
        for (int i = 0; i < colorList.Count; i++)
        {
            floorList[0].colorList.Add(colorList[i]);
        }
        floorList[0].OnInit();
        StartCoroutine(floorList[0].RegenerateBrick());
        colorList.Clear();
        foreach (Enemy e in enemies)
        {
            e.OnInit();
        }
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
            floorList[nextFloor].OnInit();
        }
        else
        {
            Debug.Log("Congrats!");
        }
    }

    public void StopAllEnemies()
    {
        foreach (Enemy e in enemies)
        {
            e.agent.isStopped = true;
        }
    }
}
