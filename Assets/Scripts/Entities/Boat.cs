using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class Boat : MonoBehaviour {
    [Serializable]
    public class WeaponSettings {
        public string name;
        public float fireTimeout;
        public Bullet.Settings bulletSettings;
    }

    [Serializable]
    public class Settings {
        public List<WeaponSettings> weapons;
        public float speed, rotationSpeed, acceleration;
    }

    public Vector2 Position => _transform.position;
    [SerializeField] private Transform firePoint;

    private SignalBus _signalBus;
    private Settings _settings;
    private GameInput _input;
    private Bullet.Pool _bulletPool;
    
    private Rigidbody2D _rigidbody;
    private Transform _transform;
    private bool _alive = true;
    private float _speed = 0f;
    private int _weaponIdx = 0;
    private float _lastFire = 0f;

    [Inject]
    public void Construct(SignalBus signalBus, Settings settings, GameInput input, Bullet.Pool bulletPool) {
        _signalBus = signalBus;
        _settings = settings;
        _input = input;
        _bulletPool = bulletPool;
        
        _rigidbody = GetComponent<Rigidbody2D>();
        _transform = transform;
        _signalBus.Fire(new WeaponChangedSignal() { NewWeapon = _settings.weapons[_weaponIdx].name});
    }

    // Update is called once per frame
    private void Update() {
        if (!_alive) return;
        
        _rigidbody.rotation -= _settings.rotationSpeed * Time.smoothDeltaTime * _input.Horizontal;
        _speed = Mathf.MoveTowards(_speed, _settings.speed * _input.Vertical,
            _settings.acceleration * Time.smoothDeltaTime);

        if (_input.SwapWeapon) {
            _weaponIdx += 1;
            _weaponIdx %= _settings.weapons.Count;
            _signalBus.Fire(new WeaponChangedSignal() { NewWeapon = _settings.weapons[_weaponIdx].name});
        }

        if (_input.Fire) {
            var activeWeapon = _settings.weapons[_weaponIdx];
            if (Time.time > activeWeapon.fireTimeout + _lastFire) {
                _lastFire = Time.time;
                var angle = -Mathf.Atan2(firePoint.up.x, firePoint.up.y) * Mathf.Rad2Deg;
                _bulletPool.Spawn(firePoint.position, angle, activeWeapon.bulletSettings);
            }
        }

        _rigidbody.velocity = _transform.up * _speed;
    }

    public void Kill() {
        _alive = false;
        _rigidbody.velocity = Vector2.zero;
        _signalBus.Fire(new PlayerKilledSignal());
    }

    public void Respawn() {
        _alive = true;
    }
}
