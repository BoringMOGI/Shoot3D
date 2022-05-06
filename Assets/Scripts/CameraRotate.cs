using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    const string KEY_MOUSE_X = "SensitivityX";
    const string KEY_MOUSE_Y = "SensitivityY";

    [SerializeField] Transform playerBody;
    [SerializeField] Transform playerEye;

    [Range(0.0f, 90.0f)]
    [SerializeField]float limitUp;

    [Range(-90.0f, 0.0f)]
    [SerializeField] float limitDown;

    [Range(1f, 1000f)]
    [SerializeField] float sensitivityX;    // ���� ����.

    [Range(1f, 1000f)]
    [SerializeField] float sensitivityY;    // ���� ����.

    float rotateX;  // ���� ȸ�� ����.

    private void Start()
    {
        sensitivityX = PlayerPrefs.GetFloat(KEY_MOUSE_X, 200f);
        sensitivityY = PlayerPrefs.GetFloat(KEY_MOUSE_Y, 100f);

        Cursor.lockState = CursorLockMode.Locked;   // ���콺 ����.
    }

    private void Update()
    {
        // GetAxisRaw : -1 or 0 or 1.
        // GetAxis : -1.0f ~ 1.0f.

        float mouseX = Input.GetAxis("Mouse X") * sensitivityX * 0.005f;  // ���콺�� x�� �̵���.
        float mouseY = Input.GetAxis("Mouse Y") * sensitivityY * 0.005f;  // ���콺�� y�� �̵���.

        OnMouseLook(new Vector2(mouseX, mouseY)); 
    }

    private void OnMouseLook(Vector2 axix)
    {
        // ���� ȸ��
        playerBody.Rotate(Vector2.up * axix.x);

        // ���� ȸ��
        // ���콺�� ���� �̵����� ���� rotateX�� ���� ��ȯ. (��, ������ ������ �д�.)
        rotateX = Mathf.Clamp(rotateX - axix.y, limitUp * -1f, limitDown * -1f);
        playerEye.localRotation = Quaternion.Euler(rotateX, 0f, 0f);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat(KEY_MOUSE_X, sensitivityX);
        PlayerPrefs.SetFloat(KEY_MOUSE_Y, sensitivityY);
    }
}
