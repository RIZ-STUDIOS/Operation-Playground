using OperationPlayground.Interactables;
using OperationPlayground.Player;
using OperationPlayground.Weapons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerCanvas : MonoBehaviour
    {
        public CanvasGroup interactCG;
        public CanvasGroup supplyShopCG;
        public GameObject reticle;

        private RectTransform canvasRectTransform;
        private RectTransform reticleRectTransform;

        public PlayerManager playerManager;
        private Weapon currentPlayerWeapon;
        private Camera playerCamera;

        private Coroutine fadeCoroutine;

        private Vector3 smoothVelocity = Vector3.zero;

        private void Awake()
        {
            InitPlayerFields();

            canvasRectTransform = GetComponent<RectTransform>();
            reticleRectTransform = reticle.GetComponent<RectTransform>();
        }

        private void FixedUpdate()
        {
            MatchReticleToMuzzleTrajectory();
        }

        private void OnSetInteractable(Interactable interactable)
        {
            if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

            if (interactable)
                fadeCoroutine = StartCoroutine(ToggleCanvasElement(interactCG, true));
            else
                fadeCoroutine = StartCoroutine(ToggleCanvasElement(interactCG, false));
        }

        private void ToggleReticle(InputAction.CallbackContext value)
        {
            reticle.SetActive(!reticle.activeSelf);
        }

        private void MatchReticleToMuzzleTrajectory()
        {
            if (!currentPlayerWeapon) return;

            Ray firepointRay = new Ray
                (
                    currentPlayerWeapon.FirePointTransform.position,
                    currentPlayerWeapon.FirePointTransform.forward
                );

            Vector3 screenPoint;

            if (Physics.Raycast(firepointRay, out RaycastHit hit, 999f, 1, QueryTriggerInteraction.Ignore))
            {
                screenPoint = playerCamera.WorldToScreenPoint(hit.point);
            }
            else
            {
                screenPoint = playerCamera.WorldToScreenPoint(firepointRay.origin + firepointRay.direction * 200f);
            }

            screenPoint.z = 0;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, playerCamera, out Vector2 rectPoint))
            {
                reticleRectTransform.anchoredPosition = Vector3.SmoothDamp(reticleRectTransform.anchoredPosition, rectPoint, ref smoothVelocity, 0.1f);
            }
        }

        private void InitPlayerFields()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            playerManager.PlayerInteraction.onSetInteractable += OnSetInteractable;

            playerManager.playerInput.Basic.ZoomMap.performed += ToggleReticle;

            playerManager.PlayerShooter.onWeaponSwitch += (weapon) => currentPlayerWeapon = weapon;

            playerCamera = playerManager.PlayerCamera.Camera;
        }

        public IEnumerator ToggleCanvasElement(CanvasGroup canvasGroup, bool isFadeIn, bool interactable = false, float fadeSpeed = 2)
        {
            Vector2 fadeVector;

            if (isFadeIn) fadeVector = new Vector2(canvasGroup.alpha, 1);
            else fadeVector = new Vector2(canvasGroup.alpha, 0);

            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime * fadeSpeed;
                canvasGroup.alpha = Mathf.Lerp(fadeVector.x, fadeVector.y, progress);

                yield return null;
            }
            canvasGroup.alpha = fadeVector.y;

            if (interactable)
            {
                if (isFadeIn) canvasGroup.interactable = true;
                else canvasGroup.interactable = false;
            }

            fadeCoroutine = null;
        }
    }
}
