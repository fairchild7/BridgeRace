using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public NavMeshAgent agent;
    public Color color;
    public IState currentState;

    private Vector3 destination;

    private void Awake()
    {
       
    }

    protected override void Start()
    {
        base.Start(); 
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(currentState);
            Debug.Log(destination);
        }
    }

    public override void OnInit()
    {
        base.OnInit();
        gameObject.GetComponent<MeshRenderer>().material.color = color;
        ChangeState(new PatrolState());
    }

    public void SetDestination(Vector3 target)
    {
        this.destination = target;
    }

    public Vector3 GetDestination()
    {
        return this.destination;
    }
    
    public float GetDistance(Vector3 target)
    {
        return Mathf.Sqrt(Mathf.Pow(Mathf.Abs(transform.position.x - target.x), 2f) + Mathf.Pow(Mathf.Abs(transform.position.z - target.z), 2f));
    }

    public void ChangeState(IState newState)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = newState;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }

    public void MoveToBrick()
    {
        Vector3 target = FindBrick();
        if (target == Vector3.down)
        {
            //MoveToBrick();
        }
        else
        {
            SetDestination(target);
            agent.SetDestination(target);
        }
    }

    private Vector3 FindBrick()
    {
        List<Brick> brickList = LevelManager.Instance.floorList[currentFloor].brickList;
        foreach (Brick brick in brickList)
        {
            if (brick.GetColor() == color || brick.GetColor() == Color.gray)
            {
                brickList.Remove(brick);
                return brick.transform.position;
            }
        }
        return Vector3.down;
    }

    public void FindBridge()
    {
        GameObject box = GameObject.Find("FinishBox");
        agent.SetDestination(box.transform.position);
    }
}
