using UnityEngine;

public class Collision : MonoBehaviour
{
    [SerializeField] GameObject ship;
    [SerializeField] Camera cam;
    CameraControl camC;

    public ExplosionScript explode;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy")
        {
            cam = Camera.main; //shake shake shake shake shake your bodyyy shake your bodyyyy
            camC = cam.GetComponent<CameraControl>();
            camC.StartCoroutine(camC.Shaking());

            explode.Do();

            ship.SetActive(false);
            //ship.
        }
    }
}
