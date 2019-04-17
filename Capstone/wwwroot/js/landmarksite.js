// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.map;
window.marker = false;
window.mapRadius;
window.home_lat;
window.home_lon;
window.distanceElements = [];
window.searchRadius = 15;

window.itineraryMap;
window.itineraryMarker = false;
window.itinerary_lat;
window.itinerary_lon;
window.itinerary_landmarks = [];


function initMap() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            window.home_lat = position.coords.latitude;
            window.home_lon = position.coords.longitude;
            updateAllDistanceElements();
            showDistanceCalculations();
            setMarkerToCurrentLocation();
            setRadiusOnMap();
        }, function () {
            //handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        //handleLocationError(false, infoWindow, map.getCenter());
    }

    //The center location of our map.
    var centerOfMap = new google.maps.LatLng(window.home_lat, window.home_lon);

    //Map options.
    var options = {
        center: centerOfMap, //Set center.
        zoom: 10 //The zoom value.
    };

    //Create the map object.
    window.map = new google.maps.Map(document.getElementById('map'), options);

    //Listen for any clicks on the map.
    google.maps.event.addListener(map, 'click', function (event) {
        updateStartLocationOnMap(event);
    });

    google.maps.event.addDomListener(window, 'load', initMap);

    showAllLandmarkMarkersOnSearchMap();
}

function initItineraryMap() {
    var directionsService = new google.maps.DirectionsService;
    var directionsDisplay = new google.maps.DirectionsRenderer({ suppressMarkers: true });
    window.itinerary_lat = +document.getElementById("itineraryStartingLatitude").value; //unary (+) coerces value into a number
    window.itinerary_lon = +document.getElementById("itineraryStartingLongitude").value; //unary (+) coerces value into a number

    //The center location of our map.
    var centerOfMap = new google.maps.LatLng(window.itinerary_lat, window.itinerary_lon);

    //Map options.
    var options = {
        center: centerOfMap, //Set center.
        zoom: 10 //The zoom value.
    };

    //Create the map object.
    window.itineraryMap = new google.maps.Map(document.getElementById('itineraryMap'), options);

    directionsDisplay.setMap(window.itineraryMap);

    window.itineraryMarker = new google.maps.Marker({
        position: { lat: window.itinerary_lat, lng: window.itinerary_lon },
        map: window.itineraryMap,
        animation: google.maps.Animation.DROP,
        draggable: false,
        title: 'Itinerary starting point'
    });

    //Listen for any clicks on the map.
    google.maps.event.addListener(window.itineraryMap, 'click', function (event) {
        updateStartLocationOfItinerary(event);
    });

    google.maps.event.addDomListener(window, 'load', initItineraryMap);
    placeItineraryLandmarksOnMap();
    calculateAndDisplayRoute(directionsService, directionsDisplay);
}

