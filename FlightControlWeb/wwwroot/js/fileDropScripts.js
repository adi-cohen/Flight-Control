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
    let fileAsStr = JSON.stringify(file);
    return {
        "method": "POST",
        "headers": {'Content-Type': ContentType },
        "body": fileAsStr
    }
}


