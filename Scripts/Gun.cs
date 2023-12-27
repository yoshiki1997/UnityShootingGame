using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum FireMode{Auto, Burst, Single};
    public FireMode fireMode;

    public Transform[] projectileSpawn;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;
    public int burstCount;
    public int projectilePerMag;
    public float reloadTime = .3f;

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2 ( .05f, .2f);
    public Vector2 recoilAngleMinMax = new Vector2 (3, 5);
    public float recoilMoveSettelTime = .1f;
    public float recoilRotationSettelTime = .1f;

    [Header("Effect")]
    public Transform shell;
    public Transform shellEjection;
    public AudioClip shootAudio;
    public AudioClip reloadAudio;
    MuzzleFlash muzzleFlash;
    float nextShotTime;
    int shotRemainingInBurst;
    int projectileRemainingInMag;
    bool isReloading;

    bool triggerReleaseLastShot;

    Vector3 recoilSmoothDampVerocity;
    float recoilRotSmoothDampVerocity;
    float recoilAngle;

    void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash> ();
        shotRemainingInBurst = burstCount;
        projectileRemainingInMag = projectilePerMag;
    }

    void LateUpdate()
    {
        // animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVerocity, recoilMoveSettelTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVerocity, recoilRotationSettelTime);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

        if (!isReloading && projectileRemainingInMag == 0)
        {
            Reload();
        }
    }

    void Shoot()
    {
        if (!isReloading && Time.time > nextShotTime  && projectileRemainingInMag > 0)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shotRemainingInBurst == 0)
                {
                    return;
                }
                shotRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleaseLastShot)
                {
                    return;
                }
            }

            for (int i = 0; i < projectileSpawn.Length; i++)
            {
                if (projectileRemainingInMag == 0)
                {
                    break;
                }
                projectileRemainingInMag --;
                nextShotTime = Time.time + msBetweenShots / 1000;
                Projectile newProjectile = Instantiate(projectile, projectileSpawn[i].position, projectileSpawn[i].rotation) as Projectile;
                newProjectile.SetSpeed (muzzleVelocity);
            }

            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();

            transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);

            AudioManager.instance.PlaySound(shootAudio, transform.position);
        }
    }

    public void Reload()
    {
        if (!isReloading && projectileRemainingInMag != projectilePerMag)
        {
            StartCoroutine(AnimateReload());
            AudioManager.instance.PlaySound(reloadAudio, transform.position);
        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(.2f);
        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 30f;

        while (percent < 1)
        {
            percent += reloadSpeed * Time.deltaTime;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;
            yield return null;
        }

        isReloading = false;
        projectileRemainingInMag = projectilePerMag;
    }

    public void Aim(Vector3 aimPoint)
    {
        if (!isReloading)
        {
            transform.LookAt (aimPoint);
        }
    }

    public void OnTriggerHold()
    {
        Shoot();
        triggerReleaseLastShot = false;
    }

    public void OnTriggerRelease()
    {
        triggerReleaseLastShot = true;
        shotRemainingInBurst = burstCount;
    }
}
