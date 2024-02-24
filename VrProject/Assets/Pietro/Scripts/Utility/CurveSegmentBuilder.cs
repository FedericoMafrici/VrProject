using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveSegmentBuilder : MonoBehaviour {
    public List<Transform> controlPoints = new List<Transform>();
    public GameObject prefabToSpawn;
    public int numberOfObjects = 10; // Number of objects to spawn
    public float totalCurveLength = 0f; // Total length of the curve
    //public int resolution = 30;
    public float spacing = 0.5f;
    private Stack<GameObject> _spawnedStack = new Stack<GameObject>();
    private List<Vector3> _prevControlPositions = new List<Vector3>();
    private List<Vector3> pointsOnCurve = new List<Vector3>(); // List to store points on the curve
    [SerializeField] private bool _manuallySetPoints = false;
    int _prevResolution;
    float _prevSpacing;
    int _prevCount;
    int _prevChildCount;
    int _prevNumObjects;
    private void Start() {
        if (!_manuallySetPoints) {
            controlPoints.Clear();
            Transform[] children = transform.GetComponentsInChildren<Transform>();
            foreach(Transform child in children) {
                if (child.gameObject != this.gameObject)
                    controlPoints.Add(child);
            }
        }
        _prevNumObjects = numberOfObjects;
        _prevChildCount = transform.childCount;
        //_prevResolution = resolution;
        _prevSpacing = spacing;
        _prevCount = controlPoints.Count;
        foreach (Transform t in controlPoints) {
            _prevControlPositions.Add(t.position);
        }
        UpdateCurve();
    }

    private void Update() {
        bool different = false;

        if (_prevChildCount != transform.childCount) {
            controlPoints.Clear();
            different = true;
            Transform[] children = transform.GetComponentsInChildren<Transform>();
            foreach (Transform child in children) {
                if (child.gameObject != this.gameObject)
                    controlPoints.Add(child);
            }
            _prevChildCount = transform.childCount;
        }

        //control point removed or added
        if (_prevCount != controlPoints.Count) {
            different = true;
            if (_prevCount < controlPoints.Count) {
                for (int i = _prevCount; i < controlPoints.Count; i++) {
                    _prevControlPositions.Add(controlPoints[i].position);
                }
            } else {
                for (int i = _prevCount-1; i >= controlPoints.Count; i--) {
                    _prevControlPositions.RemoveAt(_prevControlPositions.Count-1);
                }
            }

            _prevCount = controlPoints.Count;
        }

        //control points moved
        for (int i = 0; i < controlPoints.Count; i++) {
            if (_prevControlPositions[i] != controlPoints[i].position) {
                different = true;
                _prevControlPositions[i] = controlPoints[i].position;
            }
        }

        //spacing changed
        if (_prevSpacing != spacing) {
            _prevSpacing = spacing;
            different = true;
        }

        /*
        //resolution changed
        if (_prevResolution != resolution) {
            _prevResolution = resolution;
            different = true;
        }
        */

        if (_prevNumObjects != numberOfObjects) {
            _prevNumObjects = numberOfObjects;
            different = true;
        }

        if (different) {
            while (_spawnedStack.Count > 0) {
                GameObject tmp = _spawnedStack.Pop();
                Destroy(tmp);
            }
            UpdateCurve();
        }
    }

    private void UpdateCurve() {
        // Previous code unchanged

        // Clear the list of points
        pointsOnCurve.Clear();

        // Calculate parameter step size
        float step = 1f / (numberOfObjects - 1);

        // Iterate along the curve with constant parameter step
        for (int i = 0; i < numberOfObjects; i++) {
            // Calculate parameter t for the current step
            float t = i * step;

            // Add the point on the curve to the list
            pointsOnCurve.Add(CalculatePointOnCurve(t));
        }

        // Spawn objects at the calculated points
        foreach (Vector3 point in pointsOnCurve) {
            SpawnObjectAtPoint(point);
        }
    }

    /*
    private void UpdateCurve() {
        // Previous code unchanged

        // Calculate the total length of the curve
        float totalLength = 0f;
        for (int i = 0; i < resolution - 1; i++) {
            float t1 = (float)i / (resolution - 1);
            float t2 = (float)(i + 1) / (resolution - 1);
            Vector3 p1 = CalculatePointOnCurve(t1);
            Vector3 p2 = CalculatePointOnCurve(t2);
            totalLength += Vector3.Distance(p1, p2);
        }

        // Adjust spacing based on total length
        float actualSpacing = spacing * totalLength;

        // Iterate along the curve with constant spacing
        float currentDistance = 0f;
        for (int i = 0; i < resolution; i++) {
            float t = (float)i / (resolution - 1);
            Vector3 point = CalculatePointOnCurve(t);

            // Adjust current distance
            currentDistance += actualSpacing;

            // Find the corresponding parameter t for the current distance
            float tAtDistance = FindTAtDistance(currentDistance, totalLength);

            // Spawn object at the calculated point
            SpawnObjectAtPoint(CalculatePointOnCurve(tAtDistance));
        }
    }
    */


    /*
    private void UpdateCurve() {
        if (controlPoints.Count < 2) {
            Debug.LogError("Please assign at least two control points.");
            return;
        }

        if (prefabToSpawn == null) {
            Debug.LogError("Please assign a prefab to spawn.");
            return;
        }

        for (int i = 0; i < resolution; i++) {
            float t = (float)i / (resolution - 1);
            Vector3 point = CalculatePointOnCurve(t);
            SpawnObjectAtPoint(point);
        }
    }
    */

    private float CalculateTotalCurveLength() {
        float length = 0f;
        for (int i = 0; i < numberOfObjects - 1; i++) {
            float t1 = (float)i / (numberOfObjects - 1);
            float t2 = (float)(i + 1) / (numberOfObjects - 1);
            Vector3 p1 = CalculatePointOnCurve(t1);
            Vector3 p2 = CalculatePointOnCurve(t2);
            length += Vector3.Distance(p1, p2);
        }
        return length;
    }

    private float FindTAtDistance(float distance, float totalLength) {
        float targetLength = distance / totalLength;
        float currentLength = 0f;
        float t1 = 0f;
        float t2 = 0f;

        for (int i = 0; i < numberOfObjects - 1; i++) {
            t1 = (float)i / (numberOfObjects - 1);
            t2 = (float)(i + 1) / (numberOfObjects - 1);
            Vector3 p1 = CalculatePointOnCurve(t1);
            Vector3 p2 = CalculatePointOnCurve(t2);
            float segmentLength = Vector3.Distance(p1, p2);
            currentLength += segmentLength;

            if (currentLength >= targetLength) {
                float remainingLength = currentLength - targetLength;
                float segmentRatio = remainingLength / segmentLength;
                return Mathf.Lerp(t1, t2, 1f - segmentRatio);
            }
        }

        return t2; // Default to end of curve
    }


    /*
    private Vector3 CalculatePointOnCurve(float t) {
        int n = controlPoints.Count - 1;
        Vector3 point = Vector3.zero;

        for (int i = 0; i <= n; i++) {
            float blend = CalculateBlendFunction(i, n, t);
            point += blend * controlPoints[i].position;
        }

        return point;
    }
    */


    private Vector3 CalculatePointOnCurve(float t)
{
    int n = controlPoints.Count - 1;
    Vector3 point = Vector3.zero;

    for (int i = 0; i <= n; i++)
    {
        float basis = Bernstein(n, i, t);
        point += basis * controlPoints[i].position;
    }

    return point;
}
     

    private float CalculateBlendFunction(int i, int n, float t) {
        float blend = 1;
        for (int j = 1; j <= n; j++) {
            float t_j = (float)j / (n - i + 1);
            blend *= (t >= t_j) ? 1 : (t - t_j) / (t_j - t);
        }
        return blend;
    }

    private float Bernstein(int n, int i, float t) {
        float coef = BinomialCoefficient(n, i);
        return coef * Mathf.Pow(t, i) * Mathf.Pow(1 - t, n - i);
    }

    private float BinomialCoefficient(int n, int k) {
        return Factorial(n) / (Factorial(k) * Factorial(n - k));
    }

    private float Factorial(int n) {
        if (n == 0)
            return 1;

        float result = 1;
        for (int i = 1; i <= n; i++) {
            result *= i;
        }
        return result;
    }

    private void SpawnObjectAtPoint(Vector3 point) {
        GameObject spawnedObject = Instantiate(prefabToSpawn, point, Quaternion.identity);
        Vector3 tangent = CalculateTangentAtPoint(point);
        spawnedObject.transform.rotation = Quaternion.LookRotation(tangent, Vector3.up);
        _spawnedStack.Push(spawnedObject);
    }

    private Vector3 CalculateTangentAtPoint(Vector3 point) {
        float delta = 0.01f; // Small delta value for numerical stability
        float t = CalculateClosestT(point);

        // Calculate points with slight offset to approximate tangent
        Vector3 p1 = CalculatePointOnCurve(t + delta);
        Vector3 p2 = CalculatePointOnCurve(t - delta);

        // Approximate tangent vector
        Vector3 tangent = (p1 - p2).normalized;

        return tangent;
    }

    private float CalculateClosestT(Vector3 point) {
        float minDistance = float.MaxValue;
        float closestT = 0f;

        for (float t = 0f; t <= 1f; t += 0.01f) {
            Vector3 curvePoint = CalculatePointOnCurve(t);
            float distance = Vector3.Distance(curvePoint, point);
            if (distance < minDistance) {
                minDistance = distance;
                closestT = t;
            }
        }

        return closestT;
    }


    /*
    private Vector3 CalculateBezierTangent(Vector3 point) {
        float delta = 0.01f; // Small delta value for numerical stability
        float t = CalculateClosestT(point);

        Vector3 tangent = Vector3.zero;

        for (int i = 0; i < controlPoints.Count; i++) {
            Vector3[] p2Delta = new Vector3[controlPoints.Count];
            p2Delta[i] = controlPoints[i].forward * delta;

            Vector3 p1 = CalculatePointOnCurve(t + delta, p2Delta);
            Vector3 p2 = CalculatePointOnCurve(t - delta, p2Delta);

            tangent += (p1 - p2);
        }

        return tangent.normalized;
    }

    
    private float CalculateClosestT(Vector3 point) {
        float minDistance = float.MaxValue;
        float closestT = 0f;

        for (float t = 0f; t <= 1f; t += 0.01f) {
            Vector3 curvePoint = CalculatePointOnCurve(t);
            float distance = Vector3.Distance(curvePoint, point);
            if (distance < minDistance) {
                minDistance = distance;
                closestT = t;
            }
        }

        return closestT;
    }

    private Vector3 CalculatePointOnCurve(float t, Vector3[] p2Delta) {
        Vector3 p0 = startPoint.position;
        Vector3 p1 = endPoint.position;
        Vector3[] p2 = new Vector3[controlPoints.Count];

        for (int i = 0; i < controlPoints.Count; i++) {
            p2[i] = controlPoints[i].position + p2Delta[i];
        }

        float oneMinusT = 1f - t;
        Vector3 result = oneMinusT * oneMinusT * p0;

        for (int i = 0; i < p2.Length; i++) {
            result += Mathf.Pow(oneMinusT, p2.Length - i) * Mathf.Pow(t, i) * p2[i];
        }

        result += t * t * p1;

        return result;
    }

    */
}
