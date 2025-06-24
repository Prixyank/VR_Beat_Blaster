using UnityEngine;

public class enemy : MonoBehaviour
{
    public SimpleShoot shooter;
    public float shootTriggerTime = 0.4f; // Time in animation when gun should shoot
    public float triggerUncertainty = 0.1f; // Allowed margin
    public string shootAnimationName = "Shoot";
    
    private Animator animator;
    private bool hasShotThisCycle = false;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (shooter != null)
        {
            shooter.maxAmmo = 100;
        }
        else
        {
            Debug.LogWarning("Shooter is not assigned on enemy.");
        }
    }

    void Update()
    {
        // Face the camera
        if (Camera.main != null)
        {
            Vector3 delta = shooter.transform.position - transform.position;
            transform.forward = Vector3.ProjectOnPlane(Camera.main.transform.position  - delta - transform.position, Vector3.up).normalized;
           
        }

        HandleShootingByAnimation();
    }

    void HandleShootingByAnimation()
    {
        if (animator == null) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!stateInfo.IsName(shootAnimationName)) {
            hasShotThisCycle = false; // Reset for next time Shoot anim starts
            return;
        }

        float currentLoopTime = (stateInfo.normalizedTime % 1f) * stateInfo.length;
        float lowerBound = shootTriggerTime - triggerUncertainty;
        float upperBound = shootTriggerTime + triggerUncertainty;

        if (!hasShotThisCycle && currentLoopTime >= lowerBound && currentLoopTime <= upperBound)
        {
            Shoot();
            hasShotThisCycle = true;
        }

        // Reset flag if animation loop finished
        if (currentLoopTime < lowerBound)
        {
            hasShotThisCycle = false;
        }
    }

    void Shoot()
    {
        if (shooter != null)
        {
            shooter.Shoot();
        }
    }

    public void Dead(Vector3 hitPoint)
    {
        if (animator != null) animator.enabled = false;

        SetupRagdoll(false);

        foreach (var item in Physics.OverlapSphere(hitPoint, 0.5f))
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(1000f, hitPoint, 0.5f);
            }
        }

        Destroy(gameObject, 5f); // Destroy after 5 seconds
        this.enabled = false;
    }

    void SetupRagdoll(bool isAnimated)
    {
        foreach (var body in GetComponentsInChildren<Rigidbody>())
            body.isKinematic = isAnimated;

        foreach (var col in GetComponentsInChildren<Collider>())
            col.enabled = !isAnimated;

        var mainCol = GetComponent<Collider>();
        if (mainCol != null) mainCol.enabled = isAnimated;

        var mainRB = GetComponent<Rigidbody>();
        if (mainRB != null) mainRB.isKinematic = !isAnimated;
    }
}
