using UnityEngine;

public class Projectile : MonoBehaviour {
    [SerializeField] private AudioClip _destroyClip;
    [SerializeField] private GameObject _particles;

    [SerializeField] private float speed;
    [SerializeField] private int hitcount = 3;

    [SerializeField] private float _explosionRadius = 5;
    [SerializeField] private float _explosionForce = 500;

    private Vector3 lastvelocity;
    private Rigidbody rb;

    private bool destoryed = true;
    private int hit;

    private Vector3 _dir;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        lastvelocity = rb.velocity;
    }

    public void Init(Vector3 dir) {

        GetComponent<Rigidbody>().AddForce(dir);
        //Invoke(nameof(DestroyBall), 3);

        
    }

    private void OnCollisionEnter(Collision col)
    {

        var surroundingObject = Physics.OverlapSphere(transform.position, _explosionRadius);

        foreach (var obj in surroundingObject)
        {
            var rb = obj.GetComponent<Rigidbody>();
            if (rb == null) continue;

            rb.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
        }

        var speed = lastvelocity.magnitude;
        var direction = Vector3.Reflect(lastvelocity.normalized, col.contacts[0].normal);

        rb.velocity = direction * Mathf.Max(speed, 0f);

        hit += 1;



        if (hit == hitcount)
        {
            destoryed = false;
        }

        if (destoryed == false)
        {
            DestroyBall();
        }

        if (col.gameObject.tag == "Egg")
        {
            col.gameObject.GetComponent<PlayerEgg>().Damaged();
        }


    }


    private void DestroyBall() {
        AudioSource.PlayClipAtPoint(_destroyClip, transform.position);
        Instantiate(_particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}