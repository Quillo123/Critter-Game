using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public PlayerController player;

    private void Start()
    {
        Camera.SetupCurrent(GetComponent<Camera>());
    }

    void Update()
    {
        if(player != null)
            transform.position = player.transform.position.SetZ(-10);
    }
}
