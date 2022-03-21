using UnityEngine;

namespace Bitszer
{
    public class LoadingSpinner : MonoBehaviour
    {
        public float speed = .05f;

        private void Update()
        {
            Quaternion rotation = Quaternion.AngleAxis(180, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed);

            if (Quaternion.Angle(transform.rotation, rotation) < 1)
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
        }
    }
}