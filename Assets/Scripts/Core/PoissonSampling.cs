using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sail
{
    using ConstraintType = System.Func<Vector2, bool>;

    // Add in range constraint
    public class PoissonSampling
    {
        private float Radius = 150f;
        private float SafeRadius = 300f;
        private int CandidateMaxTry = 30;
        private Vector2 RangeBottomLeft = Vector2.zero;
        private Vector2 RangeTopRight = Vector2.one;
        private ConstraintType ConstraintDelegate = null;

        public PoissonSampling( float radius,
                                Vector2 rangeBottomLeft, Vector2 rangeTopRight,
                                int candidateMaxTry = 30, ConstraintType constraintDelegate = null)
        {
            Radius = radius;
            SafeRadius = Radius * 2f;
            CandidateMaxTry = candidateMaxTry;
            RangeBottomLeft = rangeBottomLeft;
            RangeTopRight = rangeTopRight;
            ConstraintDelegate = constraintDelegate;
        }

        public List<Vector2> get()
        {
            List<Vector2> samples = new List<Vector2>();
            List<bool> isSampleActive = new List<bool>();

            UnityEngine.Random.InitState(System.DateTime.Now.Second);

            //var rootRect = EventList.GetComponent<RectTransform>().rect;

            //samples.Add(new Vector2(rootRect.width / 2, rootRect.height / 2));
            // Entry / Exit node
            //samples.Add(new Vector2(0, rootRect.height / 2));
            //samples.Add(new Vector2(rootRect.width, rootRect.height / 2));

            var center = RangeBottomLeft + (RangeTopRight - RangeBottomLeft) / 2f;
            samples.Add(center);

            isSampleActive.Add(true);

            int activeSampleIndex = -1;
            while ((activeSampleIndex = isSampleActive.FindIndex(e => e == true)) != -1)
            {
                var activeSample = samples[activeSampleIndex];

                for (int i = 0; i < CandidateMaxTry; i++)
                {
                    var c = candidate(activeSample);

                    if (isCandidateValid(c, samples))
                    {
                        samples.Add(c);
                        isSampleActive.Add(true);
                        break;
                    }

                    if (i == CandidateMaxTry - 1)
                    {
                        isSampleActive[activeSampleIndex] = false;
                    }
                }
            }

            return samples;
        }

        private Vector2 candidate(Vector2 p)
        {
            var angle = UnityEngine.Random.Range(0, Mathf.PI * 2f);
            var radius = UnityEngine.Random.Range(Radius, SafeRadius);

            return p + new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
        }

        private bool isCandidateValid(Vector2 candidate, List<Vector2> samples)
        {
            // in screen
            if (ConstraintDelegate != null && ConstraintDelegate(candidate) == false)
            {
                return false;
            }

            foreach (var s in samples)
            {
                if ((s - candidate).magnitude < Radius)
                {
                    return false;
                }
            }

            return true;
        }
    }

}
