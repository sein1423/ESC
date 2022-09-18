using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiPlayer : MonoBehaviourPunCallbacks
{

    // ?€νΌ??μ‘°μ  λ³??
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;

    // ?ν ?λ
    [SerializeField]
    private float jumpForce;

    // ?ν λ³??
    private bool isRun = false;
    public bool isGround = true;
    private bool isCrouch = false;

    // ?μ?????Όλ§???μμ§ κ²°μ ?λ λ³??
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // λ―Όκ°??
    [SerializeField]
    private float lookSensitivity;

    // μΉ΄λ©???κ³
    [SerializeField]
    private float cameraRotationLimit;
    public float currentCameraRotationX = 0;
    public float currentCameraRotationY = 0;

    // ?μ??μ»΄ν¬?νΈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;

    Animator anim;

    //public GameObject weapons;
    public bool hasLighter = false;
    public GameObject light;
    public GameObject lighter;
    public bool isMenu = false;
    public GameObject MenuSet, OptionSet;
    public GameManagement gm;
    public string nickname;
    public GameObject Oculus;
    public bool GetLight = false;
    public GameObject anotherPlayer;

    private void Awake()
    {
    }
    void Start()
    {
        Debug.Log("Player Start");
        if (GameObject.Find("MultiplayManager") != null)
        {
            if (!(GameManagement.staticPlaymode == "soloplay" || GameManagement.staticPlaymode == null))
            {
                if (gameObject.transform.GetChild(2).GetComponent<PhotonView>().IsMine)
                {
                    gameObject.tag = "PlayerMine";
                }
                anotherPlayer = GameObject.FindWithTag("Player");
            }
        }
        gm = GameObject.Find("GameManagement").GetComponent<GameManagement>();
        walkSpeed = 40;
        runSpeed = 150;
        crouchSpeed = 1;
        jumpForce = 5;
        isGround = true;
        crouchPosY = 1;
        lookSensitivity = 2;
        cameraRotationLimit = 45;
        lighter = gameObject.transform.GetChild(0).transform.GetChild(5).transform.GetChild(0).transform.GetChild(1).gameObject;
        light = lighter.transform.GetChild(2).gameObject;
        //anim = gameObject.transform.GetChild(1).GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
        originPosY = transform.position.y;
        applyCrouchPosY = crouchPosY;
        applySpeed = walkSpeed;
        theCamera = gameObject.transform.GetChild(0).GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update()
    {
            if(GameObject.Find("MultiplayManager") != null)
            {
                if (gameObject.transform.GetChild(2).GetComponent<PhotonView>().IsMine)
                {
                    PlayerActive();
                    Debug.Log(MultiplayManager.Player1Position);
                    Debug.Log(MultiplayManager.Player1Rotation);
                    Debug.Log(MultiplayManager.Player2Position);
                    Debug.Log(MultiplayManager.Player2Rotation);
                }
            }
            else
            {
                PlayerActive();
            }
        

    }

    public void PlayerActive()
    {

        //wasdλ‘??νμ’μ°λ‘??΄λ
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        //Vector2 mouseX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        GameObject.FindWithTag("Player").transform.eulerAngles = new Vector3(0, GameObject.Find("CenterEyeAnchor").transform.eulerAngles.y, 0);
        //PlayerCamera??λ§μ°???μ κ°????

        //currentCameraRotationY += mouseX.x * lookSensitivity;
        //transform.eulerAngles = new Vector3(0, currentCameraRotationY, 0);



        // ?ν
        if (Input.GetButtonDown("Jump") && isGround)
        {
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }

        // ?κΈ°
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouch = !isCrouch;
        }

        // ?κΈ° ?ν?μ ?μ§μ ?ν
        if (isCrouch)
        {
            applyCrouchPosY = crouchPosY;
            applySpeed = crouchSpeed;
        }
        else
        {
            applyCrouchPosY = originPosY;
            applySpeed = walkSpeed;
        }

        // ?΄λ
        Move(h, v);

        if (gameObject.transform.GetChild(2).GetComponent<PhotonView>().ViewID == 1001)
        {
            MultiplayManager.Player1Position = gameObject.transform.position;
            MultiplayManager.Player1Rotation = gameObject.transform.GetChild(2).transform.rotation;
        }
        else
        {
            MultiplayManager.Player2Position = gameObject.transform.position;
            MultiplayManager.Player2Rotation = gameObject.transform.GetChild(2).transform.rotation;
        }
        //g???λ ₯???Όμ΄?°μ λΆμ μΌκ³  ?λ€.
        if (Input.GetButtonDown("Light") || OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger))
        {
            if (light.activeSelf)
            {
                light.SetActive(false);
            }
            else
            {
                light.SetActive(true);
            }
        }
    }

    //Move?¨μ κ΅¬ν
    public void Move(float h, float v)
    {
        //h? vκ°μΌλ‘??νμ’μ° ?΄λ
        Vector3 moveVelocity = Vector3.zero;

        //shift?€λ? ?λ₯΄λ©??μ§?λκ° μ¦κ??λ€.
        if (Input.GetButton("Dash"))
        {
            applySpeed = runSpeed;
        }
        else
        {
            applySpeed = walkSpeed;
        }


        
        moveVelocity.x = h * applySpeed;
        moveVelocity.y = 0;
        moveVelocity.z = v * applySpeed;
        moveVelocity = transform.TransformDirection(moveVelocity);
        //Debug.Log(GameObject.FindWithTag("PlayerTransform").transform.rotation);
        /*float angle = GameObject.FindWithTag("PlayerTransform").transform.rotation.y * 100f;
        if(angle < 0)
        {
            angle = 360f - angle;
        }
        moveVelocity = Quaternion.AngleAxis(angle, Vector3.up) * moveVelocity;
        Debug.Log(moveVelocity);*/

        Vector3 dir = GameObject.Find("CenterEyeAnchor").transform.TransformDirection(moveVelocity);
        //(moveVelocity.normalized * Time.deltaTime);
        /*
        GameObject.FindWithTag("Player").transform.Translate(moveVelocity * .01f);*/


        if (h != 0 || v != 0)
        gameObject.transform.Translate(dir.normalized * Time.deltaTime * applySpeed);


        //?μ§,?μ§ ? λλ©μ΄???€ν


    }
    void OnTriggerStay(Collider other)
    {

        GameObject GMLighter = GameObject.Find("lighter");
        
        if (other.tag == "Lighter")
        {
            if (Input.GetButtonDown("Get"))
            {
                //Destroy(other.gameObject);
                Destroy(GMLighter);
                light.SetActive(false);
                    lighter.SetActive(true);
                    hasLighter = true;
                GameManagement.staticGetLighter = true;
            }      
        }
    }

    public void GetLighter(GameObject other)
    {
        GameObject GMLighter = GameObject.Find("lighter");

        if (other.tag == "Lighter")
        {
            //Destroy(other.gameObject);
            Destroy(GMLighter);
            light.SetActive(false);
            lighter.SetActive(true);
            hasLighter = true;
        }
    }

    public void RPC_Light()
    {
        light.SetActive(true);
    }

    public void RPC_Lighter()
    {
        lighter.SetActive(true);
    }
}
