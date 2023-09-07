using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    //PlayersHead
    [SerializeField] private GameObject head;
    [SerializeField] private float mouseSensitivity;
    internal Vector2 turn;

    private void Update()
    {
        RotatePlayerWithMouse();
    }

    private void RotatePlayerWithMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        turn.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        turn.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
        turn.y = Mathf.Clamp(turn.y, -55, 60);

        transform.localRotation = Quaternion.Euler(0, turn.x, 0);
        head.transform.localRotation = Quaternion.Euler(-turn.y, 0, 0);
        
    }
}
