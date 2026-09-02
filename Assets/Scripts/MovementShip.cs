using UnityEngine;
using UnityEngine.UI;

public class MovementShip : MonoBehaviour
{
    float horiInput, vertInput; //Input for horizontal and vertical

    public float moveSpeed; //You can guess what this does

    public float tilt; //How much tilt
    public float tiltSpeed; //Speed of tilt
    Vector3 tiltAngle; //ANgle of tilt

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horiInput = Input.GetAxis("Horizontal");
        vertInput = Input.GetAxis("Vertical");


        HandleTilting();
        ClampToScreen();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement() //moves :D
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        Vector3 movement = new Vector3(horiInput, vertInput, 0); //gets current movment
        transform.localPosition += new Vector3(movement.x, movement.y, movement.z) * moveSpeed * Time.deltaTime; //moves object to that movement
    }

    void HandleTilting()
    {
        TiltZ(horiInput);
        TiltX(vertInput);
    }

    void TiltX(float axis) //up/down
    {
        Vector3 targetEuAng = transform.localEulerAngles; //current rotation

        transform.localEulerAngles = new Vector3(Mathf.LerpAngle(targetEuAng.x, -axis * tilt, tiltSpeed), targetEuAng.y,targetEuAng.z); //interpolates targetEuAng.y and axis * tilt with the speed
    }
    void TiltZ(float axis) //right/left
    {
        Vector3 targetEuAng = transform.localEulerAngles; //current rotation
        
        transform.localEulerAngles = new Vector3(targetEuAng.x,
            Mathf.LerpAngle(targetEuAng.y, axis * tilt, tiltSpeed), //interpolates targetEuAng.y and axis * tilt with the speed
            Mathf.LerpAngle(targetEuAng.z, -axis * tilt, tiltSpeed)); 
    }

    void ClampToScreen()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
