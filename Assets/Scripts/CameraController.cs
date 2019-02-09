using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Tilemap map;
    private Vector3 bottomLeftLimit, topRightLimit;
    private float halfHeight, halfWidth;

    // Start is called before the first frame update
    void Start()
    {
        //target = PlayerController.instance.transform; //due to execution orfer looks for instance before is instantiated
        target = FindObjectOfType<PlayerController>().transform;

        halfHeight = Camera.main.orthographicSize; // camera height
        halfWidth = halfHeight * Camera.main.aspect;

        bottomLeftLimit = map.localBounds.min + new Vector3(halfWidth, halfHeight, 0);
        topRightLimit = map.localBounds.max - new Vector3(halfWidth, halfHeight, 0) ;

        FindObjectOfType<PlayerController>().setBounds(map.localBounds.min, map.localBounds.max);
    }

    // Late update is called once per frame after update
    void LateUpdate()
    {
        if(target)
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        //keep camera inside bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x),
            Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }
}
