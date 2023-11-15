using OperationPlayground.Rounds;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Splines;

namespace OperationPlayground.SupplyTruck
{
    public class SupplyTruckMovement : MonoBehaviour
    {
        public SplineContainer entranceSpline;
        public SplineContainer exitSpline;

        private SplineAnimate splineAnimate;

        private Coroutine moveCoroutine;

        private void Start()
        {
            splineAnimate = GetComponent<SplineAnimate>();
            RoundManager.Instance.onPreRoundStart += TruckEnter;
            RoundManager.Instance.onPreRoundEnd += TruckExit;
        }

        /*private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I)) MoveTruck(entranceSpline);
            if (Input.GetKeyDown(KeyCode.O)) MoveTruck(exitSpline);
        }*/

        private void TruckEnter()
        {
            Debug.Log("TRUCK INCOMING!");
            moveCoroutine ??= StartCoroutine(MoveTruckOnSpline(entranceSpline));
        }

        private void TruckExit()
        {
            Debug.Log("TRUCK HEADING OUT!");
            moveCoroutine ??= StartCoroutine(MoveTruckOnSpline(exitSpline));
        }

        /*private void MoveTruck(SplineContainer spline)
        {
            moveCoroutine ??= StartCoroutine(MoveTruckOnSpline(spline));
        }*/

        private IEnumerator MoveTruckOnSpline(SplineContainer spline)
        {
            splineAnimate.Container = spline;
            splineAnimate.Restart(true);

            while (splineAnimate.ElapsedTime < splineAnimate.Duration - 0.05f) yield return null;

            moveCoroutine = null;
        }
    }
}
