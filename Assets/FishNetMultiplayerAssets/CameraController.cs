using Cinemachine;
using FishNet.Object;
using UnityEngine;

public class CameraController : NetworkBehaviour
{
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            Camera c = Camera.main;

            CinemachineVirtualCamera vc = c.GetComponent<CinemachineVirtualCamera>();

            vc.Follow = transform;
            vc.LookAt = transform;
        }
    }
}
