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

    let file = event.dataTransfer.files[0];
    let postOptions = preparePost(file);
    fetch("/api/FlightPlan", postOptions)
        .then(response => response.json())
        .then()
        .catch(error => console.log(error))
}

let ContentType = 'application/json;charset=utf-8'
function preparePost(file) {
    let jsonObj = '{"passengers": 150,"company_name": "SwissAir", "initial_location": {"longitude": 20.0,"latitude": 30.2,"date_time": "2020-12-27T01:56:21Z"}';
    let fileAsStr = JSON.stringify(jsonObj);
    //console.log(file);
    console.log("XXXXXXXXXXXXXXXXXXXXXXXXXXXXx");
    console.log(fileAsStr)
    return {
        "method": "POST",
        "headers": {'Content-Type': ContentType },
        "body": fileAsStr
    }
}


