using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameCamera : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float toleration = 1f;
    private Transform _transform;

    private void Start() {
        _transform = transform;
    }

    private void LateUpdate() {
#if UNITY_EDITOR
        if (target == null) return;
#endif
        var targetPosition = target.position + offset;
        var diff = Vector3.ClampMagnitude(_transform.position - targetPosition, toleration);
        _transform.position = diff + targetPosition;
    }
}
