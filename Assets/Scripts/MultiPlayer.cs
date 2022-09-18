using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiPlayer : MonoBehaviourPunCallbacks
{

    // 스피드 조정 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;

    // 점프 정도
    [SerializeField]
    private float jumpForce;

    // 상태 변수
    private bool isRun = false;
    public bool isGround = true;
    private bool isCrouch = false;

    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // 민감도
    [SerializeField]
    private float lookSensitivity;

    // 카메라 한계
    [SerializeField]
    private float cameraRotationLimit;
    public float currentCameraRotationX = 0;
    public float currentCameraRotationY = 0;

    // 필요한 컴포넌트
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
    //플레이어의 기본적인 움직임 구현

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
        //자신의 클라이언트 일때만 PlayerActive 함수를 실행함
        if (!gm.isMenu)
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
                else
                {
                    if (gameObject.transform.GetChild(2).GetComponent<PhotonView>().ViewID == 1001)
                    {
                        MultiplayManager.Player1Position = gameObject.transform.position;
                        MultiplayManager.Player1Rotation = gameObject.transform.GetChild(2).transform.rotation;
                        anotherPlayer.transform.position = MultiplayManager.Player2Position;
                        anotherPlayer.transform.rotation = MultiplayManager.Player2Rotation;
                    }
                    else
                    {
                        MultiplayManager.Player2Position = gameObject.transform.position;
                        MultiplayManager.Player2Rotation = gameObject.transform.GetChild(2).transform.rotation;
                        anotherPlayer.transform.position = MultiplayManager.Player1Position;
                        anotherPlayer.transform.rotation = MultiplayManager.Player1Rotation;
                    }
                }
            }
            else
            {
                PlayerActive();
            }
        }

    }

    public void PlayerActive()
    {

        //wasd로 전후좌우로 이동
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        //Vector2 mouseX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        GameObject.FindWithTag("Player").transform.eulerAngles = new Vector3(0, GameObject.Find("CenterEyeAnchor").transform.eulerAngles.y, 0);
        //PlayerCamera에 마우스 회전값 대입

        //currentCameraRotationY += mouseX.x * lookSensitivity;
        //transform.eulerAngles = new Vector3(0, currentCameraRotationY, 0);



        // 점프
        if (Input.GetButtonDown("Jump") && isGround)
        {
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }

        // 앉기
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouch = !isCrouch;
        }

        // 앉기 상태에서 움직임 제한
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

        // 이동
        Move(h, v);

        //g키 입력시 라이터의 불을 켜고 끈다.
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

    //Move함수 구현
    public void Move(float h, float v)
    {
        //h와 v값으로 전후좌우 이동
        Vector3 moveVelocity = Vector3.zero;

        //shift키를 누르면 전진속도가 증가한다.
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


        //전진,후진 애니메이션 실행


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
