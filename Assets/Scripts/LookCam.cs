using UnityEngine;
using TMPro;

public class LookCam : MonoBehaviour
{
    public GameObject Cam;
    public Player py;
    Vector3 startScale;
    public float distance = 3;
    public TextMeshPro tmp;

    private void Start()
    {
        tmp = gameObject.GetComponent<TextMeshPro>();
        startScale = transform.localScale;
        
    }
    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(Cam.transform.position, transform.position);
        Vector3 newScale = startScale * dist / distance;
        transform.localScale = newScale;

        transform.rotation = Cam.transform.rotation;

        //tmp.text =
    }
}
