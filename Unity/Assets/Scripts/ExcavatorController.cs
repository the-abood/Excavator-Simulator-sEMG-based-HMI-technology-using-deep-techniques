using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeverAngles
{
    public float upDown;
    public float leftRight;


    public LeverAngles(float upDown, float leftRight)
    {
        this.upDown = upDown;
        this.leftRight = leftRight;
    }

    public bool isOperated()
    {
        if (this.upDown != 0 || this.leftRight != 0) return true;
        else return false;
    }

    public void clear()
    {
        this.upDown = 0F;
        this.leftRight = 0F;
    }
}


public class InputEvents
{
    private float _swing;
    private float _boom;
    private float _arm;
    private float _bucket;
    private float _trackRight;
    private float _trackLeft;

    private bool _updated;

    public void clear()
    {
        _swing = 0F;
        _boom = 0F;
        _arm = 0F;
        _bucket = 0F;
        _trackRight = 0F;
        _trackLeft = 0F;

        _updated = false;
    }

    public bool isUpdated
    {
        get { return _updated; }
    }

    public float swing
    {
        set { _swing = value; _updated = true; }
        get { return _swing; }
    }

    public float boom
    {
        set { _boom = value; _updated = true; }
        get => _boom;
    }

    public float arm
    {
        set { _arm = value; _updated = true; }
        get => _arm;
    }

    public float bucket
    {
        set { _bucket = value; _updated = true; }
        get => _bucket;
    }

    public float trackRight
    {
        set { _trackRight = value; _updated = true; }
        get => _trackRight;
    }

    public float trackLeft
    {
        set { _trackLeft = value; _updated = true; }
        get => _trackLeft;
    }

}


public class ExcavatorController : MonoBehaviour
{
    public float maxSpeed = 3F;
    public float creepSpeed = 1F;
    public float initialAccel = 15F;
    public float deltaAccel = 0.5F;
    public float maxAccel = 45F;
    public float deltaDirection = 2F;

    public bool enableRearCameras = false;

    public bool useHook = false;

    private DriveParams driveParams;

    private InputEvents inputEvents = new InputEvents();

    public Excavator excavator;

    bool travel = false;
    Image travelIndicator;
    Image operationIndicator;

    [Space]
    [SerializeField] private float swingTime = 1f;
    [SerializeField] private float armTime = 1f;
    [SerializeField] private float boomTime = 1f;
    [SerializeField] private float bucketTime = 1f;
    [SerializeField] private float trackTime = 1f;
    [Space]
    [SerializeField] private TextAsset csvFile;


    private float currentSwingTime;
    private float currentArmTime;
    private float currentBoomTime;
    private float currentBucketTime;
    private float currentTrackTime;
    

