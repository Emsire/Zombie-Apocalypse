using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;
    public float bodyHeight;

    public float moveSpeed;
    public float rotationSpeed;

    private Vector3 startPos, targetPos;
    private Quaternion startQua, targetQua;
    private float VecTime;
    private float QuaTime;
    private Vector3 tempPos;
    private Quaternion tempQua;
    
    private void Update()
    {
        if (target == null)
            return;

        tempPos = target.position + Vector3.up * offsetPos.y + target.forward * offsetPos.z + target.right * offsetPos.x;
        if (targetPos!= tempPos)
        {
            startPos = this.transform.position;
            VecTime = 0;
            targetPos = tempPos;
        }

        tempQua = Quaternion.LookRotation((target.position + Vector3.up * bodyHeight) - this.transform.position);
        if (targetQua != tempQua)
        {
            startQua = this.transform.rotation;
            QuaTime = 0;
            targetQua = tempQua;
        }

        VecTime += Time.deltaTime;
        this.transform.position = Vector3.Lerp(startPos, targetPos, VecTime*moveSpeed);

        QuaTime += Time.deltaTime;
        this.transform.rotation = Quaternion.Slerp(startQua, targetQua, QuaTime*rotationSpeed);
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }
}
