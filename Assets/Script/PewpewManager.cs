using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PewpewManager : MonoBehaviour
{
    public static PewpewManager Instance { get; private set; }

    public GameObject cannonball;
    public Transform cannon;
    public float cannonCooldown;
    private float lastCannonFireTime;

    public GameObject missile;
    public Transform missileLauncher;
    public float missileCooldown;
    private float lastMissileFireTime;

    public GameObject bullet;
    public Transform machineGun;
    public float machineGunCooldown;
    public float overheatTime;
    public float overheatRecovery;
    private float overheatMeter;
    private bool overheated;
    private float lastMachineGunFireTime;

    private float percentCannonReload => 1 - (Time.time - lastCannonFireTime) / cannonCooldown;
    private float percentMissileReload => 1 - (Time.time - lastMissileFireTime) / missileCooldown;
    private float percentOverheat => overheatMeter / overheatTime;

    public Camera myCam;


    public static Quaternion shellRotation = Quaternion.Euler(-90f, 0f, 0f);

    public enum Weapon { MachineGun, Cannon, Missile }
    public Weapon currentWeapon = Weapon.MachineGun;

    void Awake()
    {


        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        lastCannonFireTime = Time.time - cannonCooldown;
        lastMachineGunFireTime = Time.time - machineGunCooldown;
        lastMissileFireTime = Time.time - missileCooldown;

    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentWeapon = (Weapon)(((int)currentWeapon + 1) % 3);
            string weaponName = "";

            switch(currentWeapon)
            {
                case Weapon.MachineGun: weaponName = "> Machine Gun <"; break;
                case Weapon.Cannon: weaponName = "> Tank cannon <"; break;
                case Weapon.Missile: weaponName = "> Missile Launcher <"; break;
            }
            UIManager.Instance.ChangeWeaponTo(weaponName);

            if (currentWeapon == Weapon.Cannon)
                UIManager.Instance.UpdateCoolingBar(percentCannonReload);
            else if (currentWeapon == Weapon.Missile)
                UIManager.Instance.UpdateCoolingBar(percentMissileReload);
        }

        bool shootingMachineGunWhileOverheated = currentWeapon == Weapon.MachineGun && overheated;
        if (Input.GetMouseButton(0) && !shootingMachineGunWhileOverheated)
        {
            switch (currentWeapon)
            {
                case Weapon.MachineGun:
                    if (!overheated && Time.time >= lastMachineGunFireTime + machineGunCooldown)
                    {
                        overheatMeter += machineGunCooldown;
                        if (overheatMeter > overheatTime)
                        {
                            overheatMeter = overheatTime;
                            overheated = true;
                            Debug.Log("Machine gun overheated!");
                            UIManager.Instance.Overheat();
                        }
                        UIManager.Instance.UpdateCoolingBar(percentOverheat);
                        FireMachineGun();
                        lastMachineGunFireTime = Time.time;
                    }

                    break;

                case Weapon.Cannon:
                    if (Time.time >= lastCannonFireTime + cannonCooldown)
                    {
                        UIManager.Instance.Reloading();
                        FireCannon();
                        lastCannonFireTime = Time.time;
                    }
                    UIManager.Instance.UpdateCoolingBar(percentCannonReload);
                    break;

                case Weapon.Missile:
                    if (Time.time >= lastMissileFireTime + missileCooldown)
                    {
                        UIManager.Instance.Reloading();
                        StartCoroutine(FireMissilesAtIntervals());
                        lastMissileFireTime = Time.time;
                    }
                    UIManager.Instance.UpdateCoolingBar(percentMissileReload);
                    break;
            }
        }
        else if (currentWeapon == Weapon.MachineGun)
        {
            overheatMeter -= Time.deltaTime;
            
            if (overheated && overheatMeter < overheatTime - overheatRecovery)
            {
                Debug.Log("Machine gun cooled down");
                UIManager.Instance.Cooled();
                overheated = false;
            }
            else if (overheated)
            {
                UIManager.Instance.Overheat();
            }
            else if (overheatMeter <= 0) overheatMeter = 0;
            UIManager.Instance.UpdateCoolingBar(percentOverheat);
        }
        else if (currentWeapon == Weapon.Cannon)
        {
            UIManager.Instance.UpdateCoolingBar(Mathf.Clamp01(percentCannonReload));
            if (percentCannonReload > 0) UIManager.Instance.Reloading(); 
            if (percentCannonReload <= 0) UIManager.Instance.Cooled();
        }
        else if (currentWeapon == Weapon.Missile)
        {
            UIManager.Instance.UpdateCoolingBar(Mathf.Clamp01(percentMissileReload)); 
            if (percentMissileReload > 0) UIManager.Instance.Reloading(); 
            if (percentMissileReload <= 0) UIManager.Instance.Cooled();
        }

    }

    void FireCannon()
    {
        Vector3 spawn = cannon.position - cannon.forward * 1.2f;
        Instantiate(cannonball, spawn, cannon.rotation * shellRotation);
        StartCoroutine(myCam.GetComponent<CameraShake>().Shake(0.2f, 10f)); 

        //Debug.Log("Cannon fired!");
    }

    IEnumerator FireMissilesAtIntervals()
    {
        for (int i = 0; i < 4; i++)
        {
            Vector3 spawn = missileLauncher.position - missileLauncher.forward * 0.4f;
            Instantiate(missile, spawn, missileLauncher.rotation * shellRotation);
            //Debug.Log("Missile fired!");
            StartCoroutine(myCam.GetComponent<CameraShake>().Shake(0.2f, 10f)); 
            yield return new WaitForSeconds(0.2f);

        }
    }

    void FireMachineGun()
    {
        Vector3 spawn = machineGun.position - machineGun.forward * 0.6f;
        Instantiate(bullet, spawn, machineGun.rotation * shellRotation);
        //Debug.Log("Machine gun firing!");
    }
}
