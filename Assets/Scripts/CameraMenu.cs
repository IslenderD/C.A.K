// Controls the camera transitions in the menu scene
using UnityEngine;
using Unity.Cinemachine;  

public class CameraMenu : MonoBehaviour
{
    [SerializeField] CinemachineCamera defaultCam, playCam, optionsCam, creditsCam;

    public void GoToDefault() => Activate(defaultCam);
    public void GoToPlay()    => Activate(playCam);
    public void GoToOptions() => Activate(optionsCam);
    public void GoToCredits() => Activate(creditsCam);

    void Activate(CinemachineCamera target)
    {
        foreach (var cam in new[] { defaultCam, playCam, optionsCam, creditsCam })
            cam.Priority = (cam == target) ? 20 : 10;
    }
}