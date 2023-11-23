using UnityEngine;
using UnityEngine.UI;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;
    public GameObject projectile;
    public float launchSpeed = 10f;
    public Transform Barrel;

    [Header("****Trajectory Display****")]
    public LineRenderer lineRenderer;
    public int linePoints = 175;
    public float timeIntervalInPoints = 0.01f;

    public Text launchSpeedText;
    public Text launchRotationText;
    public Text alturaMaximaText;
    public Text alcanceText;

    void Update()
    {
        UpdateTextObjects();
    }


    void DrawTrajectory()
    {
        Vector3 origin = launchPoint.position;
        Vector3 startVelocity = launchSpeed * launchPoint.up;
        lineRenderer.positionCount = linePoints;
        float time = 0;
        for (int i = 0; i < linePoints; i++)
        {
            // s = u*t + 1/2*g*t*t
            var x = (startVelocity.x * time) + (Physics.gravity.x / 2 * time * time);
            var y = (startVelocity.y * time) + (Physics.gravity.y / 2 * time * time);
            Vector3 point = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, origin + point);
            time += timeIntervalInPoints;
        }

    }

    // Function to increase launch speed by one
    public void IncreaseLaunchSpeed()
    {
        launchSpeed += 1f;
    }

    // Function to decrease launch speed by one
    public void DecreaseLaunchSpeed()
    {
        launchSpeed = Mathf.Max(launchSpeed - 1f, 0f);
    }

    // Function to increase launch point rotation by one
    public void IncreaseLaunchRotation()
    {
        Barrel.Rotate(Vector3.forward, 1f);
    }

    // Function to decrease launch point rotation by one
    public void DecreaseLaunchRotation()
    {
        Barrel.Rotate(Vector3.forward, -1f);
    }

    public void LaunchProjectile()
    {
        var _projectile = Instantiate(projectile, launchPoint.position, launchPoint.rotation);  
        _projectile.GetComponent<Rigidbody2D>().velocity = launchSpeed * launchPoint.up;

        // Calculate and display maximum altitude
        float maxAltitude = CalculateMaxAltitude(launchSpeed, Mathf.Floor(Barrel.rotation.eulerAngles.z) - 270);
        alturaMaximaText.text = maxAltitude.ToString("F2") + "m";

        float range = CalculateRange(launchSpeed, Mathf.Floor(Barrel.rotation.eulerAngles.z) - 270);
        alcanceText.text = range.ToString("F2") + "m";

        // Draw trajectory after the projectile is launched
       
        lineRenderer.enabled = true;
        DrawTrajectory();
    }


    void UpdateTextObjects()
    {
        if (launchSpeedText != null)
        {
            launchSpeedText.text =  launchSpeed.ToString() + "m/s";
        }

        if (launchRotationText != null)
        {
            float truncatedRotation = Mathf.Floor(Barrel.rotation.eulerAngles.z);
            launchRotationText.text = (truncatedRotation - 270).ToString() + "º";
        }

        if (alcanceText != null)
        {
            Debug.Log("oi");
        }

        if (alturaMaximaText != null)
        {
            Debug.Log("oi");
        }
    }


    float CalculateMaxAltitude(float initialVelocity, float launchAngle)
    {
        Debug.Log("Angulo q veio: " + launchAngle);
        Debug.Log("Angulo q veio: " + Physics.gravity.y);
        // Convert launch angle to radians
        float launchAngleRad = launchAngle * Mathf.Deg2Rad;
        // Calculate the maximum height using the formula
        Debug.Log(initialVelocity);
        float maxAltitude = (initialVelocity * initialVelocity * Mathf.Sin(launchAngleRad) * Mathf.Sin(launchAngleRad)) / (2 * Mathf.Abs(Physics.gravity.y));

        Debug.Log("Seno de graus"  + Mathf.Sin(launchAngleRad));

        return maxAltitude;
    }

    float CalculateRange(float initialVelocity, float launchAngle)
    {
        // Convert launch angle to radians
        float launchAngleRad = launchAngle * Mathf.Deg2Rad;

        // Calculate the range using the formula
        float range = (initialVelocity * initialVelocity * Mathf.Sin(2 * launchAngleRad)) / Mathf.Abs(Physics.gravity.y);

        return range;
    }

}
