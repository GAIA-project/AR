using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

    public int speed = 10;

    MeshRenderer meshRenderer;

	// Use this for initialization
	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Bounds bounds = meshRenderer.bounds;
        transform.RotateAround(bounds.center, transform.up, speed * Time.deltaTime);
	}
}
