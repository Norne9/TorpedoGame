using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Torpedo : MonoBehaviour {
    private SignalBus _signalBus;
    private TorpedoManager _manager;
    private Boat _boat;

    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private float _speed = 0;
    private int _health = 0;
    
    [Inject]
    public void Construct(SignalBus signalBus, TorpedoManager manager, Boat boat) {
        _signalBus = signalBus;
        _manager = manager;
        _boat = boat;
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        var dir = (_boat.Position - (Vector2)_transform.position).normalized;
        var angle = Mathf.Atan2(-dir.x, dir.y) * Mathf.Rad2Deg;
        _rigidbody.rotation = Mathf.MoveTowardsAngle(_rigidbody.rotation, angle,
            _speed * 180f * Time.smoothDeltaTime);
        _rigidbody.velocity = _transform.up * _speed;
    }

    public void TakeDamage(int damage) {
        _health -= damage;
        if (_health <= 0) {
            _manager.Despawn(this);
            _signalBus.Fire(new TorpedoDestroedSignal());
        }
    }

    private void ResetTorpedo(Vector2 pos, float speed) {
        _health = Random.Range(2, 4);
        _speed = speed;
        _transform.position = pos;
        _rigidbody.rotation = Random.Range(0f, 360f);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        var boat = other.gameObject.GetComponent<Boat>();
        if (boat != null) {
            boat.Kill();
        }
    }

    public class Pool: MonoMemoryPool<Vector2, float, Torpedo> {
        protected override void Reinitialize(Vector2 pos, float speed, Torpedo torpedo) {
            torpedo.ResetTorpedo(pos, speed);
        }
    }
}
