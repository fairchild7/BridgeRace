using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] LayerMask bridgeLayer;
    [SerializeField] protected Rigidbody rb;

    protected int brickCount;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckBridge());  
    }

    public virtual void OnInit()
    {
        
    }

    public void ChangeAnimation(string animationName)
    {
        RuntimeAnimatorController newController = Resources.Load<RuntimeAnimatorController>("AnimationControllers/" + animationName);
        animator.runtimeAnimatorController = newController;
    }

    public Color GetColor()
    {
        //return gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
        return gameObject.GetComponent<MeshRenderer>().material.color;
    }

    public int GetBrickCount()
    {
        return this.brickCount;
    }

    protected void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Brick"))
        {
            GameObject brick = collider.gameObject;
            if (brick.GetComponent<Brick>().GetColor() == GetColor())
            {
                Vector3 addPosition = new Vector3(0f, 1f + brickCount * 0.2f, -0.6f);
                if (brick.GetComponentInParent<Floor>() == null)
                {
                    Debug.Log("null");
                }
                else
                {
                    brick.GetComponentInParent<Floor>().SetFalseState(brick.GetComponent<Brick>().position.x, brick.GetComponent<Brick>().position.y);
                }
                brick.transform.rotation = transform.rotation;
                brick.transform.SetParent(transform);
                brick.transform.localPosition = addPosition;
                brickCount++;
            }
        }

        if (collider.CompareTag("Gate"))
        {
            collider.gameObject.GetComponent<MeshRenderer>().material.color = GetColor();
            LevelManager.Instance.CloseBridge(collider.gameObject.GetComponentInParent<Bridge>());
        }
    }

    protected void OnTriggerExit(Collider collider)
    {

    }

    protected IEnumerator CheckBridge()
    {
        while (true)
        {
            CheckBlocked();

            Vector3 raycastPos = transform.position;
            raycastPos.y += 10f;
            RaycastHit hit;
            if (Physics.Raycast(raycastPos, Vector3.down, out hit, 50f, bridgeLayer))
            {
                Color bridgeColor = hit.collider.gameObject.GetComponent<MeshRenderer>().material.color;
                if (brickCount > 0 && bridgeColor != GetColor())
                {
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material.color = GetColor();
                    DropBrick();
                }
                /*
                else if (brickCount <= 0 && bridgeColor == Color.white)
                {
                    Debug.Log("Game Over");
                }
                */
            }
            yield return null;
        }
    }

    protected void CheckBlocked()
    {
        Vector3 raycastPos = transform.position;
        raycastPos.y += 10f;
        raycastPos.z += 0.75f;
        RaycastHit hit;
        if (Physics.Raycast(raycastPos, Vector3.down, out hit, 50f, bridgeLayer))
        {
            if (brickCount > 0)
            {
                hit.collider.gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
            }
        }
    }

    protected void DropBrick()
    {
        Transform bottomBrick = GetTopBrick();
        Destroy(bottomBrick.gameObject);
        brickCount--;
    }

    protected Transform GetTopBrick()
    {
        Transform bottomBrick = null;

        Transform[] bricks = transform.GetComponentsInChildren<Transform>();
        foreach (Transform brick in bricks)
        {
            if (brick.CompareTag("Brick"))
            {
                if (bottomBrick == null || brick.position.y > bottomBrick.position.y)
                {
                    bottomBrick = brick;
                }
            }
        }
        return bottomBrick;
    }
}
