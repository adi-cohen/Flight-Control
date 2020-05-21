function allowDrop(event) {
    event.preventDefault();
    document.getElementById("draganddropimage").style.visibility = "visible";
    $("#myFlightList").hide();
    $("#draganddropimage").show();
}

function dragLeave(event) {
    event.preventDefault();
    $("#draganddropimage").hide();
    $("#myFlightList").show();
}

function drop(event) {
    event.preventDefault();
    $("#draganddropimage").hide();
    $("#myFlightList").show();

    // change
    let file = event.dataTransfer.files[0];

    
    //let fileAsStr = JSON.stringify(file);

    let reader = new FileReader();
    reader.readAsText(file);

    let jdata;
    reader.onload = function () {
        jdata = reader.result.replace('/r', '');
        postData(jdata);
    }
}

    
    //request.send(JSON.stringify({
    //    "passengers": 150,
    //    "company_name": "SwissAir",
    //    "initial_location": {
    //        "longitude": 20.0,
    //        "latitude": 30.2,
    //        "date_time": "2020-12-27T01:56:21Z"
    //    },
    //    "segments": [
    //        {
    //            "longitude": 33.23,
    //            "latitde": 31.56,
    //            "timespan_seconds": 850
    //        }
    //    ]
    //}));

    //let postOptions = preparePost(file);
    //fetch("/api/FlightPlan", postOptions)
    //    .then(response => response.json())
    //    .then()
    //    .catch(error => console.log(error))


//let ContentType = 'application/json;charset=utf-8'
//function preparePost(file) {
    //let jsonObj = '{"passengers": 150,"company_name": "SwissAir", "initial_location": {"longitude": 20.0,"latitude": 30.2,"date_time": "2020-12-27T01:56:21Z"}';
    //let fileAsStr = JSON.stringify({
    //    "passengers": 150,
    //    "company_name": "SwissAir",
    //    "initial_location": {
    //        "longitude": 20.0,
    //        "latitude": 30.2,
    //        "date_time": "2020-12-27T01:56:21Z"
    //    },
    //    "segments": [
    //        {
    //            "longitude": 33.23,
    //            "latitde": 31.56,
    //            "timespan_seconds": 850
    //        }
    //    ]
    //});
    
    //console.log(file);
    //console.log("XXXXXXXXXXXXXXXXXXXXXXXXXXXXx");
    //console.log(fileAsStr)
    //return {
    //    "method": "POST",
    //    "headers": {'Content-Type': ContentType },
    //    "body": fileAsStr3


    function postData(jdata) {
        let request = new XMLHttpRequest();
        request.open("POST", "/api/FlightPlan", true);
        request.setRequestHeader("Content-Type", "application/json");
        request.send(jdata);
    }



