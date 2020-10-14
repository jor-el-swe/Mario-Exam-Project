using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]

    [Header("Game Behaviour")] 
    public float playerSpeed = 2f;
    public float jumpStrength = 600f;
    public float maxXVelocity = 6f;
    public float maxFallVelocity = -10f;
    
    public bool PlayerHasWon => playerHasWon;
    public bool PlayerHasStarted => playerHasStarted;
    public bool PlayerhasReset => playerhasReset;
    public bool PlayerhasDied => playerhasDied;

    //player components
    private Vector3 _spawnPosition;
    private Rigidbody2D _playerRB;
    
    //player mechanics
    private bool playerIsOnGround = false;
    
    
    //gameplay logic
    private bool playerHasWon = false;
    private bool playerHasStarted = false;
    private bool playerhasReset = true;
    private bool playerhasDied = false;
    
    public void ResetGame()
    {
        _playerRB.velocity = Vector2.zero;
        _playerRB.position = _spawnPosition;
        playerhasReset = true;
        playerHasStarted = false;
        playerhasDied = false;
    }
    
    
    void Start()
    {
        _spawnPosition = transform.position;
        _playerRB = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerHasStarted && !playerhasDied)
        {
            MovePlayer();
            playerhasReset = false;
            playerHasWon = false;
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
                ResetGame();
            }
        }
    }

    private void MovePlayer()
    {
            //check if we have fallen too far
            if (!playerIsOnGround && playerHasStarted)
            {
                if (_playerRB.velocity.y < maxFallVelocity)
                {
                    playerhasDied = true;
                    return;
                }
            }

            if ( (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && Mathf.Abs(_playerRB.velocity.x) < maxXVelocity)
            {
                _playerRB.AddForce(Vector2.left * playerSpeed );
            }
        
            if ( (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) &&  _playerRB.velocity.x < maxXVelocity)
            {
                _playerRB.AddForce(Vector2.right * playerSpeed);
            }
            
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {  if (playerIsOnGround)
                _playerRB.AddForce(Vector2.up * jumpStrength);
            }  
        
    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        //this is the straight up from ground/platform jump
        if ( (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform")) && 
             collision.otherCollider.name == "FeetCollider")
        {
            playerIsOnGround = true;
        }

        if (collision.gameObject.CompareTag("flag"))
        {
            playerHasWon = true;
            playerHasStarted = false;
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
