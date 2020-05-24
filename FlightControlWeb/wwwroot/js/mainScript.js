let mymap = L.map('mapId').setView([51.505, -0.09], 1);
let polyline;
let deletedIsSelected;
let className;
mymap.on('click', function () {
    if (polyline) {
        polyline.remove(mymap);
        $('.flightDetails').html('');
        if (selectedId) {
            document.getElementById(selectedId).style.background = "";
            selectedId = 0;
        }
        
    }
});
let layerGroup;
let blackIcon = L.icon({
    iconUrl: 'pictures/blackAirplane.png',
    iconSize: [38, 40], // size of the icon
    iconAnchor: [26, 26], // point of the icon which will correspond to marker's location
    popupAnchor: [-3, -76] // point from which the popup should open relative to the iconAnchor
});
let greenIcon = L.icon({
    iconUrl: 'pictures/greenAirplane.png',
    iconSize: [38, 40], // size of the icon
    iconAnchor: [26, 26], // point of the icon which will correspond to marker's location
    popupAnchor: [-3, -76] // point from which the popup should open relative to the iconAnchor
});
let selectedIcon;
let selectedId;
createMap();
node.textContent = "Some error message" // To draw attention 
node.style.color = "red";

let func = function dataUpdate() {
    //let getOptions = prepareGetAll();
    let date = getDate();
    //console.log(date);
    let url = "/api/Flights?relative_to=".concat(date).concat("&sync_all");
    //let url = "http://ronyut.atwebpages.com/ap2/api/Flights?relative_to=".concat(date);

    //console.log("url is: " + url);
    //let url = "api/FlightPlan"
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            clearLists();
            if (layerGroup) {
                layerGroup.clearLayers();
            }
            
            let len = response.length;
            for (let i = 0; i < len; i++) {
                if (response[i].is_external == 1) {
                    addToExtFlight(response[i]);
                    //testFunc();
                } else {
                    addToMyFlight(response[i]);
                    //console.log(response[i]);
                }

                addToMap(response[i]);

            }
        },
        error: function (error) {
            tempAlert("Can not sync flight from the servers.", 5000);
        }
    });
    
}

function createMap() {
    layerGroup = L.layerGroup().addTo(mymap);
    leyer = L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1Ijoib21lcm1vayIsImEiOiJja2FnaXlvbmMwNDFpMnhtb2ptdnJ3NnZhIn0._fxKcKn0NhvxL8chgMNzYQ', {
        attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
        maxZoom: 18,
        minZoom: 1,
        id: 'mapbox/streets-v11',
        tileSize: 512,
        zoomOffset: -1,
        accessToken: 'pk.eyJ1Ijoib21lcm1vayIsImEiOiJja2FnaXlvbmMwNDFpMnhtb2ptdnJ3NnZhIn0._fxKcKn0NhvxL8chgMNzYQ'
    }).addTo(mymap);
}



function addToMap(flight) {
    let marker1;
    if (flight.flight_id == selectedId) {
        marker1 = L.marker([flight.longitude, flight.latitude], { icon: greenIcon }).addTo(layerGroup);
    } else {
        marker1 = L.marker([flight.longitude, flight.latitude], { icon: blackIcon }).addTo(layerGroup);
    }
    //let marker1 = L.marker([flight.longitude, flight.latitude]).addTo(mymap);

    marker1.className = flight.flight_id;
    marker1.id = "marker" + flight.flight_id;
    marker1.on('click', flightSelected);



}

function flightSelected(event) {
    
    let url = "/api/FlightPlan/".concat(this.className);
    if (selectedId) {
       document.getElementById(selectedId).style.background = "";
    }
   
    selectedId = this.className;
    $.ajax({
        url: url,
        type: 'GET',
        dataType: 'JSON',
        success: function (response) {
            document.getElementById(selectedId).style.background = "#66d9ff";
            showFlightDetails(response);
        },
        error: function (error) {
            tempAlert("Can not get flight details from server. Please try again later.", 5000);
            console.log(error);
        }
    });
}
function showFlightDetails(details) {

    drawPath(details.segments, details.initial_location);
    //changeIcon(details);
    writeDetails(details);
}

/*function changeIcon() {
    if (selectedIcon) {
        selectedIcon.remove();
        //todo: change back to black icon.
    }
    selectedIcon = 

}*/

function writeDetails(details) {
    let startPoint = document.getElementById("startPointID");
    let endPoint = document.getElementById("endPointID");
    let departureTime = document.getElementById("departureTimeID");
    let arrivalTime = document.getElementById("arrivalTimeID");
    let companyName = document.getElementById("companyNameID");
    let passengersCount = document.getElementById("passengersCountID");
    // get time of the flight in seconds
    let len = details.segments.length;
    let secondsSum = 0;
    for (let i = 0; i < len; i++) {
        secondsSum += details.segments[i].timespan_seconds;
    }
    // print details
    startPoint.innerHTML = "start Point: (" + details.initial_location.longitude
        + "," + details.initial_location.latitude + ")";
    endPoint.innerHTML = "end Point: (" + details.segments[details.segments.length - 1].longitude
        + "," + details.segments[details.segments.length - 1].latitude + ")";
    departureTime.innerHTML = "departure time: " + details.initial_location.date_time.replace('T', ' ').replace('Z', '');
    arrivalTime.innerHTML = "arrival time: " + getFinishTime(details.initial_location.date_time, secondsSum);
    companyName.innerHTML = "company name time: " + details.company_name;
    passengersCount.innerHTML = "passengers: " + details.passengers;

   
   
}

