using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks
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
    private bool isGround = true;
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
    private float currentCameraRotationX = 0;
    private float currentCameraRotationY = 0;

    // 필요한 컴포넌트
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;

    Animator anim;

    GameObject nearObject;
    bool iDown;
    //public GameObject weapons;
    public bool hasLighter = false;
    public GameObject light;
    public GameObject lighter;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    GameObject equipWeapon;
    bool isSwap;
    int equipWeaponIndex = -1;

    //플레이어의 기본적인 움직임 구현
    void Start()
    {
        anim = gameObject.transform.GetChild(1).GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        originPosY = transform.position.y;
        applyCrouchPosY = crouchPosY;
        applySpeed = walkSpeed;
        isSwap = false;
        equipWeaponIndex = -1;

    }

    // Update is called once per frame
    private void Update()
    {
        //wasd로 전후좌우로 이동
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        //마우스의 회전값
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //PlayerCamera에 마우스 회전값 대입
        currentCameraRotationX -= mouseY * lookSensitivity;
        currentCameraRotationY += mouseX * lookSensitivity;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);

        //플레이어의 Y회전값은 카메라 Y회전값과 같음
        transform.eulerAngles = new Vector3(0, currentCameraRotationY, 0);



        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }

        // 앉기
        if (Input.GetKeyDown(KeyCode.C))
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
        if (Input.GetKeyDown(KeyCode.G))
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
        if (Input.GetKey(KeyCode.LeftShift))
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
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
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
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (other.tag == "Lighter")
            {
                Destroy(other.gameObject);
                light.SetActive(false);
                lighter.SetActive(true);
                hasLighter = true;
            }
        }

    }

}
