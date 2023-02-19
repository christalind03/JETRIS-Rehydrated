using UnityEngine;

public class ObjectSlicer
{
    public void MakeSlicable(GameObject sliceableObject)
    {
        SliceOptions sliceOptions = new SliceOptions();
        CallbackOptions callbackOptions = new CallbackOptions();
        Slice sliceScript = sliceableObject.AddComponent<Slice>();
        Mesh sliceableObjectMesh = sliceableObject.GetComponent<MeshCollider>().sharedMesh;
        Material sliceableObjectMaterial = sliceableObject.GetComponent<SkinnedMeshRenderer>().material;

        sliceOptions.enableReslicing = true;
        sliceOptions.maxResliceCount = 3;
        sliceOptions.insideMaterial = sliceableObjectMaterial;
        sliceScript.sliceOptions = sliceOptions;

        callbackOptions.onCompleted = null;
        sliceScript.callbackOptions = callbackOptions;

        sliceableObject.GetComponent<MeshFilter>().sharedMesh = sliceableObjectMesh;
        sliceableObject.GetComponent<MeshRenderer>().material = sliceableObjectMaterial;
    }

    public void SliceObject(float heightToCut, GameObject sliceableObject)
    {
        Slice slicer = sliceableObject.GetComponent<Slice>();
        Vector3 slicerNormal = Vector3.up;

        Vector3 sliceableObjectPosition = sliceableObject.transform.position;
        Vector3 slicerOrigin = new Vector3(sliceableObjectPosition.x, heightToCut, sliceableObjectPosition.z);

        slicer.ComputeSlice(slicerNormal, slicerOrigin);
    }
}
