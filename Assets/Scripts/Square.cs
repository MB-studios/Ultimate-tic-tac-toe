using UnityEngine;

public class Square : MonoBehaviour
{
    public Game game;
    public int squareNumber;
    public BoxCollider2D boxCollider2D;
    public SpriteRenderer playerSpriteRenderer;

    void Start()
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
