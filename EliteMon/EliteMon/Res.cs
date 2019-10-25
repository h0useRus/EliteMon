using System.Windows;
using System.Windows.Media;
using NSW.EliteDangerous.API;

namespace NSW.EliteDangerous.Monitor
{
    public static class Res
    {
        public static T GetResource<T>(string key)
        {
            var result = Application.Current.TryFindResource(key);
            if (result != null)
                return (T)result;
            return default;
        }
        
        public static DrawingImage GetVector(StationType stationType)
        {
            var imageKey = stationType switch
            {
                StationType.CoriolisStarport => "CoriolisStarport",
                StationType.OcellusStarport => "OcellusStarport",
                StationType.OrbisStarport => "OrbisStarport",
                StationType.PlanetaryOutpost => "PlanetaryOutpost",
                StationType.PlanetaryPort => "PlanetaryPort",
                StationType.AsteroidBase => "AsteroidBase",
                StationType.MegaShip => "MegaShip",
                StationType.CivilianMegaShip => "MegaShip",
                _ => "Outpost"
            };
            return GetResource<DrawingImage>($"Station.{imageKey}.DrawingImage");
        }
    }
}
