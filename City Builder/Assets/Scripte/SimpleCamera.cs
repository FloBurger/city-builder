using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCamera : MonoBehaviour
{
    public float speed = 20f;
    public float scrollSpeed = 20f;
    public Vector2 panLimit;
    Camera cam;
    public float minY = 10;
    public float maxY = 100;
   void Start() {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if(Input.GetKey("w")){
            pos.z += speed * Time.deltaTime;
        }
        if(Input.GetKey("s")){
            pos.z -= speed * Time.deltaTime;
        }
        if(Input.GetKey("a")){
            pos.x -= speed * Time.deltaTime;
        }
        if(Input.GetKey("d")){
            pos.x += speed * Time.deltaTime;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= scroll * scrollSpeed * 100f * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, -panLimit.x, panLimit.x);
        pos.y = Mathf.Clamp(pos.y, minY,maxY);
        pos.z = Mathf.Clamp(pos.z, -panLimit.y, panLimit.y);

        transform.position = pos;
    }
}
