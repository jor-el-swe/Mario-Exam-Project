
using UnityEngine;

public class BetterJump : MonoBehaviour
{

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    private Rigidbody2D playerRb;


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (playerRb.velocity.y <0)
        {
            playerRb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        }
        else if (playerRb.velocity.y > 0 && !(Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) )
        {
            playerRb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime); 
        }
        
    }
}
