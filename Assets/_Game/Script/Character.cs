using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] LayerMask bridgeLayer;
    [SerializeField] protected Rigidbody rb;

    protected int brickCount;
    protected int currentFloor;
    protected bool onBridge;
    protected bool collectable;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        StartCoroutine(CheckBridge());
    }

    public virtual void OnInit()
    {
        currentFloor = 0;
        collectable = true;
        onBridge = false;
    }

    public void ChangeAnimation(string animationName)
    {
        RuntimeAnimatorController newController = Resources.Load<RuntimeAnimatorController>("AnimationControllers/" + animationName);
        animator.runtimeAnimatorController = newController;
    }

    public Color GetColor()
    {
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
            if (collectable)
            {
                GameObject brick = collider.gameObject;
                if (brick.GetComponent<Brick>().GetColor() == GetColor() || brick.GetComponent<Brick>().GetColor() == Color.gray)
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
                    brick.GetComponent<MeshRenderer>().material.color = GetColor();
                    brickCount++;
                }
            }
        }

        if (collider.CompareTag("Gate"))
        {
            collider.gameObject.GetComponent<MeshRenderer>().material.color = GetColor();
            collider.gameObject.tag = "Untagged";
            LevelManager.Instance.CloseBridge(collider.gameObject.GetComponentInParent<Bridge>());
            currentFloor++;
            if (transform.GetComponent<Enemy>() != null)
            {
                transform.GetComponent<Enemy>().ChangeState(new PatrolState());
            }
        }

        if (collider.CompareTag("Finish"))
        {
            BR_UIManager.Instance.gamePlay.Close();
            if (GetComponent<Enemy>() != null)
            {
                BR_UIManager.Instance.lose.Open();
            }
            else if (GetComponent<Player>() != null)
            {
                BR_UIManager.Instance.win.Open();
            }
            GameControl.Instance.End();
        }
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
            }
            yield return null;
        }
    }

    protected void CheckBlocked()
    {
        Vector3 raycastPos = transform.position + rb.velocity * 0.2f;
        raycastPos.y += 10f;
        if (transform.GetComponent<Enemy>() != null)
        {
            raycastPos.z += 0.75f;
        }
        RaycastHit hit;
        Debug.DrawRay(raycastPos, Vector3.down * 50f);
        if (Physics.Raycast(raycastPos, Vector3.down, out hit, 50f, bridgeLayer))
        {
            onBridge = true;
            if (brickCount > 0)
            {
                hit.collider.gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 1f, 1f);
            }
            else if (brickCount == 0 && hit.collider.gameObject.GetComponent<MeshRenderer>().material.color != GetColor())
            {
                hit.collider.gameObject.GetComponent<BoxCollider>().size = new Vector3(1f, 10f, 1f);
            }
        }
        else
        {
            onBridge = false;
        }
    }

    protected void DropBrick()
    {
        Transform bottomBrick = GetTopBrick();
        ObjectPoolManager.Instance.ReturnObjectToPool(bottomBrick.gameObject);
        bottomBrick.SetParent(ObjectPoolManager.Instance.poolObject.transform);
        brickCount--;
    }

    protected void Fall(Vector3 fallPosition)
    {
        Transform[] bricks = transform.GetComponentsInChildren<Transform>();
        foreach (Transform brick in bricks)
        {
            if (brick.CompareTag("Brick"))
            {
                brick.transform.position = Vector3.Lerp(brick.transform.position,
                    new Vector3(fallPosition.x + Random.Range(-3f, 3f), 0.6f, fallPosition.z + Random.Range(-3f, 3f)), 1f);
                brick.transform.eulerAngles = new Vector3(0f, Random.Range(0, 360f), 0f);
                brick.SetParent(LevelManager.Instance.floorList[currentFloor].transform);
                brick.transform.GetComponent<MeshRenderer>().material.color = Color.gray;
                LevelManager.Instance.floorList[currentFloor].brickList.Add(brick.transform.GetComponent<Brick>());
                brickCount = 0;
            }
        }
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

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (!onBridge)
            {
                if (GetBrickCount() < collision.collider.GetComponent<Character>().GetBrickCount() && collectable)
                {
                    Fall(transform.position);
                    StartCoroutine(PlayerFall());
                    Debug.Log(name + " fall");
                }
            }
            else if (onBridge)
            {
                Physics.IgnoreCollision(transform.GetComponent<CapsuleCollider>(), collision.collider, true);
            }
        }
    }

    protected IEnumerator PlayerFall()
    {
        transform.eulerAngles = new Vector3(90f, 0f, 0f);
        collectable = false;
        if (transform.GetComponent<Enemy>() != null)
        {
            transform.GetComponent<NavMeshAgent>().speed = 0f;
        }
        yield return new WaitForSeconds(2f);
        transform.eulerAngles = new Vector3(0f, 0f, 0f);
        collectable = true;
        if (transform.GetComponent<Enemy>() != null)
        {
            transform.GetComponent<NavMeshAgent>().speed = 3.5f;
        }
    }
}
