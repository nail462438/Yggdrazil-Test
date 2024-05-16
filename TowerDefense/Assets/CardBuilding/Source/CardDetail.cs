using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    R,
    SR,
    SSR,
}

[CreateAssetMenu(fileName = "ExampleCard", menuName = "CardBuilding")]
public class CardDetail : ScriptableObject
{
    public Color Color;
    public Sprite Image;

    [Header("Card Info")]
    public string Name;
    public string Description;
    public string TypeBuilding;
    public int Price;
    public Rarity Rarity;

    [Header("Card Building")]
    public GameObject Building;
}
