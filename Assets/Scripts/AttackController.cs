using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class AttackController : MonoBehaviour
    {
        private DateTime _prevAttack;
        public float firingAngle = 45.0f;
        public float gravity = 9.8f;

        public GameObject Projectile;
        
        Vector3 m_DistanceFromCamera;

        void Start()
        {
            m_DistanceFromCamera = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - 1);
        }

        private void Update()
        {
            //Detect when there is a mouse click
            if (Input.GetMouseButton(0))
            {
                if (DateTime.UtcNow - _prevAttack < TimeSpan.FromMilliseconds(500))
                {
                    return;
                }
                _prevAttack = DateTime.UtcNow;
                //Create a ray from the Mouse click position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out var hit))
                {
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
            yield return new WaitForSeconds(0.2f);

            var pos = transform.position;
            var projectile = Instantiate(Projectile, new Vector3(pos.x, pos.y + 1, pos.z), Quaternion.identity) as GameObject;
            var pTr = projectile.transform;

            // Calculate distance to target
            float target_Distance = Vector3.Distance(pTr.position, target.position);

            // Calculate the velocity needed to throw the object to the target at specified angle.
            float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

            // Extract the X & Y componenent of the velocity
            float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
            float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);


            // Calculate flight time.
            float flightDuration = target_Distance / Vx;

            // Rotate projectile to face the target.
            pTr.rotation = Quaternion.LookRotation(target.position - pTr.position);
            float elapse_time = 0;
            while (elapse_time < flightDuration)
            {
                pTr.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime, Vx * Time.deltaTime);
                elapse_time += Time.deltaTime;
                yield return null;
            }

            Destroy(projectile);
        }
    }
}