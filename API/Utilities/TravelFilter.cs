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
            <= 0 => 0,
            >= 300 => 12,
            >= 50 => (2.1 * Math.Log(tripLength)),
            _ => (Math.Pow(1.045, tripLength) - 0.5)
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

    public static double GetBearings(double lat1, double lon1, double lat2, double lon2) {
    
        var x = ((Math.Cos(lat1) * Math.Sin(lat2)) - (Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(lon2 - lon1)));
        var y = (Math.Sin(lon2 - lon1) * Math.Cos(lat2));
        var b = Math.Atan2(y, x);
        b = ((((b * 180) / Math.PI) + 360) % 360);
    
        return b;
    }

    public static bool RelevantRideFull(SearchInfo searchInfo, IEnumerable<MetaModel> metaList)
    {
////////Common Variable Declaration Start//////////////////////////////////////////////////////////
        var tripDistance = metaList.Sum(meta => meta.Distance);
        var tripDivertionDistance = PossibleDivertionDistance(tripDistance);
////////Common Variable Declaration End////////////////////////////////////////////////////////////
////////Relevance Check Start//////////////////////////////////////////////////////////////////////
        var averageBearing = metaList.Average(meta => meta.Bearing);
        var searchRelevanceBearings = Math.Abs(averageBearing - (double)searchInfo.Bearings) <= 45;
        bool searchRelevanceOrigin = false, searchRelevanceDestination = false;

        foreach (var meta in metaList)
        {
            if ((Math.Abs(meta.Bearing - (double)searchInfo.Bearings) <= 20) && !searchRelevanceBearings)
            {
                searchRelevanceBearings = true;
            }
            if ((
                CloseCoords(
                    meta.OriginLat, meta.OriginLng, 
                    (double)searchInfo.OriginLat, (double)searchInfo.OriginLng, 
                    tripDivertionDistance
                )) && !searchRelevanceOrigin
            )
            {
                searchRelevanceOrigin = true;
            }
            if ((
                CloseCoords(
                    meta.DestinationLat, meta.DestinationLng, 
                    (double)searchInfo.DestinationLat, (double)searchInfo.DestinationLng, 
                    tripDivertionDistance
                )) && !searchRelevanceDestination
            )
            {
                searchRelevanceDestination = true;
            }
        }

        return (searchRelevanceBearings) && (searchRelevanceOrigin) && (searchRelevanceDestination);
/////////Relevance Check End///////////////////////////////////////////////////////////////////////
    }
    public static bool RelevantRideOrigin(SearchInfo searchInfo, IEnumerable<MetaModel> metaList)
    {
////////Common Variable Declaration Start//////////////////////////////////////////////////////////
        var tripDistance = metaList.Sum(meta => meta.Distance);
        var tripDivertionDistance = PossibleDivertionDistance(tripDistance);
////////Common Variable Declaration End////////////////////////////////////////////////////////////
////////Relevance Check Start//////////////////////////////////////////////////////////////////////
        return metaList.Any(meta => (CloseCoords
            (meta.OriginLat, meta.OriginLng, (double)searchInfo.OriginLat, 
                (double)searchInfo.OriginLng, tripDivertionDistance)));
/////////Relevance Check End///////////////////////////////////////////////////////////////////////
    }
    public static bool RelevantRideDestination(SearchInfo searchInfo, IEnumerable<MetaModel> metaList)
    {
////////Common Variable Declaration Start//////////////////////////////////////////////////////////
        var tripDistance = metaList.Sum(meta => meta.Distance);
        var tripDivertionDistance = PossibleDivertionDistance(tripDistance);
////////Common Variable Declaration End////////////////////////////////////////////////////////////
////////Relevance Check Start//////////////////////////////////////////////////////////////////////
        return metaList.Any(meta => (CloseCoords
        (meta.DestinationLat, meta.DestinationLng, (double)searchInfo.DestinationLat, 
            (double)searchInfo.DestinationLng, tripDivertionDistance)));
/////////Relevance Check End///////////////////////////////////////////////////////////////////////
    }
    
    // public static bool isNearOrigin(result, userOriginLat, userOriginLon) {
    //
    //     let originLat = result.routes[0].legs[0].start_location.lat();
    //     let originLon = result.routes[0].legs[0].start_location.lon();
    //     let distance = distanceBetweenCoordinates(originLat, originLon, userOriginLat, userOriginLon);
    //     let divertion = possibleDivertionDistance(fullTripDist(result));
    //
    //     return distance <= divertion;
    // }
    //
    // public static bool isNearDestination(result, userDestinationLat, userDestinationLon) {
    //
    //     let destinationLat = result.routes[0].legs[0].end_location.lat();
    //     let destinationLon = result.routes[0].legs[0].end_location.lon();
    //     let distance = distanceBetweenCoordinates(destinationLat, destinationLon, userDestinationLat, userDestinationLon);
    //     let divertion = possibleDivertionDistance(fullTripDist(result));
    //
    //     return distance <= divertion;
    // }
    //
    // public static bool isSimilarTrip(result, userTrip) {
    //
    //     let userOriginLat = userTrip.routes[0].legs[0].start_location.lat();
    //     let userOriginLon = userTrip.routes[0].legs[0].start_location.lon();
    //     let userDestinationLat = userTrip.routes[0].legs[0].end_location.lat();
    //     let userDestinationLon = userTrip.routes[0].legs[0].end_location.lon();
    //
    //     return isNearOrigin(result, userOriginLat, userOriginLon) && isNearDestination(result, userDestinationLat, userDestinationLon);
    // }
    //
    // public static double tripDistanceDifference(result, userTrip) {
    //
    //     let userOriginLat = userTrip.routes[0].legs[0].start_location.lat();
    //     let userOriginLon = userTrip.routes[0].legs[0].start_location.lon();
    //     let userDestinationLat = userTrip.routes[0].legs[0].end_location.lat();
    //     let userDestinationLon = userTrip.routes[0].legs[0].end_location.lon();
    //
    //     let tripOriginLat = result.routes[0].legs[0].start_location.lat();
    //     let tripOriginLon = result.routes[0].legs[0].start_location.lon();
    //     let tripDestinationLat = result.routes[0].legs[0].end_location.lat();
    //     let tripDestinationLon = result.routes[0].legs[0].end_location.lon();
    //
    //     let userDistance = distanceBetweenCoordinates(userOriginLat, userOriginLon, userDestinationLat, userDestinationLon);
    //     let tripDistance = distanceBetweenCoordinates(tripOriginLat, tripOriginLon, tripDestinationLat, tripDestinationLon);
    //
    //     return userDistance / (tripDistance / 100);
    // }
    //
    // function similarBearing(result, userBearing) {
    //
    // }
    
}