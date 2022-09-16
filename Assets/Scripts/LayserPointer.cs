using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LayserPointer : MonoBehaviour
{
    private LineRenderer layser;        // ������
    private RaycastHit Collided_object; // �浹�� ��ü
    private GameObject currentObject;   // ���� �ֱٿ� �浹�� ��ü�� �����ϱ� ���� ��ü
    public GameObject KeyboardPrefabs;

    public float raycastDistance = 100f; // ������ ������ ���� �Ÿ�
    private TouchScreenKeyboard overlayKeyboard;
    public static string inputText = "";

    // Start is called before the first frame update
    void Start()
    {
        // ��ũ��Ʈ�� ���Ե� ��ü�� ���� ��������� ������Ʈ�� �ְ��ִ�.
        layser = this.gameObject.AddComponent<LineRenderer>();

        // ������ �������� ���� ǥ��
        Material material = new Material(Shader.Find("Standard"));
        material.color = new Color(0, 195, 255, 0.5f);
        layser.material = material;
        // �������� �������� 2���� �ʿ� �� ���� ������ ��� ǥ�� �� �� �ִ�.
        layser.positionCount = 2;
        // ������ ���� ǥ��
        layser.startWidth = 0.01f;
        layser.endWidth = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        layser.SetPosition(0, transform.position); // ù��° ������ ��ġ
                                                   // ������Ʈ�� �־� �����ν�, �÷��̾ �̵��ϸ� �̵��� ���󰡰� �ȴ�.
                                                   //  �� �����(�浹 ������ ����)
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.green, 0.5f);

        // �浹 ���� ��
        if (Physics.Raycast(transform.position, transform.forward, out Collided_object, raycastDistance))
        {
            layser.SetPosition(1, Collided_object.point);

            // �浹 ��ü�� �±װ� Button�� ���
            if (Collided_object.collider.gameObject.CompareTag("Button"))
            {
                // ��ŧ���� �� �����ܿ� ū ���׶�� �κ��� ���� ���
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    // ��ư�� ��ϵ� onClick �޼ҵ带 �����Ѵ�.
                    Collided_object.collider.gameObject.GetComponent<Button>().onClick.Invoke();
                }

                else
                {
                    Collided_object.collider.gameObject.GetComponent<Button>().OnPointerEnter(null);
                    currentObject = Collided_object.collider.gameObject;
                }
            }
            else if (Collided_object.collider.gameObject.CompareTag("InputField"))
            {
                // ��ŧ���� �� �����ܿ� ū ���׶�� �κ��� ���� ���
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    GameObject keyboard = Instantiate(KeyboardPrefabs);
                    keyboard.GetComponent<KeyBoardController>().Field = Collided_object.collider.gameObject.GetComponent<InputField>();
                    keyboard.transform.position = new Vector3(0, -5.5f, 8.13f);
                }
            }
            else
            {
                if(SceneManager.GetActiveScene().name == "Game")
                {
                    if (Collided_object.collider.gameObject.CompareTag("Lighter"))
                    {
                        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                        {
                            GameObject.Find("OVRCameraRig").GetComponent<MultiPlayer>().GetLighter(Collided_object.collider.gameObject);
                        }
                    }
                    else if (Collided_object.collider.gameObject.CompareTag("Candle"))
                    {
                        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                        {
                            if (GameObject.Find("OVRCameraRig").GetComponent<MultiPlayer>().light.activeSelf)
                            {
                                Collided_object.collider.gameObject.GetComponent<PuzzleCandle>().fire_state = true;
                                Collided_object.collider.gameObject.GetComponent<PuzzleCandle>().PuzzleControl();
                                Collided_object.collider.gameObject.GetComponent<PuzzleCandle>().fire_state = false;
                            }
                        }

                    }
                }
            }
        }

        else
        {
            // �������� ������ ���� ���� ������ ������ �ʱ� ���� ���̸�ŭ ��� �����.
            layser.SetPosition(1, transform.position + (transform.forward * raycastDistance));

            // �ֱ� ������ ������Ʈ�� Button�� ���
            // ��ư�� ���� �����ִ� �����̹Ƿ� �̰��� Ǯ���ش�.
            if (currentObject != null)
            {
                currentObject.GetComponent<Button>().OnPointerExit(null);
                currentObject = null;
            }

        }

    }

    private void LateUpdate()
    {
        // ��ư�� ���� ���        
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            layser.material.color = new Color(255, 255, 255, 0.5f);
        }

        // ��ư�� �� ���          
        else if (OVRInput.GetUp(OVRInput.Button.One))
        {
            layser.material.color = new Color(0, 195, 255, 0.5f);
        }
    }
}