using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ExampleCard", menuName = "CardBuilding")]
public class CardDetail : ScriptableObject
{
    public Color Color;
    public Sprite Image;
    public string Name;
    public string Description;
    public string TypeBuilding;
    public int Price;
    public GameObject Building;
}
