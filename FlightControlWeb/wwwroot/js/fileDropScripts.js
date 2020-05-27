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

function postData(jdata) {
	let request = new XMLHttpRequest();
	request.open("POST", "/api/FlightPlan", true);
	request.setRequestHeader("Content-Type", "application/json");
	request.send(jdata);
	/*if (request.status == ) {

	}*/
}

function drop(event) {
	event.preventDefault();
	$("#draganddropimage").hide();
	$("#flightList").show();
	let file = event.dataTransfer.files[0];
	let reader = new FileReader();
	reader.readAsText(file);
	let jdata;
	reader.onload = function () {
		jdata = reader.result.replace('/r', '');
		postData(jdata);
	};
}
