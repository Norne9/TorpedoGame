using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {
    [Serializable]
    public class Settings {
        public float size;
        public float speed;
        public float lifeTime;
        public int damage;
        public Color color;
    }
    
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private Renderer _renderer;
    private Pool _pool;
    private int _damage = 0;
    
    [Inject]
    public void Construct(Pool pool) {
        _pool = pool;
        _rigidbody = GetComponent<Rigidbody2D>();
        _renderer = GetComponentInChildren<Renderer>();
        _transform = transform;
    }

    private void ResetBullet(Vector2 pos, float rot, Settings settings) {
        _transform.position = pos;
        _transform.eulerAngles = Vector3.forward * rot;
        _rigidbody.velocity = transform.up * settings.speed;
        _damage = settings.damage;
        
        _transform.localScale = Vector3.one * settings.size;
        _renderer.material.color = settings.color;
        StartCoroutine(TimedDestroy(settings.lifeTime));
    }

    private void OnCollisionEnter2D(Collision2D other) {
        var torpedo = other.gameObject.GetComponent<Torpedo>();
        if (torpedo != null) {
            torpedo.TakeDamage(_damage);
            Remove();
        }
    }
    
    private IEnumerator TimedDestroy(float time) {
        yield return new WaitForSeconds(time);
        Remove();
    }
    private void Remove() {
        StopAllCoroutines();
        _pool.Despawn(this);
    }

    public class Pool: MonoMemoryPool<Vector2, float, Settings, Bullet> {
        protected override void Reinitialize(Vector2 pos, float rot, Settings settings, Bullet bullet) {
            bullet.ResetBullet(pos, rot, settings);
        }
    }
}
