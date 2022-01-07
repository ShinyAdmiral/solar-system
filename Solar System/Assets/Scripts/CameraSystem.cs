using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    [Range(0, 1)]
    [SerializeField] float speedAdjustSensitivity = 0.5f;

    [SerializeField] float mouseSensitivity = 0.5f;

    Vector3 vectorInput = Vector3.zero;
    Vector3 moveVector = Vector3.zero;
    Vector2 inputrotation = Vector2.zero;

    Quaternion originalOrientation;

    // Start is called before the first frame update
    void Start()
    {
        originalOrientation = transform.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        GetIpnut();
        ScrollToSpeed();
        Rotate();

        //update position
        transform.position += moveVector * speed * Time.deltaTime;
    }

    void GetIpnut() 
    {
        //clear input
        vectorInput = Vector3.zero;
        moveVector = Vector3.zero;

        //get keyboard input
        if (Input.GetKey(KeyCode.D)) vectorInput.x += 1.0f;
        if (Input.GetKey(KeyCode.A)) vectorInput.x -= 1.0f;
        if (Input.GetKey(KeyCode.E)) vectorInput.y += 1.0f;
        if (Input.GetKey(KeyCode.Q)) vectorInput.y -= 1.0f;
        if (Input.GetKey(KeyCode.W)) vectorInput.z += 1.0f;
        if (Input.GetKey(KeyCode.S)) vectorInput.z -= 1.0f;

        //vector in object space
        moveVector += transform.right * vectorInput.x;
        moveVector += transform.up * vectorInput.y;
        moveVector += transform.forward * vectorInput.z;

        //normalize vector
        moveVector.Normalize();
    }

    void ScrollToSpeed() 
    {
        //adjust scroll
        speed += Input.mouseScrollDelta.y * speedAdjustSensitivity;
    }

    void Rotate() 
    {
        //input
        inputrotation.y += Input.GetAxis("Mouse Y") * mouseSensitivity * Time.smoothDeltaTime;
        inputrotation.x += Input.GetAxis("Mouse X") * mouseSensitivity * Time.smoothDeltaTime;

        //no flipping
        inputrotation.y = Mathf.Clamp(inputrotation.y, -90, 90);

        //apply rotations
        transform.localEulerAngles = new Vector3(-inputrotation.y, inputrotation.x, 0);
    }
}
