using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Camera), typeof(PixelPerfectCamera))]
public class MainCamera : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;

    public void SetUpCamera()
    {
        this.transform.position =
            new Vector3((float)mapManager.Width / 2, (float)mapManager.Height / 2, transform.position.z);

        PixelPerfectCamera pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        pixelPerfectCamera.refResolutionX = mapManager.Width * pixelPerfectCamera.assetsPPU;
        pixelPerfectCamera.refResolutionY = mapManager.Height * pixelPerfectCamera.assetsPPU;
    }
}