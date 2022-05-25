using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.InputSystem;

public class MovementController : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    public float maxSpeed;
    private float horizontalInput;
    private float verticalInput;
    public float timeToMaxSpeed;
    private float velocityX;
    private float velocityY;
    private void Update()
    {
        float acceleration = maxSpeed / timeToMaxSpeed;
        float h = acceleration * horizontalInput;
        float v = acceleration * verticalInput;

        if(horizontalInput != 0)
            velocityX += h * Time.deltaTime; 
        else
            velocityX -= Mathf.Clamp(-acceleration * Time.deltaTime, velocityX, 0);

        if(verticalInput != 0) 
            velocityY += v * Time.deltaTime;
        else 
            velocityY -= Mathf.Clamp(-acceleration * Time.deltaTime, velocityY, 0);

        velocityY = Mathf.Clamp(velocityY, -maxSpeed, maxSpeed);
        velocityX = Mathf.Clamp(velocityX, -maxSpeed, maxSpeed);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), Mathf.Clamp(rb.velocity.y, -maxSpeed, maxSpeed));
        
    }

    private void FixedUpdate() => rb.velocity = new Vector2(velocityX, velocityY);
    public void MoveHorizontal(CallbackContext context) => horizontalInput = context.ReadValue<float>();
    public void MoveVertical(CallbackContext context) => verticalInput = context.ReadValue<float>();
    public void SetVelocityToZero() => rb.velocity = Vector2.zero;
}   