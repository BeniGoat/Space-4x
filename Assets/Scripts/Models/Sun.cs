using System;
using System.Collections;
using UnityEngine;

namespace Space4x.Models
{
        public class Sun : SystemBody
        {
                /// <inheritdoc/>
                protected override void Awake()
                {
                        base.Awake();
                        this.Rotator.SetRotationSpeed(1f);
                }
        }
}