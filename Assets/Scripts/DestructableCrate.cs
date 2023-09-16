using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestructableCrate : MonoBehaviour
{

    [SerializeField] private Transform createDestroyedPrefab;
    public static event EventHandler OnAnyDestroyed;
    private GridPosition gridPosition;

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    }
    public GridPosition GetGridPosition() => gridPosition;
    public void Damage()
    {
        Transform tt = Instantiate(createDestroyedPrefab,transform.position,transform.rotation);

        ApplyExplosionToChildren(tt,150f,transform.position,10f);

        Destroy(gameObject);
        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

     private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange){
         foreach (Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody)){
                childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
            }

            ApplyExplosionToChildren(child,explosionForce, explosionPosition, explosionRange);

        }
    }


}
