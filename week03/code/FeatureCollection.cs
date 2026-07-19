public class FeatureCollection
{
    // JSON mapping for USGS GeoJSON feed
    public Feature[] Features { get; set; }

    public class Feature
    {
        public Properties Properties { get; set; }
    }

    public class Properties
    {
        public double? Mag { get; set; }
        public string Place { get; set; }
    }
}