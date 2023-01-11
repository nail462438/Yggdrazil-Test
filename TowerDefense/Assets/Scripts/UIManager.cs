using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get { return instance; } }
    private static UIManager instance;

    public GameObject timeText;
    public GameObject waveText;

    private void Awake()
    {
        instance = this;
    }

    public void SetTimeText(string text, Color color)
    {
        timeText.GetComponent<TextMeshProUGUI>().text = text;
        timeText.GetComponent<TextMeshProUGUI>().color = color;
    }

    public void SetWaveText(string text, Color color)
    {
        waveText.GetComponent<TextMeshProUGUI>().text = text;
        waveText.GetComponent<TextMeshProUGUI>().color = color;
    }

    public void SetActiveText(bool active)
    {
        timeText.SetActive(active);
    }
}
