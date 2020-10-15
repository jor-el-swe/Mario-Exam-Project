using System;
using UnityEngine;

//an enemy is about the same thing as a trap :)
public class Enemy : Trap
{
    [Header("Enemy Behaviour")] 
    public float enemyMovementSpeed = 2f;


    private Rigidbody2D enemyRB;
    private Transform enemyPlatform;
    private float direction = 1;

    private void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    { 
        AIMovementPattern();
    }

    private void AIMovementPattern()
    {

        float stoppingBoost = 1;
        var platformLength = enemyPlatform.localScale.x;
        var deltaDistance = enemyRB.position.x - enemyPlatform.position.x;
        
        //enemy is to the right of platform center, by fraction the platform length
        if (deltaDistance > platformLength*0.2)
        {
            direction = -1;
        }
        //enemy is to the left of platform center, by fraction the platform length
        else if (deltaDistance < -platformLength*0.2)
        {
            direction = 1;
        }

        if (Math.Sign(direction) != Math.Sign(enemyRB.velocity.x))
        {
            stoppingBoost = 3;
        }
        else
        {
            stoppingBoost = 1;
        }
    

        enemyRB.AddForce(new Vector2(enemyMovementSpeed * direction* stoppingBoost, 0));
        
 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Platform"))
            enemyPlatform = other.gameObject.GetComponent<Transform>();
    }
}
