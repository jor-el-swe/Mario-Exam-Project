using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]

    [Header("Game Behaviour")] 
    public float playerSpeed = 2f;
    public float jumpStrength = 600f;
    public float maxXVelocity = 6f;
    public static bool PlayerHasWon => playerHasWon;
    public static bool PlayerHasStarted => playerHasStarted;

    public static bool PlayerhasReset => playerhasReset;

    //player components
    private Vector3 _spawnPosition;
    private Rigidbody2D _playerRB;
    
    //player mechanics
    private bool jumpIsPossible = false;
    
    
    //gameplay logic
    private static bool playerHasWon = false;
    private static bool playerHasStarted = false;
    private static bool playerhasReset = true;
    
    public void ResetGame()
    {
        _playerRB.position = _spawnPosition;
    }
    
    
    void Start()
    {
        _spawnPosition = transform.position;
        _playerRB = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (playerHasStarted)
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
                playerhasReset = true;
            }
        }
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

        if (collision.gameObject.CompareTag("flag"))
        {
            Debug.Log("winner!");
            playerHasWon = true;
            playerHasStarted = false;
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
