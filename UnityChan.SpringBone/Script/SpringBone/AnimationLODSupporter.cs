using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UTJ
{
    [RequireComponent(typeof(LODGroup))]
    [DefaultExecutionOrder(-10)]
    public class AnimationLODSupporter : MonoBehaviour
    {

        public Camera lodCamera;
        public float lodCrossFadeSmoothDrag = 5f;

        protected LODGroup _lodGroup;
        protected LOD[] _lods;
        protected int _currentLODLevel;

        public bool IsValid ()
        {
            return _lodGroup != null && _lodGroup.enabled && enabled;
        }

        public int QueryCurrentLODLevel ()
        {
            return _currentLODLevel;
        }

        protected void Start()
        {
            _lodGroup = GetComponent<LODGroup>();
            if (_lodGroup != null)
            {
                _lods = _lodGroup.GetLODs();

                if (lodCamera == null)
                    lodCamera = Camera.main;
            }
        }

        protected void LateUpdate()
        {
            var relativeHeight = GetRelativeHeight(_lodGroup, lodCamera ?? Camera.current);

            int lodIndex = (_lodGroup.lodCount - 1);
            for (var i = 0; i < _lods.Length; i++)
            {
                var lod = _lods[i];

                if (relativeHeight >= lod.screenRelativeTransitionHeight)
                {
                    lodIndex = i;
                    break;
                }
            }

            _currentLODLevel = lodIndex;
        }

        protected float GetRelativeHeight(LODGroup lodGroup, Camera camera)
        {
            var distance = (lodGroup.transform.TransformPoint(lodGroup.localReferencePoint) - camera.transform.position).magnitude;
            return DistanceToRelativeHeight(camera, (distance / QualitySettings.lodBias), GetWorldSpaceSize(lodGroup));
        }

        protected float DistanceToRelativeHeight(Camera camera, float distance, float size)
        {
            if (camera.orthographic)
                return size * 0.5F / camera.orthographicSize;

            var halfAngle = Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView * 0.5F);
            var relativeHeight = size * 0.5F / (distance * halfAngle);
            return relativeHeight;
        }
        
        protected float GetWorldSpaceSize(LODGroup lodGroup)
        {
            return GetWorldSpaceScale(lodGroup.transform) * lodGroup.size;
        }

        protected float GetWorldSpaceScale(Transform t)
        {
            var scale = t.lossyScale;
            float largestAxis = Mathf.Abs(scale.x);
            largestAxis = Mathf.Max(largestAxis, Mathf.Abs(scale.y));
            largestAxis = Mathf.Max(largestAxis, Mathf.Abs(scale.z));
            return largestAxis;
        }

    }

}
