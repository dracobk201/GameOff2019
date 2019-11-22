using System.Collections;
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
    [SerializeField]
    private GameEvent PlayerReachGoal;
    private Rigidbody playerRigidbody;
    private bool isGrounded;
    private bool isGameOver;
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
        if (!isGrounded || isGameOver)
            return;
        Vector3 targetPosition = playerRigidbody.position + new Vector3(HorizontalAxis.Value, 0, VerticalAxis.Value) * Time.deltaTime;
        playerRigidbody.MovePosition(targetPosition);
    }

    public void Rotate()
    {
        if (isGameOver)
            return;
        Vector3 playerRotation = Vector3.zero;
        playerRotation.y = RotationSpeed.Value * MouseHorizontalAxis.Value * Time.deltaTime;
        playerRigidbody.MoveRotation(playerRigidbody.rotation * Quaternion.Euler(playerRotation));
    }

    public void Shoot()
    {
        if (isGameOver)
            return;
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
        Vector3 originalPosition = playerRigidbody.position;
        Vector3 originalRotation = playerRigidbody.rotation.eulerAngles;

        Vector3 heading = position - playerRigidbody.position;
        float distance = heading.magnitude;
        float originalDistance = distance;

        Vector3 headingRotation = targetNormal - playerRigidbody.rotation.eulerAngles;
        float distanceRotation = headingRotation.magnitude;
        float originalDistanceRotation = distanceRotation;

        while (distance > originalDistance * 0.05f)
        {
            float newX = EasingFunction.Linear(originalPosition.x, position.x, distance / originalDistance);
            float newY = EasingFunction.Linear(originalPosition.y, position.y, distance / originalDistance);
            float newZ = EasingFunction.Linear(originalPosition.z, position.z, distance / originalDistance);
            playerRigidbody.position = new Vector3(newX, newY, newZ);
            newX = EasingFunction.Linear(originalRotation.x, targetNormal.x, distanceRotation / originalDistanceRotation);
            newY = EasingFunction.Linear(originalRotation.y, targetNormal.y, distanceRotation / originalDistanceRotation);
            newZ = EasingFunction.Linear(originalRotation.z, targetNormal.z, distanceRotation / originalDistanceRotation);
            playerRigidbody.rotation = Quaternion.FromToRotation(Vector3.forward, new Vector3(newX, newY, newZ));
            CameraTargetPosition.Value = playerRigidbody.position;
            yield return null;
            heading = position - playerRigidbody.position;
            distance = heading.magnitude;
            headingRotation = targetNormal - playerRigidbody.rotation.eulerAngles;
            distanceRotation = headingRotation.magnitude;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (var item in collision.contacts)
        {
            if (item.otherCollider.CompareTag(Global.GROUNDTAG))
                isGrounded = true;
            else if (item.otherCollider.CompareTag(Global.GOALTAG))
            {
                PlayerReachGoal.Raise();
                isGameOver = true;
            }
        }
    }
}
