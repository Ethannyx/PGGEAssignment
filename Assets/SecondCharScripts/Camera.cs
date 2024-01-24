using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform player; // The player's transform to follow
    public float distance = 5f; // Distance from the player
    public float height = 2f; // Height above the player's head
    public float rotationSpeed = 5f; // Camera rotation speed

    void Update()
    {
        if (player != null)
        {
            // Calculate the new position based on player's position, height, and distance
            Vector3 playerPosition = player.position - player.forward * distance + Vector3.up * height;

            // Update the camera's position without interpolation for immediate response
            transform.position = playerPosition;

            // Rotate the camera to look at the back of the player's head
            Quaternion desiredRotation = Quaternion.LookRotation(player.position - transform.position);
            
            // Update the camera's rotation directly based on the player's rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
