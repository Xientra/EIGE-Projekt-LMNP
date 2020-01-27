using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicalheroScene : MonoBehaviour {

    [SerializeField]
    private Transform firstRange;
    [SerializeField]
    private Transform secondRange;
    [SerializeField]
    private Transform thirdRange;

    private List<GameObject> visibleKeys = new List<GameObject>();

    private int penalty;
    private int penaltyPoints = 0;

    private void OnTriggerEnter(Collider other) {
        visibleKeys.Add(other.gameObject);
    }


    private void OnTriggerExit(Collider other) {
        visibleKeys.Remove(other.gameObject);
        penaltyPoints += penalty;
        //Destroy(other.gameObject); //pls ask paul why
    }

    public List<GameObject> GetVisibleKeys() {
        return visibleKeys;
    }

    public int getPenaltyPoints() {
        int points = penaltyPoints;
        penaltyPoints = 0;
        return points;
    }

    public void SetPenalty(int penalty) {
        this.penalty = penalty;
    }

    private float[] CalculateLimits(Transform range) {

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
