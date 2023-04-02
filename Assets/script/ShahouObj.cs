using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShahouObj :bulletsc
{
    public Vector2 initialSpeed;
    public float gravity = 9.81f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float timeElapsed = 0f;
    private bool shot;
    public float additionalY = 1;
    private float taiku;
    public void Launch()
    {
        startPosition = transform.position;
        shot = true;
        taiku = CalculateTimeToTarget(initialSpeed.y, target.transform.position.y - transform.position.y);
    }

    private void Update()
    {
        if (!shot) Launch();
        if (shot)
        {
            timeElapsed += Time.deltaTime;
            if(target!=null)targetPosition = target.transform.position;
            Vector3 direction = (targetPosition - startPosition).normalized;
            float distance = Vector3.Distance(startPosition, targetPosition);

            // �C�e�̏����x���v�Z����

            float initialVelocityY = initialSpeed.y;
            float initialVelocityXZ = initialSpeed.x;

            // ���݂̖C�e�̈ʒu���v�Z����
            float currentPosY = startPosition.y + initialVelocityY * timeElapsed - 0.5f * gravity * Mathf.Pow(timeElapsed, 2);

            // �C�e�̌��������݂̑��x�����Ɍ�����
            Vector3 currentVelocity = new Vector3(initialVelocityXZ * direction.x, initialVelocityY - gravity * timeElapsed, initialVelocityXZ * direction.z);
            transform.rotation = Quaternion.LookRotation(currentVelocity);

            // �C�e�����݂̈ʒu�Ɉړ�����
            transform.position = new Vector3(startPosition.x, currentPosY, startPosition.z);
            transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z), timeElapsed / taiku);

            // �ڕW�ʒu�ɓ��B�����牽�����Ȃ�
            if (timeElapsed > taiku + 0.5f) Destroy(gameObject);
        }
    }
    public float CalculateTimeToTarget(float initialVelocityY, float targetHeight)
    {
        float time = initialVelocityY / gravity;
        float length = time * (initialVelocityY - gravity / 2 * time);
        if (targetHeight > length) return float.NaN;
        length -= targetHeight;
        time += Mathf.Sqrt((2 * length) / gravity);
        return time;
    }
    public override void Start()
    {
        base.Start();
        lifetime -= 0.1f;
    }
    public override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
    }
}


