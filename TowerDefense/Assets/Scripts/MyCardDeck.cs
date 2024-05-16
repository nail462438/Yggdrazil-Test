using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyCardDeck : MonoBehaviour
{
    public List<CardDetail> cardDetails;
    public Card myCard;

    [Header("Cost Draw")]
    [SerializeField]
    private int costDraw;

    private int randomPercent;

    // Start is called before the first frame update
    void Start()
    {
        var currentCard = 0;

        while(currentCard < 4)
        {
            DrawCard();

            currentCard++;
        }

        UIManager.Instance.DrawButton.onClick.AddListener(() =>
        {
            if (GameManager.Instance.money >= costDraw)
            {
                GameManager.Instance.money -= costDraw;
                DrawCard();
            }
            else
            {
                UIManager.Instance.ShowPopupNotEnouge();
            }
        });
    }

    public void DrawCard()
    {
        var card = GetCardDetailInRound();

        Debug.Log($"Drawcard : {card.Name}");

        var obj = Instantiate(myCard, UIManager.Instance.myDeck.transform);
        obj.GetComponent<Image>().color = card.Color;
        obj.building = card.Building;
        obj.cost = card.Price;


        obj.transform.GetChild(0).GetComponent<Image>().sprite = card.Image;
        obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = card.Name;
        obj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = card.Description;
        obj.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = card.Price.ToString();
        obj.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = card.TypeBuilding;
    }

    private CardDetail GetCardDetailInRound()
    {
        int[] rarityPercent = { 30, 120};
        randomPercent = Random.Range(1, 101) + Random.Range(1, 101) + Random.Range(1, 101);

        Debug.Log($"Percent Receive : {randomPercent / 3}");

        if (randomPercent <= rarityPercent[0])
        {
            var cardType = cardDetails.FindAll((x) => x.Rarity == Rarity.SSR);
            int randomIndex = Random.Range(0, cardType.Count);
            return cardType[randomIndex];
        }
        else if (randomPercent <= rarityPercent[1])
        {
            var cardType = cardDetails.FindAll((x) => x.Rarity == Rarity.SR);
            int randomIndex = Random.Range(0, cardType.Count);
            return cardType[randomIndex];
        }
        else
        {
            var cardType = cardDetails.FindAll((x) => x.Rarity == Rarity.R);
            int randomIndex = Random.Range(0, cardType.Count);
            return cardType[randomIndex];
        }
    } 
}
