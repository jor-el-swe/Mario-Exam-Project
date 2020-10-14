

#define DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform PlayerTransform;


    [Header("Game Behaviour")] public float PlayerSpeed = 2f;
    
    //player specifics
    private Vector3 _spawnPosition;
    private Rigidbody2D _playerRB;
    
    void Start()
    {
        _spawnPosition = PlayerTransform.transform.position;
        _playerRB = PlayerTransform.GetComponent<Rigidbody2D>();
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
            Debug.Log("moving left");
            _playerRB.AddForce(Vector2.left * PlayerSpeed );
            
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("moving right");
            _playerRB.AddForce(Vector2.right * PlayerSpeed );
        }
    }
}
