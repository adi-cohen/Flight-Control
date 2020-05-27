function allowDrop(event) {
	event.preventDefault();
	document.getElementById("draganddropimage").style.visibility = "visible";
	$("#flightList").hide();
	$("#draganddropimage").show();
}

function dragLeave(event) {
	event.preventDefault();
	$("#draganddropimage").hide();
	$("#flightList").show();
}

function sendFile(data) {
	let request = new XMLHttpRequest();
	request.open("POST", "/api/FlightPlan", true);
	request.setRequestHeader("Content-Type", "application/json");
	request.onreadystatechange = function () {
		if (request.readyState === 4 && request.status >= 400) {
			tempAlert("JSON syntax error. Please check your file.", 5000);
        }	
	};
	request.send(data);
}

function tempAlert2(msg, duration) {
	let alertBox = document.createElement("div");
	alertBox.setAttribute("style", "position:absolute;top:2%;left:2%;background-color:red;font-size:large;border-radius: 5px;");
	alertBox.innerHTML = "### Error: " + msg + " ###";
	setTimeout(function () {
		alertBox.parentNode.removeChild(alertBox);
	}, duration);
	document.body.appendChild(alertBox);
}

function drop(event) {
	event.preventDefault();
	$("#draganddropimage").hide();
	$("#flightList").show();
	let file = event.dataTransfer.files[0];
	let reader = new FileReader();
	reader.readAsText(file);
	let data;
	reader.onload = function () {
		data = reader.result.replace('/r', '');
		sendFile(data);
	};
}
