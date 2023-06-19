using System;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private void Start()
    {
        lookAt = GameManager.instance.player.transform;
    }

    private Transform lookAt;

    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;
        float deltaX = lookAt.position.x - transform.position.x;
        delta.x = deltaX;
        
        float deltaY = lookAt.position.y - transform.position.y;
        delta.y = deltaY;

        transform.position += delta;
    }
}
