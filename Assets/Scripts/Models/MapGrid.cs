using System;
using System.Collections.Generic;
using UnityEngine;

namespace Space4x.Models
{
        public class MapGrid : MonoBehaviour
        {
                private Transform lineTransform;
                public List<MapLine> Lines { get; set; }

                private void Awake()
                {
                        this.lineTransform = this.transform;
                        this.Lines = new List<MapLine>();
                }

                public void SetLines(List<MapLine> mapLines)
                {
                        foreach(var mapLine in mapLines)
                        {
                                mapLine.transform.parent = this.lineTransform;
                        }
                }
        }
}