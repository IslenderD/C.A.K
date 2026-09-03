using UnityEngine;
using UnityEngine.UI;

public class MovementShip : MonoBehaviour
{
    float horiInput, vertInput; //Input for horizontal and vertical

    public float moveSpeed; //You can guess what this does

    public float speedStore;

    public enum States { Normal, Boost, Break }
    public States currentState = States.Normal;
    public bool actionActive;

    ShipEmission shipEmitters;
    public HUDManager hudManager;



    [SerializeField] Camera cam;

    public float tilt; //How much tilt
    public float tiltSpeed; //Speed of tilt
    Vector3 tiltAngle; //ANgle of tilt

    CameraControl camC;

    private void Awake()
    {
        speedStore = moveSpeed;
        shipEmitters = GetComponent<ShipEmission>();
        cam = Camera.main;
        camC = cam.GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
        horiInput = Input.GetAxisRaw("Horizontal");
        vertInput = Input.GetAxisRaw("Vertical");

        if (camC.isUpsideDown && !camC.omgIsLikeFez)
        {
            horiInput = -horiInput;
        }

        HandleTilting();
        SpeedInput();

        if (!camC.isTransitioning)
        {
            ClampToScreen();
        }

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

        Vector3 movement;
        if (camC.omgIsLikeFez)
        {
            movement = new Vector3(0, vertInput, horiInput);

            Vector3 localPos = transform.localPosition;
            localPos.x = Mathf.Lerp(localPos.x, 0f, Time.fixedDeltaTime * 5f);
            transform.localPosition = localPos;
        } else
        {
            movement = new Vector3(horiInput, vertInput, 0);
        }

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
        Vector3 pos = cam.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = cam.ViewportToWorldPoint(pos);
    }

    void SpeedInput()
    {
        if (actionActive && currentState == States.Normal)
        {
            if (Input.GetKeyDown(KeyCode.O)) // Boost
            {
                ActivateAction(States.Boost, 20f, -15f);
                shipEmitters.EmitBoost();
            }
            else if (Input.GetKeyDown(KeyCode.P)) // Brake
            {
                ActivateAction(States.Break, 5f, -5f);
                shipEmitters.EmitBrake();
            }
        }

        if (currentState != States.Normal && hudManager.actionCDSlider.value <= 0.05f)
        {
            SystemsNormal(-10f);
        }
    }

    void ActivateAction(States state, float newSpeed, float camZOffset)
    {
        currentState = state;
        actionActive = false;
        moveSpeed = newSpeed;
        camC.offSet.z = camZOffset;
        hudManager.actionCooling = false;
    }   

    void SystemsNormal(float camZOffset)
    {
        currentState = States.Normal;
        moveSpeed = speedStore;
        camC.offSet.z = camZOffset;
        hudManager.actionCooling = true;
        actionActive = false;
        shipEmitters.EmitNorm();
    }

}
