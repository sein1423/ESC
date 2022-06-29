using Photon.Pun;
using UnityEngine;
//vr���
using UnityEngine.XR;
using UnityEngine.UI;
using Photon.Realtime;

public class Player : MonoBehaviourPunCallbacks
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
    private float currentCameraRotationX = 0;
    private float currentCameraRotationY = 0;

    // �ʿ��� ������Ʈ
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

    //�÷��̾��� �⺻���� ������ ����
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
            //VR ��Ʈ�ѷ� ���̽�ƽ���� �����¿� �̵�
            float vrX = Input.GetAxis("Horizontal");
            float vrZ = Input.GetAxis("Vertical");
            Vector3 moveDirection = new Vector3(vrX, 0, vrZ);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.y = 0;
            moveDirection *= applySpeed;
            myRigid.MovePosition(myRigid.position + moveDirection * Time.deltaTime);

            //VR����� �����ӿ� ���� ī�޶� ȸ��
            float vrY = Input.GetAxis("Rotation");
            transform.Rotate(0, vrY, 0);
            currentCameraRotationX -= vrY;
            currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
            theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);
        }*/
    [PunRPC]
    public void PlayerActive()
    {
        //�ؿ��� Input.GetKeyDown�� "inputmanager"�̷������� �ٲ����
        

        //wasd�� �����¿�� �̵�
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        //���콺�� ȸ����
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        //PlayerCamera�� ���콺 ȸ���� ����
        currentCameraRotationX -= mouseY * lookSensitivity;
        currentCameraRotationY += mouseX * lookSensitivity;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);
        
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
    [PunRPC]
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

        /*
                //�¿� ���⿡ ���� ĳ���͸� ȸ����Ű�� ����
                if (h != 0)
                {
                    transform.rotation = Quaternion.Euler(0, Mathf.Atan2(moveVelocity.x, moveVelocity.z) * Mathf.Rad2Deg, 0);
                }*/

        //������ �ִϸ��̼� ����
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
