using UnityEngine;

namespace Unity.Space4x.Assets.Scripts.MathFunction
{
        public class NumberGeneration
        {
                /// <summary>
                /// Generates a random number based off a bell curve centred an intermediary value.
                /// </summary>
                /// <returns>The random number.</returns>
                public static float BellCurve(int minValue, int maxValue, float intermediaryValue)
                {
                        return NumberGeneration.BellCurve((float)minValue, (float)maxValue, intermediaryValue);
                }

                /// <summary>
                /// Generates a random number based off a bell curve centred at an intermediary value.
                /// </summary>
                /// <returns>The random number.</returns>
                public static float BellCurve(float minValue, float maxValue, float intermediaryValue)
                {
                        return Random.value <= 0.5f
                                ? Random.Range(minValue, intermediaryValue)
                                : Random.Range(intermediaryValue, maxValue);
                }
        }
}
