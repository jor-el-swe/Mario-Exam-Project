using System;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [Header("References")]

    [Header("Game Behaviour")] 
    public float playerSpeed = 2f;
    public float jumpStrength = 600f;
    public float maxXVelocity = 6f;
   
    //player components
    private Vector3 _spawnPosition;
    private Rigidbody2D _playerRB;
    
    //player mechanics
    private bool jumpIsPossible = false;
    
    void Start()
    {
        _spawnPosition = transform.position;
        _playerRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {

            if ( (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && Mathf.Abs(_playerRB.velocity.x) < maxXVelocity)
            {
                _playerRB.AddForce(Vector2.left * playerSpeed );
            }
        
            if ( (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) &&  _playerRB.velocity.x < maxXVelocity)
            {
                _playerRB.AddForce(Vector2.right * playerSpeed);
            }
            
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {  if (jumpIsPossible)
                _playerRB.AddForce(Vector2.up * jumpStrength);
            }  
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //this is the straight up from ground/platform jump
        if ( (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform")) && 
             collision.otherCollider.name == "FeetCollider")
        {
            jumpIsPossible = true;
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            jumpIsPossible = false;
        }
    }

}
