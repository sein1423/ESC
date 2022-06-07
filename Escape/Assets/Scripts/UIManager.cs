using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{

    
    public class UIManager : MonoBehaviour
    {
        private void Awake()
        {
            SetResolution(); // �ʱ⿡ ���� �ػ� ����

        }
        public void SetUpCanvasScaler(int setWidth, int setHeight)
        {
            CanvasScaler canvasScaler = FindObjectOfType<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(setWidth, setHeight);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        }
        /* �ػ� �����ϴ� �Լ� */
        public void SetResolution(int setWidth = 1920, int setHeight = 1080)
        {
            //int setWidth = 1920; // ����� ���� �ʺ�
            //int setHeight = 1080; // ����� ���� ����
            SetUpCanvasScaler(setWidth, setHeight);

            int deviceWidth = Screen.width; // ��� �ʺ� ����
            int deviceHeight = Screen.height; // ��� ���� ����

            Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution �Լ� ����� ����ϱ�

            if ((float)setWidth / setHeight < (float)deviceWidth / deviceHeight) // ����� �ػ� �� �� ū ���
            {
                float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // ���ο� �ʺ�
                Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f); // ���ο� Rect ����
            }
            else // ������ �ػ� �� �� ū ���
            {
                float newHeight = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // ���ο� ����
                Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight); // ���ο� Rect ����
            }
        }
    }
}