    private void ProcessKeyEvents(InputEvents inputEvents)
    {

        #region Swing
        // Swing
        if (Input.GetKey(KeyCode.H) && currentSwingTime <= swingTime)
        {
            currentSwingTime += Time.deltaTime;

            inputEvents.swing = 1F;
        }  // Swing right
        else if (Input.GetKey(KeyCode.F) && currentSwingTime <= swingTime)
        {
            currentSwingTime += Time.deltaTime;

            inputEvents.swing = -1F;
        }  // Swing left

        // Swing
        if (Input.GetKeyUp(KeyCode.H))
        {
            currentSwingTime = 0;

        }  // Swing right
        else if (Input.GetKeyUp(KeyCode.F))
        {
            currentSwingTime = 0;

        }  // Swing left
        #endregion

        #region Arm
        // Arm
        if (Input.GetKey(KeyCode.T) || currentArmTime<=armTime) 
        { 
            currentArmTime += Time.deltaTime;

            inputEvents.arm = 1F;
        }  // Arm roll out
        else if (Input.GetKey(KeyCode.G) && currentArmTime<=armTime) 
        {
            currentArmTime += Time.deltaTime;

            inputEvents.arm = -1F;
        }  // Arm rool in

        // Arm
        if (Input.GetKeyUp(KeyCode.T))
        {
            currentArmTime = 0;

        }  // Arm roll out
        else if (Input.GetKeyUp(KeyCode.G))
        {
            currentArmTime = 0;

        }  // Arm rool in
        #endregion

        #region Boom
        // Boom
        if (Input.GetKey(KeyCode.I) && currentBoomTime<=boomTime) 
        { 
            currentBoomTime += Time.deltaTime;

            inputEvents.boom = 1F;
        }  // Boom roll in
        else if (Input.GetKey(KeyCode.K) || currentBoomTime <= boomTime) 
        {
            currentBoomTime += Time.deltaTime;

            inputEvents.boom = -1F;
        }  // Boom roll out

        // Boom
        if (Input.GetKeyUp(KeyCode.I))
        {
            currentBoomTime = 0;

        }  // Boom roll in
        else if (Input.GetKeyUp(KeyCode.K))
        {
            currentBoomTime = 0;

        }  // Boom roll out
        #endregion

        #region Bucket
        // Bucket
        if (Input.GetKey(KeyCode.L) || currentBucketTime<=bucketTime) 
        { 
            currentBucketTime += Time.deltaTime;

            inputEvents.bucket = 1F;
        }  // Bucket roll out
        else if (Input.GetKey(KeyCode.J) && currentBucketTime <= bucketTime) 
        {
            currentBucketTime += Time.deltaTime;

            inputEvents.bucket = -1F;
        }  // Bucket roll in

        // Bucket
        if (Input.GetKeyUp(KeyCode.L))
        {
            currentBucketTime = 0;

        }  // Bucket roll out
        else if (Input.GetKeyUp(KeyCode.J))
        {
            currentBucketTime = 0;

        }  // Bucket roll in
        #endregion

        #region Track
        // Track
        if (Input.GetKey(KeyCode.U) && currentTrackTime<=trackTime) 
        { 
            currentTrackTime += Time.deltaTime;

            inputEvents.trackRight = 1F;
        }  // Track right
        else if (Input.GetKey(KeyCode.O) && currentTrackTime <= trackTime) 
        {
            currentTrackTime += Time.deltaTime;

            inputEvents.trackRight = -1F; 
        }
        if (Input.GetKey(KeyCode.Y) && currentTrackTime <= trackTime) 
        {
            currentTrackTime += Time.deltaTime;

            inputEvents.trackLeft = 1F;
        }  // Track left
        else if (Input.GetKey(KeyCode.R) && currentTrackTime <= trackTime) 
        {
            currentTrackTime += Time.deltaTime;

            inputEvents.trackLeft = -1F;
        }

        // Track
        if (Input.GetKeyUp(KeyCode.U))
        {
            currentTrackTime = 0;

        }  // Track right
        else if (Input.GetKeyUp(KeyCode.O))
        {
            currentTrackTime = 0;

        }
        if (Input.GetKeyUp(KeyCode.Y))
        {
            currentTrackTime = 0;

        }  // Track left
        else if (Input.GetKeyUp(KeyCode.R))
        {
            currentTrackTime = 0;

        }

        #endregion
    }

    // Start is called before the first frame update
    void Start()
    {
        currentSwingTime = 0;
        currentArmTime = 0;
        currentBoomTime = 0;
        currentBucketTime = 0;
        currentTrackTime = 0;


        excavator = new Excavator(transform.root.gameObject);

        travelIndicator = GameObject.FindWithTag("TravelIndicator").GetComponent<Image>();
        operationIndicator = GameObject.FindWithTag("OperationIndicator").GetComponent<Image>();
        travelIndicator.color = new Color(0, 0, 0);
        operationIndicator.color = new Color(0, 1F, 0);
        float mass = gameObject.GetComponent<Rigidbody>().mass;
        driveParams = new DriveParams(mass, maxSpeed, creepSpeed, initialAccel, deltaAccel, maxAccel, deltaDirection);
        excavator.useHook = useHook;

        ReadCSV();
    }


    #region read csv file

    private Dictionary<string, float> values = new Dictionary<string, float>();


