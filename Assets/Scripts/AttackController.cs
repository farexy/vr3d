using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class AttackController : MonoBehaviour
    {
        public float firingAngle = 45.0f;
        public float gravity = 9.8f;

        public Transform Projectile;
        private Transform myTransform;
        
        Plane m_Plane;
        Vector3 m_DistanceFromCamera;

        void Awake()
        {
            myTransform = transform;
        }

        void Start()
        {
            m_DistanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - 1);
            m_Plane = new Plane(Vector3.forward, m_DistanceFromCamera);
        }

        private void Update()
        {
            //Detect when there is a mouse click
            if (Input.GetMouseButton(0))
            {
                //Create a ray from the Mouse click position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    StartCoroutine(SimulateProjectile(hit.collider.transform));
                    if (hit.collider.gameObject.CompareTag("Crawler"))
                    {
                        //StartCoroutine(SimulateProjectile());
                    }
                }
            }
        }

        IEnumerator SimulateProjectile(Transform target)
        {
            // Short delay added before Projectile is thrown
            yield return new WaitForSeconds(1f);

            // Move projectile to the position of throwing object + add some offset if needed.
            Projectile.position = myTransform.position + new Vector3(0, 0.0f, 0);

            // Calculate distance to target
            float target_Distance = Vector3.Distance(Projectile.position, target.position);

            // Calculate the velocity needed to throw the object to the target at specified angle.
            float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

            // Extract the X & Y componenent of the velocity
            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

            // Calculate flight time.
            float flightDuration = target_Distance / Vx;

            // Rotate projectile to face the target.
            Projectile.rotation = Quaternion.LookRotation(target.position - Projectile.position);
            float elapse_time = 0;
            while (elapse_time < flightDuration)
            {
                Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
                elapse_time += Time.deltaTime;
                yield return null;
            }
            
        }
    }
}