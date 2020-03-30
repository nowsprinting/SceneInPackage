using UnityEngine;

namespace SceneInPackage
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField] private float degrees = 45.0f;

        private void Update()
        {
            var angle = degrees * Time.deltaTime;
            gameObject.transform.Rotate(angle, 0.0f, 0.0f);
        }
    }
}