    void ReadCSV()
    {
        if (csvFile == null)
        {
            Debug.LogError("CSV file is not assigned.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');

        foreach (string line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 2)
            {
                string fieldName = parts[0];
                float fieldValue;
                if (float.TryParse(parts[1], out fieldValue))
                {
                    values[fieldName] = fieldValue;
                }
                else
                {
                    Debug.LogError("Error parsing value for field: " + fieldName);
                }
            }
            else
            {
                Debug.LogError("Invalid line format: " + line);
            }
        }

        // Display the values read from the CSV file
        foreach (var kvp in values)
        {
            //Debug.Log("Field: " + kvp.Key + ", Value: " + kvp.Value);
        }

        swingTime = values["swingTime"];
        armTime = values["armTime"];
        boomTime = values["boomTime"];
        bucketTime = values["bucketTime"];
        trackTime = values["trackTime"];
    }

    #endregion

    private LeverAngles rightOperationLeverAngles = new LeverAngles(0F, 0F);
    private LeverAngles leftOperationLeverAngles = new LeverAngles(0F, 0F);
    private LeverAngles rightTravelLeverAngles = new LeverAngles(0F, 0F);
    private LeverAngles leftTravelLeverAngles = new LeverAngles(0F, 0F);

    void Update()
    {
        excavator.OrientHook();  // TODO: add hook operations
        //--- Code for manual operations from this line ---
        var delta = Time.deltaTime * 30F;

        

        ProcessKeyEvents(inputEvents);

        // Control
        if (inputEvents.isUpdated)
        {

            /* Swing */
            if (inputEvents.swing != 0F)
            {
                //Debug.Log(excavator.swingAngle);
                excavator.swingRotate(delta * 1F * inputEvents.swing);
                leftOperationLeverAngles.leftRight = 5F * inputEvents.swing;
            }

            /* Arm */
            if (inputEvents.arm != 0F)
            {
                excavator.armRotate(-delta * 1F * inputEvents.arm);
                leftOperationLeverAngles.upDown = 5F * inputEvents.arm;
            }

            /* Boom */
            if (inputEvents.boom != 0F)
            {
                excavator.boomRotate(-delta * 0.6F * inputEvents.boom);
                rightOperationLeverAngles.upDown = 5F * inputEvents.boom;
            }

            /* Bucket */
            if (inputEvents.bucket != 0F)
            {
                excavator.bucketRotate(delta * 1.5F * inputEvents.bucket);
                rightOperationLeverAngles.leftRight = 5F * inputEvents.bucket;
            }

            /* Tracks */
            if (inputEvents.trackRight != 0F || inputEvents.trackLeft != 0F)
{
                float rotationRight = -delta * 0.6F * inputEvents.trackRight;
                float rotationLeft = delta * 0.6F * inputEvents.trackLeft;
                float deltaRotation = rotationRight + rotationLeft;
                float input = inputEvents.trackRight + inputEvents.trackLeft;
                if (input > 1F) input = 1F;
                else if (input < -1F) input = -1F;
                float accel = 0F;
                if (input != 0F) accel = driveParams.initialAccel * Mathf.Sign(input) + (driveParams.maxAccel - driveParams.initialAccel) * input;
                excavator.Move(deltaRotation, accel, driveParams);
                rightTravelLeverAngles.upDown = 5F * inputEvents.trackRight;
                leftTravelLeverAngles.upDown = 5F * inputEvents.trackLeft;
            }

            inputEvents.clear();
        }

        if (rightOperationLeverAngles.isOperated() || leftOperationLeverAngles.isOperated())
        {
            excavator.rightOperationLeverRotate(rightOperationLeverAngles.leftRight, rightOperationLeverAngles.upDown);
            excavator.leftOperationLeverRotate(leftOperationLeverAngles.leftRight, leftOperationLeverAngles.upDown);
        }

        if (rightTravelLeverAngles.isOperated() || leftTravelLeverAngles.isOperated())
        {
            excavator.rightTravelLeverRotate(rightTravelLeverAngles.upDown);
            excavator.leftTravelLeverRotate(leftTravelLeverAngles.upDown);

            excavator.rightPedalRotate(-rightTravelLeverAngles.upDown*2F);
            excavator.leftPedalRotate(-leftTravelLeverAngles.upDown*2F);
        }

    }

}
