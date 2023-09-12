using UnityEngine;

public class Square : MonoBehaviour
{
    private Board board;
    private int squareNumber;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer playerSpriteRenderer;

    void OnEnable()
    {
        squareNumber = int.Parse(gameObject.name.Split(' ')[1]);
        board = gameObject.GetComponentInParent<Board>();
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        boxCollider2D.enabled = board.SquareIsActive(squareNumber);
        playerSpriteRenderer.sprite = board.GetPlayerSquareSprite(squareNumber);
    }

    public void OnClick()
    {
        board.PlayerMove(squareNumber);
    }
}
