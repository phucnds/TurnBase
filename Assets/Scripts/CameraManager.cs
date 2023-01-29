using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void Start()
    {
        BaseAction.OnAnyActionOnStarted += BaseAction_OnAnyActionOnStarted;
        BaseAction.OnAnyActionOnCompleted += BaseAction_OnAnyActionOnCompleted;

        HideActionCamera();
    }

    private void BaseAction_OnAnyActionOnStarted(BaseAction baseAction)
    {
        switch(baseAction)
        {
            case ShootAction shootAction:

                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeight = Vector3.up * 1.7f;
                Vector3 shootDir = (targetUnit.GetWorldPostition() - shooterUnit.GetWorldPostition()).normalized;
                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0 ,90 ,0) * shootDir * shoulderOffsetAmount;
                Vector3 actionCameraPosition = shooterUnit.GetWorldPostition() + cameraCharacterHeight + shoulderOffset + shootDir * -1;

                actionCameraGameObject.transform.position = actionCameraPosition;
                actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPostition()); 

                ShowActionCamera();
                break;
            
        }
    }

    private void BaseAction_OnAnyActionOnCompleted(BaseAction baseAction)
    {
        switch (baseAction)
        {
            case ShootAction:
                HideActionCamera();
                break;

        }
    }

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }
}
