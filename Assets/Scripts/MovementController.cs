using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    public PlayerController controller;

    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;

    [SerializeField] GameController gameState;

   

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.name == "skull(Clone)")
        {
            gameState.SetGameState(0);
        }
    }
}
