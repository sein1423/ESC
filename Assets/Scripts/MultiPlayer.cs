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
    public GameObject Oculus;
    public bool GetLight = false;

    //�÷��̾��� �⺻���� ������ ����

    private void Awake()
    {
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

    }

    public void PlayerActive()
    {

        //wasd�� �����¿�� �̵�
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");


        //Vector2 mouseX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        GameObject.FindWithTag("Player").transform.eulerAngles = new Vector3(0, GameObject.Find("CenterEyeAnchor").transform.eulerAngles.y, 0);
        //PlayerCamera�� ���콺 ȸ���� ����

        //currentCameraRotationY += mouseX.x * lookSensitivity;
        //transform.eulerAngles = new Vector3(0, currentCameraRotationY, 0);



        // ����
        if (Input.GetButtonDown("Jump") && isGround)
        {
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }

        // �ɱ�
        if (Input.GetButtonDown("Crouch"))
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

    //Move�Լ� ����
    public void Move(float h, float v)
    {
        //h�� v������ �����¿� �̵�
        Vector3 moveVelocity = Vector3.zero;

        //shiftŰ�� ������ �����ӵ��� �����Ѵ�.
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


        //����,���� �ִϸ��̼� ����


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
