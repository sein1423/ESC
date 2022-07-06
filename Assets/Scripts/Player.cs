using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using PN = Photon.Pun.PhotonNetwork;
using PV = Photon.Pun.PhotonView;
using UnityEngine.UI;

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
    private float currentCameraRotationX;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;

    Animator anim;

    GameObject nearObject;
    bool iDown;
    public GameObject weapons;
    public bool hasWeapons;
    public GameObject light;
    public GameObject lighter;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    GameObject equipWeapon;
    bool isSwap;
    int equipWeaponIndex = -1;

    void Awake()
    {
        if (PN.LocalPlayer.NickName == GameManagement.staticPlayerName || GameManagement.staticPlaymode == "soloplay")
        {
            anim = GetComponentInChildren<Animator>();
        }
    }


    void Start()
    {
        if (PN.LocalPlayer.NickName == GameManagement.staticPlayerName || GameManagement.staticPlaymode == "soloplay")
        {
            // ������Ʈ �Ҵ�
            capsuleCollider = GetComponent<CapsuleCollider>();
            myRigid = GetComponent<Rigidbody>();

            // �ʱ�ȭ
            applySpeed = walkSpeed;

            originPosY = theCamera.transform.localPosition.y;
            applyCrouchPosY = originPosY;
        }
    }

    void Update()
    {
        if (PN.LocalPlayer.NickName == GameManagement.staticPlayerName || GameManagement.staticPlaymode == "soloplay")
        {
            GetInput();
            IsGround();
            TryJump();
            TryRun();
            TryCrouch();
            Move();
            GetLighter();
            CameraRotation();
            CharacterRotation();
            Interaction();
        }
    }

    void GetInput()
    {
        iDown = Input.GetButtonDown("Interaction");
        sDown1 = Input.GetButtonDown("Swap1");
    }

    // ���� üũ
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    // ���� �õ�
    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    // ����
    private void Jump()
    {
        if (isCrouch)
            Crouch();

        myRigid.velocity = transform.up * jumpForce;
    }

    // �޸��� �õ�
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    // �޸���
    private void Running()
    {
        if (isCrouch)
            Crouch();

        isRun = true;
        applySpeed = runSpeed;
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        anim.SetBool("isRun", _velocity != Vector3.zero);
    }

    // �޸��� ���
    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        anim.SetBool("isWalk", _velocity != Vector3.zero);
    }

    // �ɱ� �õ�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    // �ɱ� ����
    private void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    // �ε巯�� �ɱ� ����
    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;

        while (_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.2f);
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

        anim.SetBool("isWalk", _velocity != Vector3.zero);
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;

        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));
    }

    void GetLighter()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (light.activeSelf == true)
            {
                light.SetActive(false);
            }
            else
            {
                light.SetActive(true);
            }
        }

    }
    void Interaction()
    {
        if(iDown && nearObject != null)
        {
            if(nearObject.tag == "Weapon")
            {
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons = true;

                Destroy(nearObject);
                lighter.SetActive(true);
            }
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name);
        if (other.tag == "Weapon")
            nearObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearObject = null;
    }

}
