using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      transform.position = new Vector3 (player.position.x + 3, player.position.y + 3, -10); // Camera follows the player with specified offset position
    }
}
