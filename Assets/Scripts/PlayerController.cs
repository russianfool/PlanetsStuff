using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

	public float moveSpeed = 15f;
    public float verticalBoost = 500f;

    private Vector3 moveDirection;
    private Rigidbody body;

    void Start() {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
    }

	void Update() {
		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
	}

	void FixedUpdate() {
        float up = Input.GetButton("Jump") ? verticalBoost : 0f;
        body.MovePosition(body.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
        Debug.Log(transform.TransformDirection(0f, up, 0f).ToString());
        body.AddForce(transform.TransformDirection(0f, up, 0f));
	}
}
