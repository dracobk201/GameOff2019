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

    private void Update()
    {
        CameraTargetPosition.Value = playerRigidbody.position;
    }

    public void Move()
    {
        if (!isGrounded)
            return;
        Vector3 targetPosition = playerRigidbody.position + new Vector3(HorizontalAxis.Value, 0, VerticalAxis.Value) * Time.deltaTime;
        playerRigidbody.MovePosition(targetPosition);
    }

    public void Rotate()
    {
        Vector3 playerRotation = Vector3.zero;
        playerRotation.y = RotationSpeed.Value * MouseHorizontalAxis.Value * Time.deltaTime;
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(playerRotation));
    }

    public void Shoot()
    {
        if (AimNewLocation())
        {
            playerRigidbody.position = targetLocation;
            CameraTargetPosition.Value = playerRigidbody.position;
        }
    }

    private bool AimNewLocation()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetLocation = hit.point;
            Debug.DrawRay(ray.origin, ray.direction,Color.red,2f);
            Debug.Log("Did Hit " + hit.transform.name +  " " + Vector3.Distance(transform.position, targetLocation));
            return true;
        }
        return false;
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
