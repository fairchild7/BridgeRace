using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public Vector2Int position;

    public void OnInit(Color color)
    {
        ChangeColor(color);
        SetTrigger(true);
    }

    public Color GetColor()
    {
        return gameObject.GetComponent<MeshRenderer>().material.color;
    }

    public void ChangeColor(Color color)
    {
        gameObject.GetComponent<MeshRenderer>().material.color = color;
    }

    public void SetTrigger(bool state)
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = state;
    }
}
