using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField] Brick brickPrefab;
    [SerializeField] List<Bridge> bridgeList;

    [SerializeField] int width = 10;
    [SerializeField] int length = 5;

    public (int, bool)[,] checkBrick;
    public List<Color> colorList;
    public List<Brick> brickList;

    private List<int> brickCount;

    private void Awake()
    {
        
    }

    private void Start()
    {
        
    }

    private void Update()
    {

    }

    public void OnInit()
    {
        ObjectPoolManager.Instance.OnInit(brickPrefab.gameObject, ObjectPoolManager.Instance.poolObject.transform, 100);

        brickCount = new List<int>();
        brickList = new List<Brick>();
        for (int i = 0; i < colorList.Count; i++)
        {
            brickCount.Add(0);
        }

        checkBrick = new (int, bool)[width, length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                GenerateBrick(x, y);
            }
        }
    }

    public void SetFalseState(int x, int y)
    {
        checkBrick[x, y].Item2 = false;
        brickCount[checkBrick[x, y].Item1]--;
    }

    public Color CloseBridge(Bridge bridge)
    {
        return bridge.CloseBridge();
    }

    public IEnumerator RegenerateBrick()
    {
        while (true)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < length; y++)
                {
                    if (!checkBrick[x, y].Item2)
                    {
                        float timeToWait = Random.Range(0f, 3f);
                        yield return new WaitForSeconds(timeToWait);
                        GenerateBrick(x, y);
                    }
                }
            }
            yield return null;
        }
    }

    private void GenerateBrick(int x, int y)
    {
        Vector3 startInitPos = new Vector3(-9f, 0, -4f);
        Vector3 position = startInitPos + new Vector3(x + 1f * x, 0.6f, y + 1f * y);

        int randomNumber = Random.Range(0, colorList.Count);

        int checkGenerating = 0;
        for (int i = 0; i < brickCount.Count; i++)
        {
            if (brickCount[i] >= (width * length) / brickCount.Count + 1)
            {
                checkGenerating++;
            }
        }
        if (checkGenerating == brickCount.Count)
        {
            return;
        }

        if (brickCount[randomNumber] < (width * length) / brickCount.Count + 1)
        {
            Brick brick = ObjectPoolManager.Instance.GetObjectFromPool().GetComponent<Brick>();
            brick.transform.SetParent(transform);
            brick.transform.localPosition = position;
            brick.transform.localRotation = Quaternion.identity;
            brick.OnInit(colorList[randomNumber]);
            brickCount[randomNumber]++;
            brick.position = new Vector2Int(x, y);
            checkBrick[x, y] = (randomNumber, true);
            brickList.Add(brick);
        }
        else
        {
            GenerateBrick(x, y);
        }
    }
}
