using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerNew : Player
{
    public float mouseSensivity = 1000f;

    public override float Hp { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override float MaxHp { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public override float Speed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        transform.Rotate(Vector3.right * -mouseY);
    }
}
