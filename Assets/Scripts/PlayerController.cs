using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")] 
    public  UIController uiController;
    public Transform[] walls;
    public Animator playerAnimator;
    
    [Header("Game Behaviour")] 
    public float playerSpeed = 10f;
    public float jumpStrength = 300f;
    public float maxXVelocity = 8f;
    public float maxFallVelocity = -10.5f;
    public float doubleJumpMultiplier = 0.4f;
    public float wallJumpStrength = 250f;
    
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
    private PlayerMovements playerMovements = new PlayerMovements();
    private bool playerFacingLeft;
  
    
    //gameplay logic
    private bool playerHasWon;
    private bool playerHasStarted;
    private bool playerhasReset = true;
    private bool playerhasDied;
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
            //check if we have fallen too far beyond ground level
            if (_playerRB.position.y < -9)
            {
                playerhasDied = true;
                return;
            }
            PrepareMovePlayer();
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
    
    private void PrepareMovePlayer()
    {
            if ( (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && Mathf.Abs(_playerRB.velocity.x) < maxXVelocity)
            {
                playerMovements.MoveLeft = true;
            }
            else if ( (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) &&  _playerRB.velocity.x < maxXVelocity)
            {
                playerMovements.MoveRight = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                if (playerIsOnGround || canDoubleJump)
                {
                    if (!playerIsOnGround)
                    {
                        canDoubleJump = false;
                        playerMovements.DoubleJump = true;
                    }
                    else
                    {
                        playerMovements.Jump = true;
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
                            playerMovements.WallJumpLeft = true;
                        }
                        else
                        {
                            playerMovements.WallJumpRight = true;
                        }
                        
                    }
                }
            }
            
            if (Input.GetKey(KeyCode.I))
            {
                ActivateEasterEgg(true);
            }

            if (Input.GetKey(KeyCode.U))
            {
                ActivateEasterEgg(false);
            }
    }

    private void FixedUpdate()
    {
        if(playerMovements.MoveLeft)
            _playerRB.AddForce(Vector2.left * playerSpeed);

        if (playerMovements.MoveRight)
            _playerRB.AddForce(Vector2.right * playerSpeed);
            
        if(playerMovements.DoubleJump)
            _playerRB.AddForce(Vector2.up * (jumpStrength * doubleJumpMultiplier));
        
        if(playerMovements.Jump)
            _playerRB.AddForce(Vector2.up * jumpStrength);  
        
        if(playerMovements.WallJumpLeft)
            _playerRB.AddForce(Vector2.up * wallJumpStrength + Vector2.left * (wallJumpStrength * 0.5f));
        
        if(playerMovements.WallJumpRight)
            _playerRB.AddForce(Vector2.up * wallJumpStrength + Vector2.right * (wallJumpStrength * 0.5f));

        //only animate if player is moving, and not is dead or has won
        if (!(playerhasDied || playerHasWon))
            playerAnimator.enabled = _playerRB.velocity.x != 0;
        
        //flip animation in x depending on movement direction
        if (_playerRB.velocity.x < 0f)
        {
            playerFacingLeft = true;
            FlipPLayer();
        }

        if (_playerRB.velocity.x > 0f)
        {
            playerFacingLeft = false;
            FlipPLayer();
        }
        
        
        //reset all movements
        playerMovements.Jump = false;
        playerMovements.MoveLeft = false;
        playerMovements.MoveRight = false;
        playerMovements.DoubleJump = false;
        playerMovements.WallJumpLeft = false;
        playerMovements.WallJumpRight = false;
    }

    private void FlipPLayer()
    {
        if (playerhasDied || playerHasWon) return;
        
        if (playerFacingLeft)
        {
            var transform1 = transform;
            Vector3 theScale = transform1.localScale;
            theScale.x = -1;
            transform1.localScale = theScale; 
        }
        else
        {
            var transform1 = transform;
            Vector3 theScale = transform1.localScale;
            theScale.x = 1;
            transform1.localScale = theScale; 
        }
    }

    private void ActivateEasterEgg(bool toggle)
    {
        MeshRenderer [] playerMeshes = _playerRB.gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in playerMeshes)
        {
            if(mesh.gameObject.name !="FeetCollider")
                mesh.enabled = !toggle;
        }

        playerAnimator.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = toggle;
        playerAnimator.enabled = toggle;

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
