using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    private Game game;
    private Image currentPlayer;
    private RectTransform topPanel;
    private RectTransform bottomPanel;
    public int screenWidth;
    public int screenHeight;
    public int barHeight;
    public TextMeshProUGUI player1Wins;
    public TextMeshProUGUI player2Wins;
    public Sprite currentSprite;

    void Start()
    {
        game = GameObject.Find("Game").GetComponent<Game>();
        currentPlayer = GameObject.Find("Current Player").GetComponent<Image>();
        topPanel = gameObject.transform.Find("Top Panel").GetComponent<RectTransform>();
        bottomPanel = gameObject.transform.Find("Bottom Panel").GetComponent<RectTransform>();
        player1Wins = GameObject.Find("Player 1 Wins").GetComponent<TextMeshProUGUI>();
        player2Wins = GameObject.Find("Player 2 Wins").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        barHeight = (Screen.height - Screen.width) / 2;

        topPanel.sizeDelta = new Vector2(screenWidth, barHeight);
        bottomPanel.sizeDelta = new Vector2(screenWidth, barHeight);

        player1Wins.text = game.GetPlayerWins((int)Player.X).ToString();
        player2Wins.text = game.GetPlayerWins((int)Player.O).ToString();

        currentSprite = game.getCurrentPlayerSprite();
        if (currentSprite)
        {
            currentPlayer.enabled = true;
            currentPlayer.sprite = currentSprite;
        }
        else
        {
            currentPlayer.enabled = false;
        }
    }
}
