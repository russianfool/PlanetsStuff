using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class GravityAttractor : MonoBehaviour {

    public float mass; // This should really be a protected with the "GravityBody" class as a friend, but... C# + Unity != C++

    void Start() {
        mass = GetComponent<Rigidbody>().mass;
    }
}
