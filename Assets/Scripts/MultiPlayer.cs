using Photon.Pun;
using UnityEngine;


public class MultiPlayer : MonoBehaviourPunCallbacks
{

    // ���ǵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;

    // ���� ����
    [SerializeField]
    private float jumpForce;

    // ���� ����
    private bool isRun = false;
    public bool isGround = true;
    private bool isCrouch = false;

    // �ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // �ΰ���
    [SerializeField]
    private float lookSensitivity;

    // ī�޶� �Ѱ�
    [SerializeField]
    private float cameraRotationLimit;
    public float currentCameraRotationX = 0;
    public float currentCameraRotationY = 0;

    // �ʿ��� ������Ʈ
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
    //�÷��̾��� �⺻���� ������ ����
    void Start()
    {
        gm = GameObject.Find("GameManagement").GetComponent<GameManagement>();
        anim = gameObject.transform.GetChild(1).GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
        originPosY = transform.position.y;
        applyCrouchPosY = crouchPosY;
        applySpeed = walkSpeed;
        //theCamera = gameObject.transform.GetChild(0).GetComponent<Camera>();
        
    }

    // Update is called once per frame
    private void Update()
    {
        //�ڽ��� Ŭ���̾�Ʈ �϶��� PlayerActive �Լ��� ������
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

        if (Input.GetMouseButton(0))
        {
            if (GameObject.Find("MultiplayManager") as GameObject)
            {
                Debug.Log(photonView.Owner.NickName);

            }
            Debug.Log(gameObject.transform.GetChild(0).transform.position);
            Debug.Log(gameObject.transform.position);
        }
    }

    public void PlayerActive()
    {

        //wasd�� �����¿�� �̵�
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        //���콺�� ȸ����
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //PlayerCamera�� ���콺 ȸ���� ����
        theCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 3.5f, transform.position.z);

        currentCameraRotationX -= mouseY * lookSensitivity;
        currentCameraRotationY += mouseX * lookSensitivity;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);


        //�÷��̾��� Yȸ������ ī�޶� Yȸ������ ����
        transform.eulerAngles = new Vector3(0, currentCameraRotationY, 0);


        // ����
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }

        // �ɱ�
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouch = !isCrouch;
        }

        // �ɱ� ���¿��� ������ ����
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

        // �̵�
        Move(h, v);


        //gŰ �Է½� �������� ���� �Ѱ� ����.
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

    //Move�Լ� ����
    public void Move(float h, float v)
    {
        //h�� v������ �����¿� �̵�
        Vector3 moveVelocity = Vector3.zero;

        //shiftŰ�� ������ �����ӵ��� �����Ѵ�.
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

        //����,���� �ִϸ��̼� ����
        if (v != 0)
        {
            anim.SetBool("isWalk", true);
        }
        else
        {
            anim.SetBool("isWalk", false);
        }
        /*
                //�¿� ���⿡ ���� ĳ���͸� ȸ����Ű�� ����
                if (h != 0)
                {
                    transform.rotation = Quaternion.Euler(0, Mathf.Atan2(moveVelocity.x, moveVelocity.z) * Mathf.Rad2Deg, 0);
                }*/

        //�¿� ������ �ִϸ��̼� ����
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

    public void RPC_Light()
    {
        light.SetActive(true);
    }

    public void RPC_Lighter()
    {
        lighter.SetActive(true);
    }
}
