using UnityEngine;
using UnityEngine.UI;

public class MovementShip : MonoBehaviour
{
    float horiInput, vertInput; //Input for horizontal and vertical

    public float moveSpeed; //You can guess what this does

    public float speedStore;
    public bool actionActive;

    ShipEmission shipEmitters;
    public HUDManager hudManager;

    [SerializeField] Camera cam;

    public float tilt; //How much tilt
    public float tiltSpeed; //Speed of tilt
    Vector3 tiltAngle; //ANgle of tilt

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        speedStore = moveSpeed;
        shipEmitters = GetComponent<ShipEmission>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        horiInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");


        HandleTilting();
        ClampToScreen();
        SpeedInput();

        if (hudManager.actionCDSlider.value >= 0.99f)
            actionActive = true;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement() //moves :D
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime, Space.Self);

        Vector3 movement = new Vector3(horiInput, vertInput, 0); //gets current movment
        transform.localPosition += movement * moveSpeed * Time.fixedDeltaTime;
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

    void SpeedInput()
    {
        if (Input.GetKeyDown(KeyCode.O) && actionActive) //boost
        {
            SpeedAction(20f, -15f);
            shipEmitters.EmitBoost();
        } else if (Input.GetKeyDown(KeyCode.O) || hudManager.actionCDSlider.value <= 0)
        {
            SystemsNormal(-10f);
        }

        if (Input.GetKeyDown(KeyCode.P) && actionActive) //brake
        {
            SpeedAction(5f, -5f);
            shipEmitters.EmitBrake();
        } else if (Input.GetKeyDown(KeyCode.P) || hudManager.actionCDSlider.value <= 0)
        {
            SystemsNormal(-10f);
        }
    }

    void SpeedAction(float newSpeed, float camZOffset)
    {
        moveSpeed = newSpeed;
        cam.GetComponent<CameraControl>().offSet.z = camZOffset;
        hudManager.actionCooling = false;
    }

    void SystemsNormal(float camZOffset)
    {
        moveSpeed = speedStore;
        cam.GetComponent<CameraControl>().offSet.z = camZOffset;
        hudManager.actionCooling = true;
        actionActive = false;
        shipEmitters.EmitNorm();
    }

}
