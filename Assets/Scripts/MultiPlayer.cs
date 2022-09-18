using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiPlayer : MonoBehaviourPunCallbacks
{

    // ?§Ìîº??Ï°∞Ï†ï Î≥Ä??
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;

    // ?êÌîÑ ?ïÎèÑ
    [SerializeField]
    private float jumpForce;

    // ?ÅÌÉú Î≥Ä??
    private bool isRun = false;
    public bool isGround = true;
    private bool isCrouch = false;

    // ?âÏïò?????ºÎßà???âÏùÑÏßÄ Í≤∞Ï†ï?òÎäî Î≥Ä??
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // ÎØºÍ∞ê??
    [SerializeField]
    private float lookSensitivity;

    // Ïπ¥Î©î???úÍ≥Ñ
    [SerializeField]
    private float cameraRotationLimit;
    public float currentCameraRotationX = 0;
    public float currentCameraRotationY = 0;

    // ?ÑÏöî??Ïª¥Ìè¨?åÌä∏
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
    //?åÎ†à?¥Ïñ¥??Í∏∞Î≥∏?ÅÏù∏ ?ÄÏßÅÏûÑ Íµ¨ÌòÑ

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

    public void PlayerActive()
    {

        //wasdÎ°??ÑÌõÑÏ¢åÏö∞Î°??¥Îèô
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        //Vector2 mouseX = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        GameObject.FindWithTag("Player").transform.eulerAngles = new Vector3(0, GameObject.Find("CenterEyeAnchor").transform.eulerAngles.y, 0);
        //PlayerCamera??ÎßàÏö∞???åÏ†ÑÍ∞??Ä??

        //currentCameraRotationY += mouseX.x * lookSensitivity;
        //transform.eulerAngles = new Vector3(0, currentCameraRotationY, 0);



        // ?êÌîÑ
        if (Input.GetButtonDown("Jump") && isGround)
        {
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGround = false;
        }

        // ?âÍ∏∞
        if (Input.GetButtonDown("Crouch"))
        {
            isCrouch = !isCrouch;
        }

        // ?âÍ∏∞ ?ÅÌÉú?êÏÑú ?ÄÏßÅÏûÑ ?úÌïú
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

        // ?¥Îèô
        Move(h, v);

        //g???ÖÎ†•???ºÏù¥?∞Ïùò Î∂àÏùÑ ÏºúÍ≥† ?àÎã§.
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

    //Move?®Ïàò Íµ¨ÌòÑ
    public void Move(float h, float v)
    {
        //h?Ä vÍ∞íÏúºÎ°??ÑÌõÑÏ¢åÏö∞ ?¥Îèô
        Vector3 moveVelocity = Vector3.zero;

        //shift?§Î? ?ÑÎ•¥Î©??ÑÏßÑ?çÎèÑÍ∞Ä Ï¶ùÍ??úÎã§.
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


        //?ÑÏßÑ,?ÑÏßÑ ?†ÎãàÎ©îÏù¥???§Ìñâ


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
