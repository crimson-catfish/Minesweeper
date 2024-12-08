using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MainCamera : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;

    private void OnEnable()
    {
        this.transform.position =
            new Vector3((float)mapManager.width / 2, (float)mapManager.height / 2, transform.position.z);

        GetComponent<Camera>().orthographicSize = (float)mapManager.height / 2;
    }
}