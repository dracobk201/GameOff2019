using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FloatReference MoveSpeed;
    [SerializeField]
    private FloatReference TraslationSpeed;
    [SerializeField]
    private FloatReference RotationSpeed;
    [SerializeField]
    private FloatReference ReachDistance;
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
    private Vector3 targetNormal;

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
        StopAllCoroutines();
        if (AimNewLocation())
        {
            StartCoroutine(TranslatePlayer(targetLocation));
        }
    }

    private bool AimNewLocation()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, ReachDistance.Value))
        {
            targetLocation = hit.point;
            targetNormal = hit.normal;
            return true;
        }
        return false;
    }

    private IEnumerator TranslatePlayer (Vector3 position)
    {
        Vector3 heading = position - playerRigidbody.position;
        float distance = heading.magnitude;
        float originalDistance = distance;
        Vector3 direction = heading / distance;
        while (distance > originalDistance * 0.05f)
        {
            Debug.Log(distance);
            playerRigidbody.position += direction * TraslationSpeed.Value * Time.deltaTime;
            playerRigidbody.rotation = Quaternion.FromToRotation(Vector3.forward, targetNormal);
            CameraTargetPosition.Value = playerRigidbody.position;
            yield return null;
            heading = position - playerRigidbody.position;
            distance = heading.magnitude;
            direction = heading / distance;
        }
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
