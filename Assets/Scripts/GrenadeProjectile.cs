using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{

    private Vector3 targetPosition;
    private Action onGrenadeBehaviorComplete;

    public static event EventHandler OnAnyGrenadeExploded;
    [SerializeField] private Transform grenadeExplodeVfxPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;
    private float totalDistance;
    private Vector3 positionXZ;
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        this.onGrenadeBehaviorComplete=onGrenadeBehaviorComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;
        float moveSpeed = 15f;
        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(positionXZ,targetPosition);
        float distanceNormalized = 1 - distance / totalDistance;
        // distance goingto get smaller, we want 1 -> 0 so we inverted

        float maxHeight = totalDistance / 4f;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        float reachedTargetDistance = 2f;

        if(Vector3.Distance(positionXZ,targetPosition) < reachedTargetDistance){
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius);
            foreach (Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<Unit>(out Unit targetUnit)){
                    targetUnit.Damage(30);
                }
                if(collider.TryGetComponent<DestructableCrate>(out DestructableCrate destructableCrate)){
                    destructableCrate.Damage();
                }
            }
            OnAnyGrenadeExploded?.Invoke(this,EventArgs.Empty);
            trailRenderer.transform.parent = null;
            Instantiate(grenadeExplodeVfxPrefab, targetPosition + Vector3.up * 1f , Quaternion.identity);
            Destroy(gameObject);
            onGrenadeBehaviorComplete();
        }
    }
}
