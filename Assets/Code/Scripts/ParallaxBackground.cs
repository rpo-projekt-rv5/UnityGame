using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
  // private float length;
  // private float start;
  // public GameObject camera;
  // public float effectScore;

  // Use this for initialization
  void Start (){
    // start = transform.position.x;
    // length = GetComponent<SpriteRenderer>().bounds.size.x;
  }

  // Update is called once per frame
  void FixedUpdate (){
  //   float relativeChange = camera.transform.position.x * (1 - effectScore);
  //   float distance = camera.transform.position.x * effectScore;
  //
  //   transform.position = new Vector3(start + distance, transform.position.y, transform.position.z);
  //
  //   if (relativeChange > start + length) start += length;
  //   else if (relativeChange < start + length) start -= length;
  }
}
