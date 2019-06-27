using UnityEngine;

namespace UTJ
{
    public class ForceVolume : ForceProvider 
    {
        [Range(0f, 1f)]
        public float weight = 0f;
        public float strength = 100f;

        public override Vector3 GetForceOnBone(SpringBone springBone)
        {
            return weight * strength * transform.forward;
        }

        // private

        private void OnDrawGizmos()
        {
            const float DrawScale = 0.05f;
            var origin = transform.position;
            var destination = origin + strength * DrawScale * transform.forward;
            GizmoUtil.DrawArrow(origin, destination, Color.gray, 0.1f);
        }
    }
}