using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RotateWithMouse))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private Rigidbody _playerRb;

    private void Start()
    {
        _playerRb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Walking();
    }

    private void Walking()
    {
        if (Input.GetKey(KeyCode.W))
        {
            _playerRb.AddForce(transform.forward * speed, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.A))
        {
            _playerRb.AddForce(transform.right * -speed, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _playerRb.AddForce(transform.forward * -speed, ForceMode.Acceleration);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _playerRb.AddForce(transform.right * speed, ForceMode.Acceleration);
        }
    }

    private void Jump()
    {
        if (GroundCheck() && Input.GetKeyDown(KeyCode.Space))
        {
            _playerRb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
        }
    }

    private bool GroundCheck()
    {
        return Physics.Raycast(transform.position, -transform.up, 1.1f);
    }
}
