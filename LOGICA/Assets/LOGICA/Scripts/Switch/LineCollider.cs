using UnityEngine;
using UnityEngine.Serialization;

namespace LOGICA.Switch
{
    public class LineCollider : MonoBehaviour
    {
        [FormerlySerializedAs("_swichObject")] [SerializeField] private SwitchObject _switchObject = default;

        public void OnTriggerEnter2D(Collider2D other)
        {
            _switchObject.OnCollision();
        }
    }
}