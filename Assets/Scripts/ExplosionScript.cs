using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public void Do()
    {
        Debug.Log("Explode");
        GetComponent<ParticleSystem>().Play();
        ParticleSystem.EmissionModule em = GetComponent<ParticleSystem>().emission;
        em.enabled = true;


    }
}
