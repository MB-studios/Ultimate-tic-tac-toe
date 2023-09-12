using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Input.mousePosition));
            if (!rayHit.collider) return;

            Square square = rayHit.collider.gameObject.GetComponent<Square>();
            square.OnClick();
        }
    }
}
