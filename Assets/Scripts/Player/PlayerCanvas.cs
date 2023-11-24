using OperationPlayground.Interactables;
using OperationPlayground.UI;
using OperationPlayground.Weapons;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace OperationPlayground.Player
{
    public class PlayerCanvas : MonoBehaviour
    {
        [SerializeField] private CanvasGroup interactCG;
        [SerializeField] private CanvasGroup supplyShopCG;
        [SerializeField] private CanvasGroup promptCG;
        [SerializeField] private CanvasGroup deathCG;
        public GameObject reticle;

        public GameOverUI gameOverUI;

        public PlayerManager playerManager;

        private RectTransform canvasRectTransform;
        private RectTransform reticleRectTransform;

        private Weapon currentPlayerWeapon;
        private Camera playerCamera;

        private Coroutine fadeCoroutine;
        private Coroutine promptCoroutine;
        private Coroutine deathCGCoroutine;

        [System.NonSerialized]
        public Transform firePointTransform;

        private Transform FirePointTransform => firePointTransform ?? currentPlayerWeapon.FirePointTransform;

        private Vector3 smoothVelocity = Vector3.zero;
        private bool visibleReticle;

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

            if (interactable && interactable.enabled)
                fadeCoroutine = StartCoroutine(ToggleCanvasElement(interactCG, true));
            else
                fadeCoroutine = StartCoroutine(ToggleCanvasElement(interactCG, false));

            interactable.onInteract += DisableInteract;
        }

        public void EnableInteract()
        {
            StartCoroutine(ToggleCanvasElement(interactCG, true));
        }

        public void DisableInteract(PlayerManager playerManager)
        {
            StartCoroutine(ToggleCanvasElement(interactCG, false));
            playerManager.PlayerInteraction.CurrentInteractable.onInteract -= DisableInteract;
        }

        private void MatchReticleToMuzzleTrajectory()
        {
            if (!currentPlayerWeapon) return;

            Ray firepointRay = new Ray
                (
                    FirePointTransform.position,
                    FirePointTransform.forward
                );

            Vector3 screenPoint;

            if (Physics.Raycast(firepointRay, out RaycastHit hit, 999f, playerManager.PlayerMovementTPS.LookMask, QueryTriggerInteraction.Ignore))
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

            playerManager.PlayerShooter.onWeaponSwitch += (weapon) => currentPlayerWeapon = weapon;

            playerCamera = playerManager.PlayerCamera.Camera;
        }

        public IEnumerator ToggleCanvasElement(CanvasGroup canvasGroup, bool isFadeIn, bool interactable = false, float fadeSpeedMod = 2)
        {
            Vector2 fadeVector;

            if (isFadeIn) fadeVector = new Vector2(canvasGroup.alpha, 1);
            else fadeVector = new Vector2(canvasGroup.alpha, 0);

            float progress = 0;
            while (progress < 1)
            {
                progress += Time.deltaTime * fadeSpeedMod;
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

        public void DisplayPrompt(string text, float duration = 0.5f)
        {
            if (promptCoroutine != null) StopCoroutine(promptCoroutine);

            promptCG.alpha = 0;
            promptCG.GetComponentInChildren<TextMeshProUGUI>().text = text;
            promptCoroutine = StartCoroutine(ToggleMessagePrompt(duration));
        }

        private IEnumerator ToggleMessagePrompt(float duration)
        {
            promptCoroutine = StartCoroutine(ToggleCanvasElement(promptCG, true, fadeSpeedMod: 3f));
            while (promptCG.alpha != 1) yield return null;
            
            yield return new WaitForSeconds(duration);

            promptCoroutine = StartCoroutine(ToggleCanvasElement(promptCG, false, fadeSpeedMod: 3f));
        }

        public void ShowDeathScreen()
        {
            if (deathCGCoroutine != null) StopCoroutine(deathCGCoroutine);
            visibleReticle = reticle.activeSelf;
            reticle.SetActive(false);

            deathCGCoroutine = StartCoroutine(ToggleCanvasElement(deathCG, true, true));
        }

        public void HideDeathScreen(bool instant = false)
        {
            if (deathCGCoroutine != null) StopCoroutine(deathCGCoroutine);
            reticle.SetActive(visibleReticle);

            if (instant)
            {
                deathCG.alpha = 0;
                deathCG.interactable = false;
            }
            else
            {
                deathCGCoroutine = StartCoroutine(ToggleCanvasElement(deathCG, false, false));
            }
        }
    }
}
