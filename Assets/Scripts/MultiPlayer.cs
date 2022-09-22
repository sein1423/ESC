using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiPlayer : MonoBehaviourPunCallbacks
{

    // ?�피??조정 변??
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;

    // ?�프 ?�도
    [SerializeField]
    private float jumpForce;

    // ?�태 변??
    private bool isRun = false;
    public bool isGround = true;
    private bool isCrouch = false;

    // ?�았?????�마???�을지 결정?�는 변??
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 민감??
    [SerializeField]
    private float lookSensitivity;

    // 카메???�계
    [SerializeField]
    private float cameraRotationLimit;
    public float currentCameraRotationX = 0;
    public float currentCameraRotationY = 0;

    // ?�요??컴포?�트
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

        //wasd�??�후좌우�??�동
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        //Vector2 mouseX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        //PlayerCamera??마우???�전�??�??

        //currentCameraRotationY += mouseX.x * lookSensitivity;
        //transform.eulerAngles = new Vector3(0, currentCameraRotationY, 0);



        // ?�프
        if (Input.GetButtonDown("Jump") && isGround)
        {
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }

        // ?�기
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouch = !isCrouch;
        }

        // ?�기 ?�태?�서 ?�직임 ?�한
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
        Move(h, v);

        //g???�력???�이?�의 불을 켜고 ?�다.
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

    //Move?�수 구현
    [PunRPC]
    public void Move(float h, float v)
    {
        //h?� v값으�??�후좌우 ?�동
        Vector3 moveVelocity = Vector3.zero;

        //shift?��? ?�르�??�진?�도가 증�??�다.
        if (Input.GetButton("Dash"))
        {
            applySpeed = runSpeed;
        }
        else
        {
            applySpeed = walkSpeed;
        }


        GameObject.FindWithTag("Player").transform.eulerAngles = new Vector3(0, GameObject.Find("CenterEyeAnchor").transform.eulerAngles.y, 0);

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


        //?�진,?�진 ?�니메이???�행


    }
    
    void OnTriggerStay(Collider other)
    {

        GameObject GMLighter = GameObject.Find("lighter");
        
        if (other.tag == "Lighter")
        {
            if (Input.GetButtonDown("Get"))
            {
                //Destroy(other.gameObject);
                GameManagement.staticGetLighter = true;
                Destroy(GMLighter);
                light.SetActive(false);
                    lighter.SetActive(true);
                    hasLighter = true;
            }      
        }

        if (other.tag == "Candle")
        {
            GameManagement.staticFireState = true;
            Debug.Log("true");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "RetryFloor")
        {
            gameObject.transform.position = new Vector3(0,0,0);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Candle")
        {
            GameManagement.staticFireState = false;
            Debug.Log("false");
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
