using DG.Tweening;
using Gameplay.Data;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public class CameraRotator : MonoBehaviour
    {
        [SerializeField] private Transform _anchor1;
        [SerializeField] private Transform _anchor2;
        [SerializeField] private Camera _camera;

        private CameraSettings _cameraSettings;
        private float _randomFov;


        [Inject]
        public void Construct(Settings settings)
        {
            _cameraSettings = settings.cameraSettings;
        }

        public void Start()
        {
            _camera.transform.localPosition = new Vector3(0f, _cameraSettings.height, -_cameraSettings.roundRadius);
            _camera.transform.LookAt(new Vector3(0f, _cameraSettings.lookAtHeight, 0f));

            _anchor1.DOLocalRotate(
                    new Vector3(0f, 360f, 0f),
                    _cameraSettings.roundDuration,
                    RotateMode.FastBeyond360)
                .SetLoops(-1)
                .SetEase(Ease.Linear);

            _anchor2.DOLocalMove(
                    new Vector3(
                        Random.Range(-_cameraSettings.roamingRadius, _cameraSettings.roamingRadius),
                        0f,
                        Random.Range(-_cameraSettings.roamingRadius, _cameraSettings.roamingRadius)),
                    _cameraSettings.roamingDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.Linear);

            TweenFov();
        }

        private void SaveFov()
        {
            _camera.fieldOfView = _randomFov;
            TweenFov();
        }

        private void TweenFov()
        {
            _randomFov = Random.Range(_cameraSettings.fovMin, _cameraSettings.fovMax);

            var fovTween = DOTween.Sequence();
            fovTween
                .Append(_camera
                    .DOFieldOfView(_randomFov, _cameraSettings.fovDuration)
                    .SetEase(Ease.Linear))
                .AppendInterval(_cameraSettings.fovDelay)
                .OnComplete(SaveFov);
        }
    }
}