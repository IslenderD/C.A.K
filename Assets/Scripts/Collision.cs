using UnityEngine;
using System.Collections;
using System;

public class Collision : MonoBehaviour
{
    [SerializeField] GameObject ship;
    [SerializeField] Camera cam;
    CameraControl camC;

    public SceneChanger changeScene;

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

            StartCoroutine(ChangeSceneAfter());
        }
    }

    IEnumerator ChangeSceneAfter()
    {
        yield return new WaitForSeconds(0.4f);
        changeScene.Change("StartMenu");
    } 
}
