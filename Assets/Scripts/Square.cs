using UnityEngine;

public class Square : MonoBehaviour
{
    private Game game;
    private int squareNumber;
    private BoxCollider2D boxCollider2D;
    private SpriteRenderer playerSpriteRenderer;

    void OnEnable()
    {
        game = gameObject.GetComponentInParent<Game>();
        squareNumber = int.Parse(gameObject.name.Split(' ')[1]);
        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        boxCollider2D.enabled = game.SquareIsActive(squareNumber);
        playerSpriteRenderer.sprite = game.GetPlayerSquareSprite(squareNumber);
    }

    public void OnClick()
    {
        game.PlayerMove(squareNumber);
    }
}
