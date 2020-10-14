

#define DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform playerTransform;


    [Header("Game Behaviour")] 
    public float playerSpeed = 2f;
    public float jumpStrength = 30f;
    public float jumpDisableDelay = 0.02f;
    
    //player components
    private Vector3 _spawnPosition;
    private Rigidbody2D _playerRB;
    
    //player mechanics
    private bool jumpIsPossible = false;
    
    void Start()
    {
        _spawnPosition = playerTransform.transform.position;
        _playerRB = playerTransform.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _playerRB.AddForce(Vector2.left * playerSpeed );
            
        }
        
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _playerRB.AddForce(Vector2.right * playerSpeed );
        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            if (jumpIsPossible)
            {
                Debug.Log("jumping");
                _playerRB.AddForce(Vector2.up * jumpStrength);
            }

        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if ( (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform")) && 
             (collision.otherCollider.name == "RFoot") || (collision.otherCollider.name == "LFoot"))
        {
            jumpIsPossible = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        Debug.Log("exit collision");
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            //if disable immediately, the jump will be too low
            Invoke("DisableJump", jumpDisableDelay);
        }
    }

    private void DisableJump()
    {
        jumpIsPossible = false;
    }
}