function getFinishTime(startTime, seconds) {
    let time = new Date(startTime);
    time = new Date(time.getTime() + (seconds * 1000)).toISOString();
    time = time.toString().split('.')[0].replace('T', ' ');
    return time;
}

function drawPath(segments, initial_location) {
    if (polyline) {
        polyline.remove(mymap);
    }

    let lon = initial_location.longitude;
    let lat = initial_location.latitude;
    let len = segments.length;
    let path = [ [lon,lat] ];
    for (let i = 0; i < len; i++) {
        path.push([segments[i].longitude, segments[i].latitude]);
    }
    polyline = L.polyline(path, { color: 'red' });
    polyline.addTo(mymap);
    mymap.fitBounds(polyline.getBounds());

}

function getDate() {
    let date = new Date();
    
    let year = date.getUTCFullYear().toString();
    let month = ('0' + date.getUTCMonth().toString()).substr(-2);
    let day = ('0' + date.getUTCDay().toString()).substr(-2);
    let hour = ('0' + date.getUTCHours().toString()).substr(-2);
    let minute = ('0' + date.getUTCMinutes().toString()).substr(-2);
    let second = ('0' + date.getUTCSeconds().toString()).substr(-2);

    let dateStr = year + '-' + month + '-' + day + 'T' + hour + ':' + minute + ':' + second + 'Z';
    //console.log(dateStr);
    return dateStr;
}

function prepareGetAll() {
    let ContentType = 'application/json;charset=utf-8'
    //let flight = JSON.stringify(file);
    return {
        "method": "GET",
        "headers": { 'Content-Type': ContentType },
        //"body": fileAsStr
    }
}

//function addFlights(flights) { 
//    let flightsArray = [];
//    flightsArray = JSON.parse(flights);
    
//}

function addToMyFlight(flight) {
    let myFlights = document.getElementById("myFlightList"); //body
    let myFlightsElRow = document.createElement("tr"); //row
    let myFlightElCl1 = document.createElement("td"); //name of flight
    let myFlightElCl2 = document.createElement("td"); //icon

    //icon definintion
    let icon = document.createElement("i");
    icon.className = "material-icons";
    icon.style = "font-size:24px;"
    icon.innerText = "delete";

    //column 1
    myFlightsElRow.id = flight.flight_id;
    myFlightElCl1.className = flight.flight_id;
    let x = flight.company_name + " " + flight.flight_id.toString(); //unique name of flight
    let text = document.createTextNode(x);
    myFlightElCl1.appendChild(text);
    myFlightElCl1.onclick = flightSelected; //event handler

    //column 2
    myFlightElCl2.className = flight.flight_id
    myFlightElCl2.appendChild(icon);
    myFlightElCl2.onclick = removeFlight; // event handler

    if (flight.flight_id == selectedId) {
        myFlightsElRow.style.background = "#66d9ff";
    }

    //children addition
    myFlightsElRow.appendChild(myFlightElCl1);
    myFlightsElRow.appendChild(myFlightElCl2);
    myFlights.appendChild(myFlightsElRow);



}

function addToExtFlight(flight) {
    let extFlights = document.getElementById("extFlightList"); //body
    let extFlightsElRow = document.createElement("tr"); //row
    let extFlightElCl1 = document.createElement("td"); //name of flight

    //column 1
    extFlightsElRow.id = flight.flight_id;
    extFlightElCl1.className = flight.flight_id;
    let x = flight.company_name + " " + flight.flight_id.toString(); //unique name of flight
    let text = document.createTextNode(x);
    extFlightElCl1.appendChild(text);
    extFlightElCl1.onclick = flightSelected; //event handler

    if (flight.flight_id == selectedId) {
        extFlightsElRow.style.background = "#66d9ff";
    }

    //children addition
    extFlightsElRow.appendChild(extFlightElCl1);
    extFlights.appendChild(extFlightsElRow);







   /* //code for select-option
    let extFlights = document.getElementById("extFlightList");
    let extFlightsEl = document.createElement("option");
    extFlightsEl.id = flight.flight_id;
    extFlightsEl.innerHTML = flight.flight_id;
    extFlights.append(myFlightsEl);
    console.log(JSON.stringify(flight));*/
}

function clearLists() {
    $("#myFlightList").empty();
    $("#extFlightList").empty();
}

function testGetDetails(event) {
    console.log("you asked for details!");
}

function testRemove(event) {
    console.log("you asked for Deletion!");
}

function removeFlight(event) {
    
    //mymap.removeLayer(document.getElementById("marker" + this.className));
    
    let url = "/api/Flights/".concat(this.className);
    deletedIsSelected = (this.className == selectedId);
    className = this.className;
    //selectedId = this.className;
    $.ajax({
        url: url,
        type: 'DELETE',
        //dataType: 'JSON',
        success: function (response) { 
            $('.' + className).hide();
            if (deletedIsSelected) {
                document.getElementById(selectedId).style.background = "";
                selectedId = 0;
                $('.flightDetails').html('');
                polyline.remove(mymap);
            }
        },
        error: function (error) {
            tempAlert("Can not delete flight from server. Please try again later.", 5000);
        }
    });
}

function tempAlert(msg, duration) {
    let alertBox = document.createElement("div");
    alertBox.setAttribute("style", "position:absolute;top:18%;left:20%;background-color:red;font-size:large;border-radius: 5px;");
    alertBox.innerHTML = "### Error: " + msg + " ###";
    setTimeout(function () {
        alertBox.parentNode.removeChild(alertBox);
    }, duration);
    document.body.appendChild(alertBox);
}

setInterval(func, 3000);
