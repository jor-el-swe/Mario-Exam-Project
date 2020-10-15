using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")] 
    public  UIController uiController;
    public Transform[] walls;
    public Animator playerAnimator;
    
    [Header("Game Behaviour")] 
    public float playerSpeed = 2f;
    public float jumpStrength = 600f;
    public float maxXVelocity = 6f;
    public float maxFallVelocity = -10.5f;
    public float doubleJumpMultiplier = 0.4f;
    public float wallJumpStrength = 600f;
    
    public bool PlayerHasWon => playerHasWon;
    public bool PlayerHasStarted => playerHasStarted;
    public bool PlayerhasReset => playerhasReset;
    public bool PlayerhasDied
    {
        get => playerhasDied;
        set => playerhasDied = value;
    }

    //player components
    private Vector3 _spawnPosition;
    private Rigidbody2D _playerRB;
    
    //player mechanics
    private bool playerIsOnGround = false;
    private int noLives = 3;
    private float lastKnownFallVelocity = 0;
    
    
    //gameplay logic
    private bool playerHasWon = false;
    private bool playerHasStarted = false;
    private bool playerhasReset = true;
    private bool playerhasDied = false;
    private bool canDoubleJump = true;
    
    public void ResetGame()
    {
        _playerRB.velocity = Vector2.zero;
        _playerRB.position = _spawnPosition;
        playerhasReset = true;
        playerHasStarted = false;
        playerhasDied = false;
        playerHasWon = false;
        noLives = 3;
        uiController.SetLifeText(noLives);
    }
    
    
    void Start()
    {
        _spawnPosition = transform.position;
        _playerRB = GetComponent<Rigidbody2D>();
        uiController.SetLifeText(noLives);

        playerAnimator.enabled = false;

    }

    // Update is called once per frame
    private void Update()
    {
        lastKnownFallVelocity = _playerRB.velocity.y;
        //needed for OnTriggerStay2D() to work properly, whilst standing still in a trap
        _playerRB.WakeUp();
        
        if (playerHasStarted && !playerhasDied)
        {
            MovePlayer();
        }
        else
        {
            if (Input.GetKey(KeyCode.P) && playerhasReset)
            {
                playerHasStarted = true;
                playerhasReset = false;
            }

            if (Input.GetKey(KeyCode.R))
            { 
                playerhasReset = true;
            }
        }
    }

    private void MovePlayer()
    {
            //check if we have fallen too far beyond ground level
            if (playerHasStarted)
            {
                if (_playerRB.position.y < -9)
                {
                    playerhasDied = true;
                    return;
                }
            }

            if ( (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && Mathf.Abs(_playerRB.velocity.x) < maxXVelocity)
            {
                _playerRB.AddForce(Vector2.left * playerSpeed );
                playerAnimator.enabled = true;
            }
            else if ( (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) &&  _playerRB.velocity.x < maxXVelocity)
            {
                _playerRB.AddForce(Vector2.right * playerSpeed);
                playerAnimator.enabled = true;
            }
            else
            {
                playerAnimator.enabled = false;
            }

            if (Input.GetKey(KeyCode.F12))
            {
                ActivateEasterEgg();
            }
            
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (playerIsOnGround || canDoubleJump)
                {
                    if (!playerIsOnGround)
                    {
                        canDoubleJump = false;
                        _playerRB.AddForce(Vector2.up * (jumpStrength * doubleJumpMultiplier));
                    }
                    else
                    {
                        _playerRB.AddForce(Vector2.up * jumpStrength);  
                    }
                }
                else
                {
                    //try wall jump
                    foreach(var wall in walls)
                    {
                        if (Mathf.Abs(_playerRB.position.x - wall.position.x) >= 1.2f ||
                            
                            Mathf.Abs(_playerRB.position.y - wall.position.y) > 5f) continue;

                        if (wall.position.x > _playerRB.position.x)
                        {
                            _playerRB.AddForce(Vector2.up * wallJumpStrength + Vector2.left * (wallJumpStrength * 0.5f));
                        }
                        else
                        {
                            _playerRB.AddForce(Vector2.up * wallJumpStrength + Vector2.right * (wallJumpStrength * 0.5f));
                        }
                        
                    }
                }
            }
    }

    private void ActivateEasterEgg()
    {
        Debug.Log("easter egg found!");
        
        MeshRenderer [] playerMeshes = _playerRB.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in playerMeshes)
        {
            mesh.enabled = false;
        }

        playerAnimator.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
        playerAnimator.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (!(lastKnownFallVelocity < maxFallVelocity)) return;
        
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            playerhasDied = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ( (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform")) && 
             collision.otherCollider.name == "FeetCollider")
        {
            playerIsOnGround = true;
            canDoubleJump = true;
        }

        if (collision.gameObject.CompareTag("flag"))
        {
            playerHasWon = true;
            playerHasStarted = false;
        }
        

    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("trap"))
        {
            if (other.GetComponent<Trap>().IsTrapActive())
            {
                noLives--;
                uiController.SetLifeText(noLives);
            }
            
            if (noLives <= 0)
            {
                playerhasDied = true;
                playerHasStarted = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            playerIsOnGround = false;
        }
    }
}
