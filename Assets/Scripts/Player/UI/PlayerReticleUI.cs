using OperationPlayground.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OperationPlayground.Player.UI
{
    public class PlayerReticleUI : MonoBehaviour
    {
        private PlayerCanvasManager _playerCanvas;
        private CanvasGroup _canvasGroup;

        private RectTransform _playerCanvasRectTransform;
        private RectTransform _canvasRectTransform;

        private Weapon CurrentWeapon => _playerCanvas.playerManager.Shooter.CurrentWeapon;
        private PlayerCamera PlayerCamera => _playerCanvas.playerManager.PlayerCamera;
        private LayerMask LookLayerMask => _playerCanvas.playerManager.PlayerMovementTPS.LookMask;

        private Vector3 smoothVelocity;

        private void Awake()
        {
            _playerCanvas = GetComponentInParent<PlayerCanvasManager>();
            _canvasGroup = GetComponent<CanvasGroup>();

            _playerCanvasRectTransform = _playerCanvas.GetComponent<RectTransform>();
            _canvasRectTransform = GetComponent<RectTransform>();
        }

        private void FixedUpdate()
        {
            MatchReticleToMuzzleTrajectory();
        }

        private void MatchReticleToMuzzleTrajectory()
        {
            if (!CurrentWeapon) return;

            Ray firepointRay = new Ray
                (
                    CurrentWeapon.FirePointTransform.position,
                    CurrentWeapon.FirePointTransform.forward
                );

            Vector3 screenPoint;

            if (Physics.Raycast(firepointRay, out RaycastHit hit, 999f, LookLayerMask, QueryTriggerInteraction.Ignore))
            {
                screenPoint = PlayerCamera.Camera.WorldToScreenPoint(hit.point);
            }
            else
            {
                screenPoint = PlayerCamera.Camera.WorldToScreenPoint(firepointRay.origin + firepointRay.direction * 200f);
            }

            screenPoint.z = 0;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_playerCanvasRectTransform, screenPoint, PlayerCamera.Camera, out Vector2 rectPoint))
            {
                _canvasRectTransform.anchoredPosition = Vector3.SmoothDamp(_canvasRectTransform.anchoredPosition, rectPoint, ref smoothVelocity, 0.1f);
            }
        }
    }
}
