using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPlane : MonoBehaviour
{
    public Material select;
    public Material unselect;
    public int indexBox = 0;
    public bool busy = false;

    private void OnMouseEnter()
    {
        GetComponent<Renderer>().material = select;
        GameManager.Instance.indexBox = indexBox;
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material = unselect;
        GameManager.Instance.showModel = false;
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.holdCard && !busy)
        {
            GameManager.Instance.showModel = true;
        }
    }
}
