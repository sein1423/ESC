using UnityEngine;
using TMPro;
using Photon.Pun;

public class LookCam : MonoBehaviourPunCallbacks
{
    public GameObject Cam;
    public MultiPlayer py;
    Vector3 startScale;
    public float distance = 3;
    public TextMeshPro tmp;

    private void Start()
    {
        tmp = gameObject.GetComponent<TextMeshPro>();
        startScale = transform.localScale;
        py = gameObject.transform.parent.GetComponent<MultiPlayer>();
        tmp.text = py.photonView.Owner.NickName;
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
