using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
    private int maxThrowDistance = 7;
    
    [SerializeField]private Transform grenadeProjectilePrefab ;
    public override string GetActionName()
    {
        return "Grenade";
    }

    private void Update() {
        if(!isActive) return;
        
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction{
            gridPosition = gridPosition,
            actionValue = 0
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        
        List<GridPosition> validGridPositionList = new List<GridPosition>();      
        GridPosition unitGridPosition = unit.GetGridPosition();
        
        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                // unavailable positions
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition)) continue;


                float testDistance = MathF.Abs(x) + Mathf.Abs(z);
                if (testDistance > maxThrowDistance)
                {
                    continue;
                }
              
                validGridPositionList.Add(testGridPosition);
            }
        }



        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Transform grenadeProjectileTransform = Instantiate(grenadeProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviorComplete);
        
        ActionStart(onActionComplete);
    }

    private void OnGrenadeBehaviorComplete(){
        ActionComplete();
    }
}
