using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoDrawForDifficulty : MonoBehaviour
{
    [SerializeField] private Color32 _gizmoColor;
    [SerializeField] private Vector3 _boxSize;


    private void OnDrawGizmos()
    {
        _gizmoColor.a = 150;
        Gizmos.color = _gizmoColor;
        Gizmos.DrawCube(transform.position, _boxSize);
    }
}
