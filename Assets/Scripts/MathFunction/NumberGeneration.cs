using UnityEngine;

namespace Unity.Space4x.Assets.Scripts.MathFunction
{
        public class NumberGeneration
        {
                /// <summary>
                /// Generates a random number based off a bell curve centred at a mean value.
                /// </summary>
                /// <returns>The random number.</returns>
                public static float BellCurve(int minValue, int maxValue, float mean)
                {
                        float rand = Random.Range(0, 1);
                        return rand <= 0.5f ? Random.Range(minValue, mean) : Random.Range(mean, maxValue);
                }

                /// <summary>
                /// Generates a random number based off a bell curve centred at the mean value.
                /// </summary>
                /// <param name="minValue"></param>
                /// <param name="maxValue"></param>
                /// <returns>The random number.</returns>
                public static float BellCurve(int minValue, int maxValue)
                {
                        float rand = Random.Range(0, 1);
                        float mean = (minValue + maxValue) * 0.5f;
                        return rand <= 0.5f ? Random.Range(minValue, mean) : Random.Range(mean, maxValue);
                }
        }
}
