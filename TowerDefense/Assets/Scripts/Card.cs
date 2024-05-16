using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject building;
    public int cost;
    private GameObject clone;

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Click");
        var obj = Instantiate(gameObject);
        GetComponent<CanvasGroup>().alpha = 0.5f;
        clone = obj;
        clone.GetComponent<CanvasGroup>().alpha = 0.75f;
        clone.transform.SetParent(transform.parent.parent.parent.parent);
        clone.transform.position = eventData.position;
        GameManager.Instance.holdCard = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.Instance.showModel && GameManager.Instance.money >= cost && !GameManager.Instance.boxPlane[GameManager.Instance.indexBox].GetComponent<BoxPlane>().busy)
        {
            var obj = Instantiate(building, GameManager.Instance.boxPlane[GameManager.Instance.indexBox]);
            obj.transform.localPosition = new Vector3(0, 1.05f, 0);
            obj.transform.localRotation = Quaternion.Euler(0, 180, 0);
            obj.transform.GetComponent<CharacterBase>().basePrice = cost;
            obj.transform.GetComponent<CharacterBase>().nameTower = transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
            GameManager.Instance.boxPlane[GameManager.Instance.indexBox].GetComponent<BoxPlane>().busy = true;

            GameManager.Instance.money -= cost;
            Destroy(gameObject);
        }
        else if (GameManager.Instance.money < cost)
        {
            StartCoroutine(ClosePopupAuto());
        }

        Destroy(clone);
        GetComponent<CanvasGroup>().alpha = 1f;
        GameManager.Instance.holdCard = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        clone.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        clone.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }

    IEnumerator ClosePopupAuto()
    {
        UIManager.Instance.SetActiveText(UIManager.Instance.popupText, true);
        UIManager.Instance.SetText(UIManager.Instance.popupText, $"Money not enought", Color.red);
        yield return new WaitForSeconds(2);
        UIManager.Instance.SetActiveText(UIManager.Instance.popupText, false);
    }
}
