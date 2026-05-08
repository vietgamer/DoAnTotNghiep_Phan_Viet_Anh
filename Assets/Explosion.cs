using UnityEngine;

public class Explosion : MonoBehaviour
{
    public ParticleSystem explosionFX;
    public float explosionForce = 10f;
    public float explosionRadius = 5f;
    public LayerMask affectedLayers;

    public void TriggerExplosion()
    {
        if (explosionFX)
            explosionFX.Play();

        Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius, affectedLayers);

        foreach (Collider obj in hitObjects)
        {
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, 1f, ForceMode.Impulse);
            }
        }

        Destroy(gameObject, 2f); // tự xóa object nổ sau 2s
    }
}
