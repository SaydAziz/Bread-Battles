using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleShotGun : Gun
{
    [SerializeField] Camera cam;
    [SerializeField] Animator animator;
    [SerializeField] Animator FPS_animator;

    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] ParticleSystem FPS_muzzleFlash;

    [SerializeField] AudioSource ShotSound;

    PhotonView PV;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    public override void Use()
    {
        Shoot();
    }

    void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;

        animator.SetTrigger("Shoot");
        FPS_animator.SetTrigger("Shoot");


        PV.RPC("RPC_GunShot", RpcTarget.All);
        FPS_muzzleFlash.Play();  
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);          
        }
    }

    [PunRPC]
    void RPC_Shoot(Vector3 hitPosition, Vector3 hitNormal)
    {
        Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);
        if(colliders.Length != 0)
        {
            GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
            Destroy(bulletImpactObj, 10f);
            bulletImpactObj.transform.SetParent(colliders[0].transform);        
        }
        
    }

    [PunRPC]
    void RPC_GunShot()
    {
        muzzleFlash.Play();
        ShotSound.Play();   
    }


}
