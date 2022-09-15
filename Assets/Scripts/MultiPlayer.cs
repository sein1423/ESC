using Photon.Pun;
using UnityEngine;


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

    //플레이어의 기본적인 움직임 구현

    private void Awake()
    {
        Oculus = GameObject.Find("OVRCameraRig");
        Oculus.transform.position = this.transform.position;
        this.transform.parent = Oculus.transform;
    }
    void Start()
    {
        gm = GameObject.Find("GameManagement").GetComponent<GameManagement>();
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
            if(GameObject.Find("MultiplayManager") as GameObject)
            {
                if (photonView.IsMine)
                {
                    PlayerActive();
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

        //마우스의 회전값
        //Vector3 JoyMove = 

        float mouseX = Input.GetAxis("XRI_Right_Secondary2DAxis_Vertical");
        float mouseY = Input.GetAxis("XRI_Right_Secondary2DAxis_Horizontal");
       
        //PlayerCamera에 마우스 회전값 대입

        /*currentCameraRotationX -= mouseY * lookSensitivity;
        currentCameraRotationY += mouseX * lookSensitivity;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);

        transform.eulerAngles = new Vector3(0, currentCameraRotationY, 0);*/



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
        moveVelocity.z = v * applySpeed;
        moveVelocity = transform.TransformDirection(moveVelocity);
        myRigid.MovePosition(myRigid.position + moveVelocity * Time.deltaTime);

        //전진,후진 애니메이션 실행
        if (v != 0)
        {
            //anim.SetBool("isWalk", true);
        }
        else
        {
            //anim.SetBool("isWalk", false);
        }
        /*
                //좌우 방향에 따라 캐릭터를 회전시키는 구현
                if (h != 0)
                {
                    transform.rotation = Quaternion.Euler(0, Mathf.Atan2(moveVelocity.x, moveVelocity.z) * Mathf.Rad2Deg, 0);
                }*/

        //좌우 움직임 애니메이션 실행
        if (h != 0)
        {
            //anim.SetBool("isRun", true);
        }
        else
        {
            //anim.SetBool("isRun", false);
        }


    }
    void OnTriggerStay(Collider other)
    {
        
        if (Input.GetButtonDown("Get"))
        {
                Destroy(other.gameObject);
                light.SetActive(false);
                lighter.SetActive(true);
                hasLighter = true;
        }      
    }

    public void GetLighter(GameObject other)
    {
            Destroy(other.gameObject);
            light.SetActive(false);
            lighter.SetActive(true);
            hasLighter = true;
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
