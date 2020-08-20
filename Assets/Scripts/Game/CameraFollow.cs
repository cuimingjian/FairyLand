using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    private Transform target;
    private Vector3 offset;
    private Vector2 ver;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null&& GameObject.FindGameObjectWithTag("Player")!= null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            offset = target.position - transform.position; 
        }
	}

    private void FixedUpdate()
    {
        if(target != null)
        {
            float posx = Mathf.SmoothDamp(transform.position.x, target.position.x - offset.x, ref ver.x, 0.05f);
            float posy = Mathf.SmoothDamp(transform.position.y, target.position.y-offset.y, ref ver.y, 0.05f);
            if(posy > transform.position.y)
            transform.position = new Vector3(posx, posy, transform.position.z);
        }
    }
}
