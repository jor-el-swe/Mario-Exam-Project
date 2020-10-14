using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")] 
    public Transform playerTransform;

    [Header("Camera Settings")] 
    public float cameraYOffset = 4f;
  
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newPosition = transform.position;
        
        //vertical movement
        if ((int)playerTransform.position.y != (int)transform.position.y)
        {
            newPosition.y = Mathf.Lerp(transform.position.y, playerTransform.position.y, Time.deltaTime);
           
            //cannot see below ground
            newPosition.y = Mathf.Clamp(newPosition.y ,0 , newPosition.y );
            
            transform.position = newPosition;
        }
        
        //horizontal movement
        if ((int)playerTransform.position.x != (int)transform.position.x)
        {
            newPosition.x = Mathf.Lerp(transform.position.x, playerTransform.position.x, Time.deltaTime);
            transform.position = newPosition;
        }
    }
}