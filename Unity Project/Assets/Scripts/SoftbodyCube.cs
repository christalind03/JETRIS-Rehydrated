using UnityEngine;

public class SoftbodyCube : MonoBehaviour
{
    [Header("Collider Settings")]
    [SerializeField] [Tooltip("Collider size of each vertex")] private float _colliderSize = 0.01f;

    [Header("Spring Settings")]
    [SerializeField] [Tooltip("Higher the value, the tighter the spring")] private float _springStrength = 100f;
    [SerializeField] [Tooltip("Higher the value, the faster the spring oscillates")] private float _damperStrength = 0.05f;

    void Start()
    {
        AddRigidbodies();
        AddColliders();
        AddSpringJoints();
    }

    private void AddRigidbodies()
    {
        Transform vertexParent = transform.GetChild(0);

        foreach(Transform vertex in vertexParent)
        {
            GameObject vertexObject = vertex.gameObject;
            Rigidbody vertexRigidbody = vertexObject.AddComponent<Rigidbody>();

            vertexRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            vertexRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }

    private void AddColliders()
    {
        Transform vertexParent = transform.GetChild(0);

        foreach(Transform vertex in vertexParent)
        {
            GameObject vertexObject = vertex.gameObject;
            BoxCollider vertexCollider = vertexObject.AddComponent<BoxCollider>();

            vertexCollider.size = new Vector3(_colliderSize, _colliderSize, _colliderSize);
        }
    }

    private void AddSpringJoints()
    {
        Transform vertexParent = transform.GetChild(0);

        foreach(Transform vertex in vertexParent)
        {
            foreach(Transform vertexSibling in vertexParent)
            {
                if(vertex != vertexSibling)
                {
                    GameObject vertexObject = vertex.gameObject;
                    GameObject vertexSiblingObject = vertexSibling.gameObject;
                    SpringJoint vertexSpring = vertexObject.AddComponent<SpringJoint>();

                    vertexSpring.connectedBody = vertexSiblingObject.GetComponent<Rigidbody>();
                    vertexSpring.spring = _springStrength;
                    vertexSpring.damper = _damperStrength;
                }
            }
        }
    }
}