using UnityEngine;
using System.Collections.Generic;

public class LineClearing : MonoBehaviour
{
    private ObjectSlicer objectSlicer = new ObjectSlicer();

    public void ClearLine(int rowNum, List<GameObject> detectedObjects)
    {
        List<GameObject> slicedObjects = new List<GameObject>();
        List<GameObject> editedObjects = new List<GameObject>();

        foreach(GameObject detectedObject in detectedObjects)
        {
            GameObject rootObject = detectedObject.transform.root.gameObject;
            GameObject cubeParent = rootObject.transform.GetChild(0).gameObject;
            GameObject detectedObjectMesh = GetLastChild(rootObject);

            detectedObjectMesh.GetComponent<Rigidbody>().isKinematic = true;
            detectedObjectMesh.GetComponent<MeshCollider>().enabled = true;

            // Only cut the mesh once
            if(!slicedObjects.Contains(rootObject))
            {
                Debug.Log("Slicing " + detectedObjectMesh.name);
                objectSlicer.SliceObject(rowNum + 0.9f, detectedObjectMesh);
                slicedObjects.Add(rootObject);
            }

            // Disable the mesh and softbody cube in the detected row
            GetLastChild(rootObject).SetActive(false);
            detectedObject.SetActive(false);

            if(cubeParent.transform.childCount == 0)
            {
                Destroy(rootObject);
            }
            else
            {
                GameObject newMesh = GetFirstChild(GetLastParent(rootObject));
                GameObject rootBone = GetFirstActiveChild(rootObject.transform.GetChild(0).gameObject).transform.GetChild(0).gameObject;

                Debug.Log("Editing " + newMesh.name + " from " + rootObject.name);
                Debug.Log(rootBone.name);

                // newMesh.AddComponent<Slice>();
                // newMesh.GetComponent<Rigidbody>().isKinematic = true;
                newMesh.GetComponent<MeshCollider>().enabled = false;
                // newMesh.GetComponent<MeshCollider>().convex = false;
                newMesh.GetComponent<Rigidbody>().useGravity = false;

                newMesh.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

                /// CHECK IF ROOT OBJECT HAS GONE THROUGH THIS ALREADY WITH ANOTHER LIST
                ///// Have the mesh render with the softbody cubes
                if(!editedObjects.Contains(rootObject))
                {
                    newMesh.transform.position += Vector3.down;
                    // SkinnedMeshRenderer newSkinnedMesh = newMesh.AddComponent<SkinnedMeshRenderer>();
                    // newSkinnedMesh.rootBone = rootBone.transform;
                    // newSkinnedMesh.sharedMaterial = newMesh.GetComponent<MeshRenderer>().sharedMaterial;

                    // FixedJoint meshJoint = newMesh.AddComponent<FixedJoint>();
                    // meshJoint.connectedBody = cubeParent.transform.GetChild(0).gameObject.GetComponent<Rigidbody>();

                    editedObjects.Add(rootObject);
                }
            }
        }
    }

    private GameObject GetLastParent(GameObject currentObject)
    {
        return GetLastChild(currentObject).transform.parent.gameObject;
    }

    private GameObject GetFirstChild(GameObject currentObject)
    {
        return currentObject.transform.GetChild(0).gameObject;
    }

    private GameObject GetLastChild(GameObject currentObject)
    {
        while(currentObject.transform.childCount != 0)
        {
            int numChildren = currentObject.transform.childCount;
            currentObject = currentObject.transform.GetChild(numChildren - 1).gameObject;
        }

        return currentObject;
    }

    private GameObject GetFirstActiveChild(GameObject currentObject)
    {
        for(int i = 0; i < currentObject.transform.childCount; i++)
        {
            GameObject currentChild = currentObject.transform.GetChild(i).gameObject;

            if(currentChild.activeSelf)
            {
                return currentChild;
            }
        }

        return null;
    }
}
