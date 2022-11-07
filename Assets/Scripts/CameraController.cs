using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float camera_speed = 6;
    public Vector3 camera_velocity;

    private void Update()
    {
        if(FindObjectOfType<PlayerController>().can_move)
        {
            transform.position += Vector3.forward * camera_speed * Time.deltaTime;
        }
        
        camera_velocity = Vector3.forward * camera_speed * Time.deltaTime;
    }
}
