// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

if (window.map == null) {
    window.map;
    window.marker = false;
}

if (window.home_lat == null) {
    window.home_lat; //default values
    window.home_lon; //default values
}
if (window.distanceElements == null) {
    window.distanceElements = [];
}

if (window.searchRadius == null) {
    window.searchRadius = 15;
}

function initMap() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            window.home_lat = position.coords.latitude;
            window.home_lon = position.coords.longitude;
            updateAllDistanceElements();
            showDistanceCalculations();
            updateSearchMapCenterToCurrentLocation();
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
        zoom: 12 //The zoom value.
    };

    //Create the map object.
    window.map = new google.maps.Map(document.getElementById('map'), options);

    //Listen for any clicks on the map.
    google.maps.event.addListener(map, 'click', function (event) {
        //Get the location that the user clicked.
        var clickedLocation = event.latLng;
        //If the marker hasn't been added.
        if (marker === false) {
            //Create the marker.
            marker = new google.maps.Marker({
                position: clickedLocation,
                map: map,
                draggable: true //make it draggable
            });
            //Listen for drag events!
            google.maps.event.addListener(marker, 'dragend', function (event) {
                markerLocation();
            });
        } else {
            //Marker has already been added, so just change its location.
            marker.setPosition(clickedLocation);
        }
        //Get the marker's location.
        markerLocation();

        indexSearch(window.searchRadius)
    });
}


function markerLocation() {
    //Get location.
    var currentLocation = marker.getPosition();
    //Add lat and lng values to a field that we can save.
    window.home_lat = currentLocation.lat(); //latitude
    window.home_lon = currentLocation.lng(); //longitude

    updateAllDistanceElements();
}



function mapURLFromCurrentLocation() {
    //var URLString = "https://www.google.com/maps/embed/v1/view?zoom=17&center=" + window.home_lat + "," + window.home_lon + "&key=AIzaSyBR3TDgdyT1bGXlPKIG9yF6-7WYX3ewges";
    //return URLString;
}

function updateSearchMapCenterToCurrentLocation() {
    //const searchMap = document.getElementById("map");
    //googleMap.src = mapURLFromCurrentLocation();
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
    const distanceToLocationElement = document.getElementById(element.id);
    let distance = distanceFromCurrentLocationInMiles(latitude, longitude);
    distanceToLocationElement.innerText = distanceString(distance);
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
}

function hideDiv(element) {
    var divElement = document.getElementById(element.id);
    divElement.style.display = "none";
}

function showDiv(element) {
    var divElement = document.getElementById(element.id);
    divElement.style.display = "flex";
}

function showDistanceCalculations() {
    var divDistanceCalculation = document.getElementsByClassName('indexDistanceCalculation');
    Array.from(divDistanceCalculation).forEach(element => {
        element.style.display = "flex";
    });    
}

//Load the map when the page has finished loading.
google.maps.event.addDomListener(window, 'load', initMap);