namespace Space4x.Settings
{
        /// <summary>
        /// The settings for a solar system.
        /// </summary>
        public class SystemSettings
        {
                /// <summary>
                /// The number of orbit slots for the system.
                /// </summary>
                public int SystemSize { get; set; }

                /// <summary>
                /// The distance between each orbit.
                /// </summary>
                public float OrbitalSeparationDistance { get; set; }

                /// <summary>
                /// The number of points in an orbit a system body can occupy.
                /// </summary>
                public int OrbitPointsCount { get; set; }

                /// <summary>
                /// The rate at which a planet will spawn in an orbit.
                /// </summary>
                public float PlanetSpawnRate { get; set; }

                /// <summary>
                /// The maximum size of a system body.
                /// </summary>
                public float MaxBodySize { get; set; }

                /// <summary>
                /// The minimum size of a system body.
                /// </summary>
                public float MinBodySize { get; set; }
        }
}
