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


function preparePost(file) {
    
    let ContentType = 'application/json;charset=utf-8'
    //let fileAsStr = JSON.stringify(file);
    let reader = new FileReader();
    let fileAsStr = JSON.stringify(reader.readAsText(file));
    //console.log(file);
    /*let jhr = new XMLHttpRequest();
    jhr.open("POST", "/api/FlightPlan", true);
    jhr.setRequestHeader("Content-Type", "application/json");
    jhr.send(jdata);*/



    return {
        "method": "POST",
        "headers": {'Content-Type': ContentType },
        "body": fileAsStr
    }
}


