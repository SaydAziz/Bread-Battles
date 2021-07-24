using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Network : MonoBehaviourPun, IPunObservable
{
     private Vector3 realPos;
     private Vector3 lastPos;
     private Vector3 velocity;
     private Quaternion realRot;
     private float predictionCoeff = 1f;
    void Update()
    {
        lastPos = realPos;
        if(!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, realPos + (predictionCoeff * velocity * Time.deltaTime), Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, realRot, Time.deltaTime);
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting && photonView.IsMine)
        {
            //
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //
            stream.SendNext((realPos - lastPos) / Time.deltaTime);
        }
        else
        {
            //
            realPos = (Vector3) (stream.ReceiveNext());
            realRot = (Quaternion) (stream.ReceiveNext());
            //
            velocity = (Vector3) (stream.ReceiveNext());
        }
    }
}
