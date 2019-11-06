﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField]
    private FloatReference RotationSpeed;
    [SerializeField]
    private FloatReference BoundVerticalRotation;
    [SerializeField]
    private FloatReference MouseHorizontalAxis;
    [SerializeField]
    private FloatReference MouseVerticalAxis;

    [SerializeField]
    private Vector3Reference CameraTargetPosition;
    [SerializeField]
    private FloatReference SmoothSpeed;
    [SerializeField]
    private Vector3Reference Offset;

    [SerializeField]
    private FloatReference ShakeDuration;
    [SerializeField]
    private FloatReference ShakeMagnitude;

    private Vector3 lastTargetPosition;
    private bool isShaking;

    private void LateUpdate()
    {
        if (isShaking)
            return;
        Vector3 desiredPosition = CameraTargetPosition.Value + Offset.Value;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed.Value);
        transform.position = smoothedPosition;
    }

    public void RotateView()
    {
        Vector3 cameraRotation = Vector3.zero;
        cameraRotation.x = RotationSpeed.Value * MouseVerticalAxis.Value * Time.deltaTime;
        cameraRotation.y = RotationSpeed.Value * MouseHorizontalAxis.Value * Time.deltaTime;
        transform.localEulerAngles += cameraRotation;

        //Vector3 tmpRotationValidator = transform.localEulerAngles;
        //tmpRotationValidator.y = Mathf.Clamp(tmpRotationValidator.x, -BoundVerticalRotation.Value, BoundVerticalRotation.Value);
        //transform.localEulerAngles = tmpRotationValidator;
    }

    public void InitShake()
    {
        StartCoroutine(CameraShake());
    }

    private IEnumerator CameraShake()
    {
        Vector3 originalPosition = Camera.main.transform.localPosition;
        float elapsed = 0f;
        isShaking = true;

        while (elapsed < ShakeDuration.Value)
        {
            float x = Random.Range(-1f, 1f) * ShakeMagnitude.Value;
            float z = Random.Range(-1f, 1f) * ShakeMagnitude.Value;

            Camera.main.transform.localPosition = new Vector3(x, Camera.main.transform.localPosition.y, z);
            elapsed += Time.deltaTime;

            yield return null;
        }

        isShaking = false;
        Camera.main.transform.localPosition = originalPosition;
    }


}