using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get { return instance; } }
    private static UIManager instance;

    public GameObject timeText;
    public GameObject waveText;
    public GameObject moneyText;
    public GameObject popupText;
    public GameObject enemyTypeText;
    public GameObject hpText;
    public GameObject myDeck;
    public GameObject nameTowerText;
    public GameObject myCommand;
    public GameObject gameOverPanel;
    public GameObject scoreText;

    [Header("Button")]
    public GameObject upgradeButton;
    public GameObject sellButton;
    public Button restartGame;
    public Button DrawButton;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        restartGame.onClick.AddListener(() => RestartGame());
    }

    public void RestartGame()
    {
        //Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void SetText(GameObject text, string message, Color color)
    {
        text.GetComponent<TextMeshProUGUI>().text = message;
        text.GetComponent<TextMeshProUGUI>().color = color;
    }

    public void SetActiveText(GameObject text, bool active)
    {
        text.SetActive(active);
    }

    public void UpdateTextUpgradeButton(string price)
    {
        upgradeButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"UPGRADE\n{price}";
    }

    public void UpdateTextSellButton(string price)
    {
        sellButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"SELL\n{price}";
    }

    public void ShowPopupNotEnouge()
    {
        StartCoroutine(ClosePopupAuto());
    }

    IEnumerator ClosePopupAuto()
    {
        UIManager.Instance.SetActiveText(UIManager.Instance.popupText, true);
        UIManager.Instance.SetText(UIManager.Instance.popupText, $"Money not enought", Color.red);
        yield return new WaitForSeconds(2);
        UIManager.Instance.SetActiveText(UIManager.Instance.popupText, false);
    }
}
