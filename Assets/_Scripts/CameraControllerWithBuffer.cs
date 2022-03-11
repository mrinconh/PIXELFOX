using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerWithBuffer : MonoBehaviour
{
    [SerializeField] private Transform player;
    [Range(1f, 10f)][SerializeField] private float cameraOffsetX = 5f;
    [Range(1f, 10f)][SerializeField] private float cameraOffsetY = 5f;



    void Update()//for rectangular buffer 
    {
        //check the x threshold
        if (player.position.x < transform.position.x - (0.5f * cameraOffsetX)) //left
        {
            transform.position = new Vector3(
                player.position.x + (0.5f * cameraOffsetX),
                transform.position.y,
                transform.position.z);
        }
        else if (player.position.x > transform.position.x + (0.5f * cameraOffsetX)) //right
        {
            transform.position = new Vector3(
                player.position.x - (0.5f * cameraOffsetX),
                transform.position.y,
                transform.position.z);
        }


        //check the y threshold
        if (player.position.y < transform.position.y - (0.5f * cameraOffsetY)) //up
        {
            transform.position = new Vector3(
                transform.position.x,
                player.position.y + (0.5f * cameraOffsetY),
                transform.position.z);
        }
        else if (player.position.y > transform.position.y + (0.5f * cameraOffsetY)) //down
        {
            transform.position = new Vector3(
                transform.position.x,
                player.position.y - (0.5f * cameraOffsetY),
                transform.position.z);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(cameraOffsetX,cameraOffsetY,0.0f));
    }
}
