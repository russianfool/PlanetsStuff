using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof (Rigidbody))]
public class GravityBody : MonoBehaviour {

    // Can change to ints later cuz fuck it
    public float reasonableG = 0.01f;
    public float reasonableRot = 50f;

    private List<GravityAttractor> attractors;
    private Rigidbody myBody;
    private Transform myTransform;

    void Start () {
        // Save access time
        myBody = GetComponent<Rigidbody>();
        myTransform = myBody.transform;

        myBody.useGravity = false;
		myBody.constraints = RigidbodyConstraints.FreezeRotation;

        attractors = new List<GravityAttractor>();

        // Loop over all the game objects and find everything we can be attracted to, and memorize it.
        // For now we assume planets are all there and we don't care about incurring extra calculation costs.
        object[] obj = FindObjectsOfType(typeof(GameObject));
        foreach (object o in obj) {
            GameObject g = (GameObject)o;
            GravityAttractor a = g.GetComponent<GravityAttractor>();
            if (a) {
                attractors.Add(a);
                Debug.Log("Found object as attractor of name " + a.name);
            }  
        }
    }

    // Quick implementation of the gravity law, because we're awesome.
    private Vector3 Attraction(GravityAttractor attractor) {
        Vector3 r = myBody.position - attractor.transform.position;
        Vector3 result = ((reasonableG * myBody.mass * attractor.mass) / Mathf.Pow(r.magnitude, 2f)) * r.normalized;
        return result;
    }

	void FixedUpdate () {
        Vector3 total = new Vector3(0f,0f,0f);

        // Use largest gravitational field to orient ourselves
        Vector3 tmp;
        Vector3 max = new Vector3(0f, 0f, 0f);

        foreach (GravityAttractor a in attractors) {
            tmp = Attraction(a);
            if (tmp.magnitude > max.magnitude) {
                max = tmp;
            }
            total -= tmp;
        }

        // Get them gravities
        myBody.GetComponent<Rigidbody>().AddForce(total);

        // Enemy gate is down, reorient ourselves
        Quaternion targetRotation = Quaternion.FromToRotation(myTransform.up, -total.normalized) * myTransform.rotation;
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, targetRotation, reasonableRot * Time.deltaTime);
    }
}
