using UnityEngine;
using TMPro;
using Photon.Pun;

public class LookCam : MonoBehaviourPunCallbacks
{
    GameObject Cam;
    MultiPlayer py;
    Vector3 startScale;
    public float distance = 20;
    TextMeshPro tmp;


    private void Awake()
    {
        if (GameManagement.staticPlaymode == "soloplay" || GameManagement.staticPlaymode == null)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        tmp = gameObject.GetComponent<TextMeshPro>();
        startScale = transform.localScale;
        py = gameObject.transform.parent.GetComponent<MultiPlayer>();
        tmp.text = py.photonView.Owner.NickName;
        Cam = GameObject.Find("Main Camera");
    }


    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(Cam.transform.position, transform.position);
        Vector3 newScale = startScale * dist / distance;
        transform.localScale = newScale;
        transform.rotation = Cam.transform.rotation;

    }
}
