using UnityEngine;

public class Board : MonoBehaviour
{
    private Game game;
    private int boardNumber;
    private SpriteRenderer backgroundSpriteRenderer;
    private SpriteRenderer playerSpriteRenderer;

    void OnEnable()
    {
        boardNumber = int.Parse(gameObject.name.Split(' ')[1]);
        game = gameObject.GetComponentInParent<Game>();
        backgroundSpriteRenderer = gameObject.transform.Find("Background").GetComponent<SpriteRenderer>();
        playerSpriteRenderer = gameObject.transform.Find("Player").GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        backgroundSpriteRenderer.sprite = game.GetBoardBackgroundSprite(boardNumber);
        playerSpriteRenderer.sprite = game.GetPlayerBoardSprite(boardNumber);
    }

    public Sprite GetPlayerSquareSprite(int squareNumber)
    {
        return game.GetPlayerSquareSprite(boardNumber, squareNumber);
    }

    public bool SquareIsActive(int squareNumber)
    {
        return game.BoardIsActive(boardNumber) && game.SquareIsActive(boardNumber, squareNumber);
    }

    public void PlayerMove(int squareNumber)
    {
        game.PlayerMove(boardNumber, squareNumber);
    }
}
