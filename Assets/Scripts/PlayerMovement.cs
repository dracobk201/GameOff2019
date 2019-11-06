using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FloatReference MoveSpeed;
    [SerializeField]
    private FloatReference RotationSpeed;
    [SerializeField]
    private FloatReference HorizontalAxis;
    [SerializeField]
    private FloatReference VerticalAxis;
    [SerializeField]
    private FloatReference MouseHorizontalAxis;
    [SerializeField]
    private Vector3Reference CameraTargetPosition;
    private Rigidbody playerRigidbody;
    private bool isGrounded;
    private Vector3 targetLocation;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void Move()
    {
        if (!isGrounded)
            return;
        Vector3 targetPosition = playerRigidbody.position + new Vector3(HorizontalAxis.Value, 0, VerticalAxis.Value) * Time.deltaTime;
        playerRigidbody.MovePosition(targetPosition);
        CameraTargetPosition.Value = playerRigidbody.position;
    }

    public void Rotate()
    {
        Vector3 playerRotation = Vector3.zero;
        playerRotation.y = RotationSpeed.Value * MouseHorizontalAxis.Value * Time.deltaTime;
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(playerRotation));
    }


    private void OnCollisionEnter(Collision collision)
    {
        foreach (var item in collision.contacts)
        {
            if (item.otherCollider.CompareTag(Global.GROUNDTAG))
                isGrounded = true;
        }
    }
}
