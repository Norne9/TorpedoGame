using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FloorMover : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float step = 10f;
    private Transform _transform;

    private void Start() {
        _transform = transform;
    }

    private void LateUpdate() {
#if UNITY_EDITOR
        if (target == null) return;
#endif
        var targetPosition = target.position;
        targetPosition.z = _transform.position.z;
        targetPosition.x = Mathf.Floor(targetPosition.x / step) * step;
        targetPosition.y = Mathf.Floor(targetPosition.y / step) * step;
        _transform.position = targetPosition;
    }
}
