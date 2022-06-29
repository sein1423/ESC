using Photon.Pun;
using UnityEngine;
//vr사용
using UnityEngine.XR;
using UnityEngine.UI;
using Photon.Realtime;

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
    private float currentCameraRotationX = 0;
    private float currentCameraRotationY = 0;

    // 필요한 컴포넌트
    [SerializeField]
    private Rigidbody myRigid;

    Animator anim;

    //public GameObject weapons;
    public bool hasLighter = false;
    public GameObject light;
    public GameObject lighter;
    public bool isMenu = false;
    public GameObject MenuSet, OptionSet;
    public TextMesh PlayerNameTextBox;

    //플레이어의 기본적인 움직임 구현
    void Start()
    {
        anim = gameObject.transform.GetChild(1).GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
        originPosY = transform.position.y;
        applyCrouchPosY = crouchPosY;
        applySpeed = walkSpeed;
        PlayerNameTextBox =
            gameObject.transform.GetChild(4).GetComponent<TextMesh>();
        PlayerNameTextBox.text = PhotonNetwork.NickName;

    }

    // Update is called once per frame
    private void Update()
    {
        if(!isMenu)
        {
            PlayerActive();
        }

        if (MenuSet.activeSelf || OptionSet.activeSelf)
        {
            isMenu = true;
        }
        else
        {
            isMenu = false;
        }
    }
    /*
        public void VrPlayerActive()
        {
            //VR 컨트롤러 조이스틱으로 전후좌우 이동
            float vrX = Input.GetAxis("Horizontal");
            float vrZ = Input.GetAxis("Vertical");
            Vector3 moveDirection = new Vector3(vrX, 0, vrZ);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.y = 0;
            moveDirection *= applySpeed;
            myRigid.MovePosition(myRigid.position + moveDirection * Time.deltaTime);

            //VR헤드기어 움직임에 따라 카메라 회전
            float vrY = Input.GetAxis("Rotation");
            transform.Rotate(0, vrY, 0);
            currentCameraRotationX -= vrY;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
        }*/
    [PunRPC]
    public void PlayerActive()
    {
        //밑에거 Input.GetKeyDown을 "inputmanager"이런식으로 바꿔야함
        

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
    [PunRPC]
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

        /*
                //좌우 방향에 따라 캐릭터를 회전시키는 구현
                if (h != 0)
                {
                    transform.rotation = Quaternion.Euler(0, Mathf.Atan2(moveVelocity.x, moveVelocity.z) * Mathf.Rad2Deg, 0);
                }*/

        //움직임 애니메이션 실행
        if (h != 0 || v != 0)
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
