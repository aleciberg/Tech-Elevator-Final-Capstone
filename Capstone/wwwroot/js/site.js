// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.home_lat = 39.997863; //default values
window.home_lon = -83.042820; //default values
window.distanceElements = [];

function initMap() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            home_lat = position.coords.latitude;
            home_lon = position.coords.longitude;
            updateAllDistanceElements();
        }, function () {
            //handleLocationError(true, infoWindow, map.getCenter());
        });
    } else {
        // Browser doesn't support Geolocation
        //handleLocationError(false, infoWindow, map.getCenter());
    }   
}


function distanceFromCurrentLocationInMiles(lat, lon) {
    var R = 6371e3; // metres
    var φ1 = toRad(home_lat);
    var φ2 = toRad(lat);
    var Δφ = toRad(lat - home_lat);
    var Δλ = toRad(lon - home_lon);

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
        const distanceToLocationElement = document.getElementById(item.element.id);
        let distance = distanceFromCurrentLocationInMiles(item.latitude, item.longitude);
        distanceToLocationElement.innerText = distanceString(distance);
    });
}

function updateElementDistance(element, latitude, longitude) {
    const distanceToLocationElement = document.getElementById(element.id);
    let distance = distanceFromCurrentLocationInMiles(latitude, longitude);
    distanceToLocationElement.innerText = distanceString(distance);

    addDistanceElement(element, latitude, longitude);
}

function addDistanceElement(element, latitude, longitude) {
    var distanceElement = { Element: element, Latitude: latitude, Longitude: longitude }
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