using UnityEngine;
using UnityEngine.U2D;

[RequireComponent(typeof(Camera), typeof(PixelPerfectCamera))]
public class MainCamera : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;

    private void OnEnable()
    {
        this.transform.position =
            new Vector3((float)mapManager.width / 2, (float)mapManager.height / 2, transform.position.z);

        PixelPerfectCamera pixelPerfectCamera = GetComponent<PixelPerfectCamera>();
        pixelPerfectCamera.refResolutionX = mapManager.width * pixelPerfectCamera.assetsPPU;
        pixelPerfectCamera.refResolutionY = mapManager.height * pixelPerfectCamera.assetsPPU;
    }
}