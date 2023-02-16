using UnityEngine;

public class SoftbodyTetromino : MonoBehaviour
{
    // SPRING SETTINGS
    private float _springStrength = 125f;
    private float _damperStrength = 0.125f;

    void Start()
    {
        foreach(Transform vertex in this.transform)
        {
            vertex.gameObject.AddComponent<SoftbodyCube>();
        }

        ConnectCubes();
    }

    private void ConnectCubes()
    {
        // Only connects the sensors who are in direct neighbors of each other (i.e: above, below, left, and right)
        foreach(Transform cube in this.transform)
        {
            GameObject cubeObject = cube.gameObject;
            SphereCollider cubeSensor = cubeObject.GetComponent<SphereCollider>();

            int numConnections = 0;

            foreach(Transform cubeSibling in this.transform)
            {
                if(cube != cubeSibling)
                {
                    GameObject cubeSiblingObject = cubeSibling.gameObject;
                    SphereCollider cubeSiblingSensor = cubeSibling.GetComponent<SphereCollider>();

                    bool hasSameY = cubeSensor.center.y == cubeSiblingSensor.center.y;
                    bool hasSameZ = cubeSensor.center.z == cubeSiblingSensor.center.z;

                    if(hasSameY || hasSameZ)
                    {
                        SpringJoint cubeSpring = cubeObject.AddComponent<SpringJoint>();

                        cubeSpring.connectedBody = cubeSibling.GetComponent<Rigidbody>();
                        cubeSpring.spring = _springStrength;
                        cubeSpring.damper = _damperStrength;

                        numConnections++;
                    }
                }
            }

            // If the current cube only has one connection, increase the spring strength
            if(numConnections < 2)
            {
                SpringJoint[] cubeSprings = cubeObject.GetComponents<SpringJoint>();
                SpringJoint cubeSpring = cubeSprings[numConnections - 1];
                float strengthMultiplier = 2.25f;

                cubeSpring.spring *= strengthMultiplier;
                cubeSpring.damper *= strengthMultiplier;
                cubeSpring.tolerance = 1f;
            }
        }
    }
}