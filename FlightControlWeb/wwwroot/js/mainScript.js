let mymap = L.map('mapId').setView([51.505, -0.09], 1);
createMap();

func = function dataUpdate() {
    let getOptions = prepareGetAll();
    let date = getDate();
    //let url = "/api/Flights?relative_to=".concat(date).concat("&sync_all");
    let url = "api/FlightPlan"
    $.ajax({
        url: url,
        type: 'get',
        dataType: 'JSON',
        success: function (response) {
            clearLists();
            let len = response.length;
            for (let i = 0; i < len; i++) {
                if (response[i].isExternal == 1) {
                    addToExtFlight(response[i]);
                    //testFunc();
                } else {
                    addToMyFlight(response[i]);
                    //console.log(response[i]);
                }
                addToMap(response[i]);

            }
        }
    });
    
}

function createMap() {
    L.tileLayer('https://api.mapbox.com/styles/v1/{id}/tiles/{z}/{x}/{y}?access_token=pk.eyJ1Ijoib21lcm1vayIsImEiOiJja2FnaXlvbmMwNDFpMnhtb2ptdnJ3NnZhIn0._fxKcKn0NhvxL8chgMNzYQ', {
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
    let blackIcon = L.icon({
        iconUrl: 'blackAirplane.png',
        iconSize: [38, 40], // size of the icon
        iconAnchor: [22, 94], // point of the icon which will correspond to marker's location
        popupAnchor: [-3, -76] // point from which the popup should open relative to the iconAnchor
    });
    //blackIcon.className = flight.id;
    let marker1 = L.marker([flight.startLongitude, flight.startLatitude], { icon: blackIcon }).addTo(mymap);
    marker1.className = flight.id;
    marker1.on('click', showFlightDetails);




}

function showFlightDetails(event) {
    console.log(this.className);
    // get for json for certain id
    let path = [[34, 42], [40, 44]];
    var polyline = L.polyline(path, { color: 'red' }).addTo(mymap);
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
    console.log(dateStr);
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

function addFlights(flights) { 
    let flightsArray = [];
    flightsArray = JSON.parse(flights);
    
}

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
    myFlightElCl1.id = flight.id;
    myFlightElCl1.className = flight.id;
    let x = flight.id.toString() + flight.company_name; //unique name of flight
    let text = document.createTextNode(x);
    myFlightElCl1.appendChild(text);
    myFlightElCl1.onclick = showFlightDetails; //event handler

    //column 2
    myFlightElCl2.appendChild(icon);
    myFlightElCl2.onclick = testRemove; // event handler

    //children addition
    myFlightsElRow.appendChild(myFlightElCl1);
    myFlightsElRow.appendChild(myFlightElCl2);
    myFlights.appendChild(myFlightsElRow);

    //debug
    console.log(JSON.stringify(flight));
}

function addToExtFlight(flight) {
    let extFlights = document.getElementById("extFlightList");
    let extFlightsEl = document.createElement("option");
    extFlightsEl.id = flight.id;
    extFlightsEl.innerHTML = flight.id;
    extFlights.append(myFlightsEl);
    console.log(JSON.stringify(flight));
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
setInterval(func, 3000);