using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalheroScene : MonoBehaviour {

    [SerializeField]
    private GameObject firstRange;
    [SerializeField]
    private GameObject secondRange;
    [SerializeField]
    private GameObject thirdRange;

    private List<GameObject> visibleKeys = new List<GameObject>();

    private void OnTriggerEnter(Collider other) {
        //Debug.Log("Key entered: " + other.gameObject.ToString());
        visibleKeys.Add(other.gameObject);
    }


    private void OnTriggerExit(Collider other) {
        visibleKeys.Remove(other.gameObject);
        //Debug.Log("Key left: " + other.gameObject.ToString());

        MusicalheroScore.Instance.DeductPoints();
        Destroy(other.gameObject);
    }

    public List<GameObject> GetVisibleKeys() {
        return visibleKeys;
    }

    private float[] CalculateLimits(GameObject range) {

        Renderer r = range.GetComponent<Renderer>();
        float upper = r.bounds.center.y + r.bounds.extents.y;
        float lower = r.bounds.center.y - r.bounds.extents.y;

        return new float[] { upper, lower };
    }

    public float[] GetFirstZone() {
        return CalculateLimits(firstRange);
    }

    public float[] GetSecondZone() {
        return CalculateLimits(secondRange);
    }

    public float[] GetThirdZone() {
        return CalculateLimits(thirdRange);
    }
}
