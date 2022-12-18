using WebApplication2.Models.Travel;

namespace WebApplication2.Utilities;

public static class TravelFilter
{
    private const double R = 6371e3;
    private const double DegreeY = 100, DegreeX = 60;

    private static bool CloseCoords(double lat1, double lon1, double lat2, double lon2, double divertionDistance)
    {
        var distance = DistanceBetweenCoordinates(lat1, lon1, lat2, lon2);
        return distance <= divertionDistance;
    }

    private static double PossibleDivertionDistance(double tripLength)
    {
        tripLength /= 1000;
        tripLength = tripLength switch
        {
            <= 1 => 0,
            >= 300 => 12,
            >= 50 => (2.1 * Math.Log(tripLength)),
            _ => (Math.Pow(1.043, tripLength))
        } * 1000;
        return tripLength;
    }

    private static double DistanceBetweenCoordinates(double lat1, double lon1, double lat2, double lon2) {
        // Pythagorean theorem
        // let x, y;
        // x = dx * Math.cos((lat1 + lat2) / 2);
        // y = dy;
        // d = Math.sqrt(x * x + y * y) * R;

        var x1 = lat1 * Math.PI / 180;
        var x2 = lat2 * Math.PI / 180;
        var dx = (lat2 - lat1) * Math.PI / 180;
        var dy = (lon2 - lon1) * Math.PI / 180;
        var a = Math.Sin(dx / 2) * Math.Sin(dx / 2) +
                Math.Cos(x1) * Math.Cos(x2) * Math.Sin(dy / 2) * Math.Sin(dy / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var d = R * c;
    
        return d;
    }

    // Function iterates through the metaData list and finds if the searched trip is similar enough to the current trip.
    public static bool RelevantRideFull(SearchInfo searchInfo, IEnumerable<MetaModel> enumMetaList, 
        string origin, string destination)
    {
        // Create the metaList from and enumerableMetaList.
        var metaList = enumMetaList.ToList();
        // Find the first leg of the current trip.
        var pos = metaList.FindIndex(0, metaList.Count, e => e.Origin == origin);
        // Calculate the possible relevant divertion distance.
        var tripDivertionDistance = PossibleDivertionDistance(metaList.Sum(meta => meta.Distance));
        // Calculate the average bearing of the entire trip.
        var averageBearing = metaList.Average(meta => meta.Bearing);
        // If the average bearing is similar to the bearing of the searched trip, then the bearing check has passed.
        var searchRelevanceBearings = Math.Abs(averageBearing - (double)searchInfo.Bearings) <= 45;
        // Set the Origin relevance and Destination relevance and Bearing leg relevance checks to false.
        bool searchRelevanceOrigin = false, searchRelevanceDestination = false, searchRelevanceBearingsLeg = false,
            searchRelevanceOriginAlternate = false, searchRelevanceDestinationAlternate = false;

        // Iterates through the entire metaList in a linear order.
        while (true)
        {
            // If the current leg of the trip is similar in bearings to the searched bearing, 
            // then searchRelevanceBearingsLeg is set to 'true'.
            if ((Math.Abs(metaList[pos].Bearing - (double)searchInfo.Bearings) <= 30) && !searchRelevanceBearingsLeg)
            {
                searchRelevanceBearingsLeg = true;
            }
            // If the current leg Origin is close to the searched Origin point,
            // searchRelevanceOrigin = 'false' and searchRelevanceOriginAlternate = 'false'
            // then
            // searchRelevanceOrigin is set to 'true'.
            if ((
                CloseCoords(
                    metaList[pos].OriginLat, metaList[pos].OriginLng, 
                    (double)searchInfo.OriginLat, (double)searchInfo.OriginLng, 
                    tripDivertionDistance
                )) && !searchRelevanceOrigin && !searchRelevanceOriginAlternate
            )
            {
                searchRelevanceOrigin = true;
            }
            // If the searchRelevanceOrigin = 'false' and searchRelevanceOriginAlternate = 'false', 
            // check if the searchInfo Origin point is close to the trip route.
            // If it is close enough, set searchRelevanceOriginAlternate = 'true'.
            if ((!searchRelevanceOrigin) && (!searchRelevanceOriginAlternate))
            {
                var bearingDiff = Math.Abs((double)(searchInfo.Bearings - metaList[pos].Bearing));
                var distanceMetaOrigin = DistanceBetweenCoordinates(metaList[pos].OriginLat, 
                    metaList[pos].OriginLng, (double)searchInfo.OriginLat, (double)searchInfo.OriginLng);
                var distanceMetaOriginDistance = Math.Sqrt((2 * (distanceMetaOrigin * distanceMetaOrigin)) -
                                                           ((2 * (distanceMetaOrigin * distanceMetaOrigin)) * 
                                                            Math.Cos(bearingDiff / 180 * Math.PI)));
                if (distanceMetaOriginDistance <= tripDivertionDistance)
                {
                    searchRelevanceOriginAlternate = true;
                }
            }
            // If the current leg Destination is close to the searched Destination point,
            // searchRelevanceDestination = 'false' and searchRelevanceDestinationAlternate = 'false'
            // and either searchRelevanceOrigin = 'true', or searchRelevanceOriginAlternate = 'true', then
            // searchRelevanceDestination is set to 'true'.
            if ((
                    CloseCoords(
                        metaList[pos].DestinationLat, metaList[pos].DestinationLng, 
                        (double)searchInfo.DestinationLat, (double)searchInfo.DestinationLng, 
                        tripDivertionDistance
                    )) && (!searchRelevanceDestination && !searchRelevanceDestinationAlternate) &&
                (searchRelevanceOrigin || searchRelevanceOriginAlternate)
               )
            {
                searchRelevanceDestination = true;
            }
            // If the searchRelevanceDestination = 'false' and searchRelevanceDestinationAlternate = 'false', 
            // and either searchRelevanceOrigin = 'true', or searchRelevanceOriginAlternate = 'true',
            // check if the searchInfo Destination point is close to the trip route.
            // If it is close enough, set searchRelevanceDestinationAlternate = 'true'.
            if ((!searchRelevanceDestination && !searchRelevanceDestinationAlternate) &&
                (searchRelevanceOrigin || searchRelevanceOriginAlternate))
            {
                var bearingDiff = Math.Abs((double)(searchInfo.Bearings - metaList[pos].Bearing));
                var distanceMetaDestination = DistanceBetweenCoordinates(metaList[pos].DestinationLat, 
                    metaList[pos].DestinationLng, (double)searchInfo.DestinationLat, (double)searchInfo.DestinationLng);
                var distanceMetaDestinationDistance = Math.Sqrt((2 * (distanceMetaDestination * distanceMetaDestination)) -
                                                           (2 * (distanceMetaDestination * distanceMetaDestination) * 
                                                            Math.Cos(bearingDiff / 180 * Math.PI)));
                if (distanceMetaDestinationDistance <= tripDivertionDistance)
                {
                    searchRelevanceDestinationAlternate = true;
                }
            }
            // If enough of the checks have passed, the function returns true.
            if ((searchRelevanceBearings || searchRelevanceBearingsLeg) &&
                (searchRelevanceOrigin || searchRelevanceOriginAlternate) && 
                (searchRelevanceDestination || searchRelevanceDestinationAlternate))
            {
                return true;
            }
            // If the end of the metaList has been reached, but not all the checks have passed,
            // the function returns false.
            if (metaList[pos].Destination == destination)
            {
                return false;
            }
            // If the current leg of the trip has a good bearing, but not all the criteria match,
            // set the searchRelevanceBearingsLeg check back to false.
            searchRelevanceBearingsLeg = false;
            // Set the position index of the metaList to the next leg of the trip.
            pos = metaList.FindIndex(0, metaList.Count, e => e.Origin == metaList[pos].Destination);
        }
    }

    public static bool RelevantRideOrigin(SearchInfo searchInfo, IEnumerable<MetaModel> metaList)
    {
        var tripDistance = metaList.Sum(meta => meta.Distance);
        var tripDivertionDistance = PossibleDivertionDistance(tripDistance);
        return metaList.Any(meta => (CloseCoords
            (meta.OriginLat, meta.OriginLng, (double)searchInfo.OriginLat, 
                (double)searchInfo.OriginLng, tripDivertionDistance)));
    }
    
    public static bool RelevantRideDestination(SearchInfo searchInfo, IEnumerable<MetaModel> metaList)
    {
        var tripDistance = metaList.Sum(meta => meta.Distance);
        var tripDivertionDistance = PossibleDivertionDistance(tripDistance);
        return metaList.Any(meta => (CloseCoords
        (meta.DestinationLat, meta.DestinationLng, (double)searchInfo.DestinationLat, 
            (double)searchInfo.DestinationLng, tripDivertionDistance)));
    }

    public static string CoordConstructor(IEnumerable<MetaModel> enumMetaList, string origin, string destination)
    {
        var metaList = enumMetaList.ToList();
        var pos = metaList.FindIndex(0, metaList.Count, e => e.Origin == origin);
        var LatLng = metaList[pos].OriginLat + "," + metaList[pos].OriginLng + ",";

        while (true)
        {
            LatLng += metaList[pos].DestinationLat + "," + metaList[pos].DestinationLng + ",";
            if (metaList[pos].Destination == destination)
            {
                return LatLng;
            }
            pos = metaList.FindIndex(0, metaList.Count, e => e.Origin == metaList[pos].Destination);
        } 
    }
}