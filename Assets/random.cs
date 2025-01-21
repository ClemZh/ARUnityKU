using UnityEngine;
using UnityEngine.XR.Management;

public class XRInitializer : MonoBehaviour
{
    void Start()
    {
        XRGeneralSettings.Instance.Manager.InitializeLoaderSync();
        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Failed to initialize XR Loader.");
        }
        else
        {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("XR Loader initialized successfully.");
        }
    }
}

