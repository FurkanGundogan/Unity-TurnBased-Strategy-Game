using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;
    [SerializeField]
    private LayerMask mousePlanePlayerMask;

    private void Awake() {
        instance = this;
    }

    public static Vector3 GetPosition(){
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());
        Physics.Raycast(ray,out RaycastHit raycastHit,float.MaxValue,instance.mousePlanePlayerMask);
        return raycastHit.point;
    }
}
