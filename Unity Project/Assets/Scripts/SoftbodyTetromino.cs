using UnityEngine;

public class SoftbodyTetromino : MonoBehaviour
{
    // SPRING SETTINGS
    private float _springStrength = 150f;
    private float _damperStrength = 5f;

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
            foreach(Transform cubeSibling in this.transform)
            {
                if(cube != cubeSibling)
                {
                    GameObject cubeObject = cube.gameObject;
                    GameObject cubeSiblingObject = cubeSibling.gameObject;

                    SphereCollider cubeSensor = cubeObject.GetComponent<SphereCollider>();
                    SphereCollider cubeSiblingSensor = cubeSibling.GetComponent<SphereCollider>();

                    bool hasSameY = cubeSensor.center.y == cubeSiblingSensor.center.y;
                    bool hasSameZ = cubeSensor.center.z == cubeSiblingSensor.center.z;

                    if(hasSameY || hasSameZ)
                    {
                        SpringJoint cubeSpring = cubeObject.AddComponent<SpringJoint>();

                        cubeSpring.connectedBody = cubeSibling.GetComponent<Rigidbody>();
                        cubeSpring.spring = _springStrength;
                        cubeSpring.damper = _damperStrength;
                    }
                }
            }
        }
    }
}
