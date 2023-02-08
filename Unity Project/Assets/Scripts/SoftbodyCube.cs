using System;
using UnityEngine;

public class SoftbodyCube : MonoBehaviour
{
    // RIGIDBODY SETTINGS
    private RigidbodyConstraints _rigidbodyConstraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    private CollisionDetectionMode _rigidbodyCollisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

    // COLLIDER SETTINGS
    private float _vertexColliderSize = 0.01f;
    private float _sensorColliderSize = 0.015f;

    // SPRING SETTINGS
    private float _springStrength = 100f;
    private float _damperStrength = 0.05f;

    void Awake()
    {
        AddVertexRigidbodies();
        AddVertexColliders();
        AddVertexSpringJoints();

        AddSensorRigidbody();
        AddSensorCollider();
        AddSensorSpringJoints();
    }

    private void AddVertexRigidbodies()
    {
        foreach(Transform vertex in this.transform)
        {
            GameObject vertexObject = vertex.gameObject;
            Rigidbody vertexRigidbody = vertexObject.AddComponent<Rigidbody>();

            vertexRigidbody.constraints = _rigidbodyConstraints;
            vertexRigidbody.collisionDetectionMode = _rigidbodyCollisionDetectionMode;
        }
    }

    private void AddVertexColliders()
    {
        foreach(Transform vertex in this.transform)
        {
            GameObject vertexObject = vertex.gameObject;
            BoxCollider vertexCollider = vertexObject.AddComponent<BoxCollider>();

            vertexCollider.size = new Vector3(_vertexColliderSize, _vertexColliderSize, _vertexColliderSize);
        }
    }

    private void AddVertexSpringJoints()
    {
        foreach(Transform vertex in this.transform)
        {
            foreach(Transform vertexSibling in this.transform)
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

    private void AddSensorRigidbody()
    {
        GameObject thisCube = this.gameObject;

        Rigidbody thisCubeRigidbody = thisCube.AddComponent<Rigidbody>();
        thisCubeRigidbody.constraints = _rigidbodyConstraints;
        thisCubeRigidbody.collisionDetectionMode = _rigidbodyCollisionDetectionMode;
    }

    private void AddSensorCollider()
    {
        GameObject thisCube = this.gameObject;
        Vector3 vertexOne = this.transform.GetChild(0).localPosition;
        Vector3 vertexTwo = this.transform.GetChild(7).localPosition;
        Vector3 vertexMidpoint = new Vector3(0, System.MathF.Round((vertexOne.y + vertexTwo.y) / 2, 2), System.MathF.Round((vertexOne.z + vertexTwo.z) / 2, 2));

        SphereCollider thisCubeSensor = thisCube.AddComponent<SphereCollider>();
        thisCubeSensor.radius = _sensorColliderSize;
        thisCubeSensor.center = vertexMidpoint;
    }

    private void AddSensorSpringJoints()
    {
        GameObject thisCube = this.gameObject;

        foreach(Transform vertex in this.transform)
        {
            GameObject vertexObject = vertex.gameObject;
            SpringJoint thisCubeSpring = thisCube.AddComponent<SpringJoint>();

            thisCubeSpring.connectedBody = vertexObject.GetComponent<Rigidbody>();
            thisCubeSpring.spring = _springStrength;
            thisCubeSpring.damper = _damperStrength;
        }
    }
}