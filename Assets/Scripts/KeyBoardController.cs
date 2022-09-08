using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyBoardController : MonoBehaviour
{
    public string NickName = "";
    public GameObject TextField;
    public bool Shift = false;
    public InputField NickNameField;
    // Start is called before the first frame update
    void Start()
    {
        NickName = "";
        NickNameField = GameObject.Find("InputField").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        TextField.GetComponent<TextMeshProUGUI>().text = NickName;
    }

    public void InputKey(string a)
    {
        if(!Shift) NickName += a;
        else NickName += a.ToUpper();
    }

    public void InputShift(GameObject go)
    {
        Shift = !Shift;
        if (Shift) go.GetComponent<Image>().color = new Color(0, 0, 255);
        else go.GetComponent<Image>().color = new Color(0,0,0);
    }

    public void CompleteKey()
    {
        NickNameField.text = NickName;
        Destroy(gameObject);
    }

    public void CancleKey()
    {
        Destroy(gameObject);
    }
}