function calculateAndDisplayRoute(directionsService, directionsDisplay) {
    var waypts = [];
    window.itinerary_landmarks.forEach(item => {
        waypts.push({
            location: { lat: item.Latitude, lng: item.Longitude },
            stopover: true
        });
    });

    var lastLandmark = window.itinerary_landmarks[window.itinerary_landmarks.length - 1];

    directionsService.route({
        origin: { lat: window.itinerary_lat, lng: window.itinerary_lon },
        destination: { lat: lastLandmark.Latitude, lng: lastLandmark.Longitude },
        waypoints: waypts,
        optimizeWaypoints: false,
        travelMode: 'DRIVING'
    }, function (response, status) {
        if (status === 'OK') {
            directionsDisplay.setDirections(response);

            var route = response.routes[0];
            var summaryPanel = document.getElementById('itineraryRoute');
            summaryPanel.innerHTML = '';
            // For each route, display summary information.
            for (var i = 0; i < route.legs.length; i++) {
                var routeSegment = i + 1;
                summaryPanel.innerHTML += '<b>Route Segment: ' + routeSegment +
                    '</b><br>';
                summaryPanel.innerHTML += route.legs[i].start_address + ' to ';
                summaryPanel.innerHTML += route.legs[i].end_address + '<br>';
                summaryPanel.innerHTML += route.legs[i].distance.text + '<br><br>';

            }
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });
}

function updateStartLocationOnMap(event) {
    //Get the location that the user clicked.
    var clickedLocation = event.latLng;
    marker.setPosition(clickedLocation);

    setHomeMarkerLatLon();
    updateAllDistanceElements();
    indexSearch(window.searchRadius);
    //showAllLandmarkMarkersOnSearchMap();
}

function updateStartLocationOfItinerary(event) {
    //Get the location that the user clicked.
    var clickedLocation = event.latLng;
    itineraryMarker.setPosition(clickedLocation);

    setItineraryMarkerLatLon();
}

function setMarkerToCurrentLocation() {
    window.marker = new google.maps.Marker({
        position: { lat: window.home_lat, lng: window.home_lon },
        map: window.map,
        animation: google.maps.Animation.DROP,
        draggable: false
    });
}

function setRadiusOnMap() {
    if (window.mapRadius != null) {
        window.mapRadius.setMap(null);
    }
    window.mapRadius = new google.maps.Circle({
        strokeColor: '#FF0000',
        strokeOpacity: 0.8,
        strokeWeight: 2,
        fillColor: '#FF0000',
        fillOpacity: 0.35,
        map: window.map,
        center: { lat: window.home_lat, lng: window.home_lon },
        radius: window.searchRadius * 1609.34
    });

    //allow clicking on circle to update selected start point
    google.maps.event.addListener(window.mapRadius, "click", function (event) {
        updateStartLocationOnMap(event)
    });
}

function setHomeMarkerLatLon() {
    //Get location.
    var currentLocation = marker.getPosition();
    //Add lat and lng values to a field that we can save.
    window.home_lat = currentLocation.lat(); //latitude
    window.home_lon = currentLocation.lng(); //longitude
}

function setItineraryMarkerLatLon() {
    //Get location.
    var startingLocation = itineraryMarker.getPosition();
    //Add lat and lng values to a field that we can save.
    window.itinerary_lat = startingLocation.lat(); //latitude
    window.itinerary_lon = startingLocation.lng(); //longitude

    document.getElementById("itineraryStartingLatitude").value = window.itinerary_lat;
    document.getElementById("itineraryStartingLongitude").value = window.itinerary_lon;
}

function clearItineraryLandmarks() {
    window.itinerary_landmarks = [];
}

function addLandmarkToItineraryMap(landmark) {
    window.itinerary_landmarks.push(landmark);
}

function placeItineraryLandmarksOnMap() {
    window.itinerary_landmarks.forEach(item => {
        new google.maps.Marker({
            position: { lat: item.Latitude, lng: item.Longitude },
            map: window.itineraryMap,
            icon: '/images/landmark.png',
            draggable: false,
            title: item.Name
        });
    });
}

function showAllLandmarkMarkersOnSearchMap() {
    distanceElements.forEach(item => {
        new google.maps.Marker({
            position: { lat: item.Latitude, lng: item.Longitude },
            map: map,
            icon: landmarkIconImage(distanceFromCurrentLocationInMiles(item.Latitude, item.Longitude)),
            draggable: false
        });
    });
}

function landmarkIconImage(distance) {
    var result = '/images/landmark_gray.png'
    if (distance <= window.searchRadius) {
        result = '/images/landmark.png';
    }
    return result;
}

function distanceFromCurrentLocationInMiles(lat, lon) {
    var R = 6371e3; // metres
    var φ1 = toRad(window.home_lat);
    var φ2 = toRad(lat);
    var Δφ = toRad(lat - window.home_lat);
    var Δλ = toRad(lon - window.home_lon);

    var a = Math.sin(Δφ / 2) * Math.sin(Δφ / 2) +
        Math.cos(φ1) * Math.cos(φ2) *
        Math.sin(Δλ / 2) * Math.sin(Δλ / 2);
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));

    return (R * c) * 0.000621371;
}

function distanceString(distance) {
    return distance.toFixed(2) + ' miles away';
}

function updateAllDistanceElements() {
    distanceElements.forEach(item => {
        updateElementDistance(item.ElementDiv, item.Element, item.Latitude, item.Longitude)
    });
}

function updateElementDistance(elementDiv, element, latitude, longitude) {
    let distance = distanceFromCurrentLocationInMiles(latitude, longitude);
    element.innerText = distanceString(distance);
}

function addDistanceElement(elementDiv, element, latitude, longitude) {
    var distanceElement = { ElementDiv: elementDiv, Element: element, Latitude: latitude, Longitude: longitude }
    window.distanceElements.push(distanceElement);
}

function toRad(Value) {
    /** Converts numeric degrees to radians */
    return Value * Math.PI / 180;
}

function handleLocationError(browserHasGeolocation, infoWindow, pos) {
    infoWindow.setPosition(pos);
    infoWindow.setContent(browserHasGeolocation ?
        'Error: The Geolocation service failed.' :
        'Error: Your browser doesn\'t support geolocation.');
    infoWindow.open(map);
}

function indexSearch(number) {
    window.searchRadius = number;
    distanceElements.forEach(item => {
        let distance = distanceFromCurrentLocationInMiles(item.Latitude, item.Longitude);

        if (distance <= number) {
            showDiv(item.ElementDiv);
        }
        else {
            hideDiv(item.ElementDiv);
        }
    });
    setRadiusOnMap();
}

function hideDiv(element) {
    element.style.display = "none";
}

function showDiv(element) {
    element.style.display = "flex";
}

function showDistanceCalculations() {
    var divDistanceCalculation = document.getElementsByClassName('indexDistanceCalculation');
    Array.from(divDistanceCalculation).forEach(element => {
        element.style.display = "flex";
    });
}

//NavBar Things
window.onscroll = function () { myFunction() };

function myFunction() {
    var navbar = document.getElementById("navbar");
    var sticky = navbar.offsetTop;
    if (window.pageYOffset >= sticky) {
        navbar.classList.add("sticky")
    } else {
        navbar.classList.remove("sticky");
    }
}

