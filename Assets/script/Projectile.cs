using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 initialSpeed;
    public float gravity = 9.81f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float timeElapsed = 0f;
    private bool shot;
    public float additionalY = 1;
    public Transform target;
    private float taiku;
    public void Start()
    {
    }
    public void Launch()
    {
        startPosition = transform.position;
        shot = true;
        taiku=CalculateTimeToTarget(initialSpeed.y, target.position.y- transform.position.y);
    }

    private void Update()
    {
        if (Input.GetKeyDown("k")) Launch();
        if (shot)
        {
            timeElapsed += Time.deltaTime;
            targetPosition = target.position;
            Vector3 direction = (targetPosition - startPosition).normalized;
            float distance = Vector3.Distance(startPosition, targetPosition);

            // –C’e‚Ì‰‘¬“x‚ğŒvZ‚·‚é

            float initialVelocityY = initialSpeed.y;
            float initialVelocityXZ = initialSpeed.x;

            // Œ»İ‚Ì–C’e‚ÌˆÊ’u‚ğŒvZ‚·‚é
            float currentPosY = startPosition.y + initialVelocityY * timeElapsed - 0.5f * gravity * Mathf.Pow(timeElapsed, 2);

            // –C’e‚ÌŒü‚«‚ğŒ»İ‚Ì‘¬“x•ûŒü‚ÉŒü‚¯‚é
            Vector3 currentVelocity = new Vector3(initialVelocityXZ * direction.x, initialVelocityY - gravity * timeElapsed, initialVelocityXZ * direction.z);
            transform.rotation = Quaternion.LookRotation(currentVelocity);

            // –C’e‚ğŒ»İ‚ÌˆÊ’u‚ÉˆÚ“®‚·‚é
            transform.position = new Vector3(startPosition.x, currentPosY, startPosition.z);
            transform.position = Vector3.Lerp(transform.position,new Vector3(targetPosition.x, transform.position.y, targetPosition.z),timeElapsed/taiku);

            // –Ú•WˆÊ’u‚É“’B‚µ‚½‚ç‰½‚à‚µ‚È‚¢
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                shot = false;
            }
        }
    }
    public float CalculateTimeToTarget(float initialVelocityY, float targetHeight)
    {
        float time= initialVelocityY / gravity;
        float length = time * (initialVelocityY -gravity/2*time);
        if (targetHeight > length) return float.NaN;
        length-= targetHeight;
        time += Mathf.Sqrt((2 * length) / gravity);
        return time;
    }

}


