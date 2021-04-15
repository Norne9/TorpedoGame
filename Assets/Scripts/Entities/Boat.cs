using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public float speed;
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
