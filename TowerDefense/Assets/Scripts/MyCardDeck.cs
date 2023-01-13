using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyCardDeck : MonoBehaviour
{
    public List<CardDetail> cardDetails;
    public GameObject myCard;
    private int randomIndex;

    // Start is called before the first frame update
    void Start()
    {
        var currentCard = 0;

        while(currentCard < 4)
        {
            DrawCard();

            currentCard++;
        }
    }

    public void DrawCard()
    {
        randomIndex = Random.Range(0, cardDetails.Count);
        var obj = Instantiate(myCard, UIManager.Instance.myDeck.transform);
        obj.transform.GetComponent<Image>().color = cardDetails[randomIndex].Color;
        obj.transform.GetComponent<Card>().building = cardDetails[randomIndex].Building;
        obj.transform.GetComponent<Card>().cost = cardDetails[randomIndex].Price;
        obj.transform.GetChild(0).GetComponent<Image>().sprite = cardDetails[randomIndex].Image;
        obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = cardDetails[randomIndex].Name;
        obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = cardDetails[randomIndex].Description;
        obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = cardDetails[randomIndex].Price.ToString();
        obj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = cardDetails[randomIndex].TypeBuilding;
    }
}
