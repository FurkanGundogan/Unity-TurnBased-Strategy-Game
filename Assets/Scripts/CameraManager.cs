using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject actionCameraGameObject;

    private void ShowActionCamera()
    {
        actionCameraGameObject.SetActive(true);
    }

    private void HideActionCamera()
    {
        actionCameraGameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;

        HideActionCamera();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
            Unit shooterUnit = shootAction.GetUnit();
            Unit targetUnit = shootAction.GetTargetUnit();
            Vector3 cameraCharacterHeight = Vector3.up  * 1.7f;
            Vector3 shootDir = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;
            float shoulderOffsetAmount = 0.5f;
            Vector3 shoulderOffset = Quaternion.Euler(0,90,0) * shootDir * shoulderOffsetAmount;
            Vector3 actionCameraPosition = 
                shooterUnit.GetWorldPosition() + 
                cameraCharacterHeight + 
                shoulderOffset+
                (shootDir * -1);

            actionCameraGameObject.transform.position = actionCameraPosition;
            actionCameraGameObject.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeight);

            ShowActionCamera();
            break;
           
        }
    }

    
    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
         switch (sender)
        {
            case ShootAction shootAction:
            HideActionCamera();
            break;
           
        }
    }
}
