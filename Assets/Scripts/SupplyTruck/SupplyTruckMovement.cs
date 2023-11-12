using System.Collections;
using System.Collections.Generic;
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
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I)) MoveTruck(entranceSpline);
            if (Input.GetKeyDown(KeyCode.O)) MoveTruck(exitSpline);
        }

        private void MoveTruck(SplineContainer spline)
        {
            moveCoroutine ??= StartCoroutine(MoveTruckOnSpline(spline));
        }

        private IEnumerator MoveTruckOnSpline(SplineContainer spline)
        {
            splineAnimate.Container = spline;
            splineAnimate.Restart(true);

            while (splineAnimate.ElapsedTime < splineAnimate.Duration - 0.05f) yield return null;

            moveCoroutine = null;
        }
    }
}
