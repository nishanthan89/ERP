//Validate date control when a user type the date in the control

function ValidateDateInput(obj) {
    obj.value = obj.value.trim();
    //alert("start");
    if (window.event.keyCode == 46) { // Delete button pressed
        //alert("window.event.keyCode == 46");
        return true;
    }
    else if (window.event.keyCode == 8) { // Backspace button pressed
        //alert("window.event.keyCode == 8");
        return true;
    }
    else if (obj.value.length >= 10) {
        //alert("obj.value.length >= 10");
        return false; // don't limit to 10 incase the user has to insert chars.
    }
    else if (window.event.keyCode == 47 && (obj.value.length == 2 || obj.value.length == 5)) {
        //alert("window.event.keyCode == 47");
        return true;
    }
    else if (window.event.keyCode < 48 || window.event.keyCode > 57) {
        //alert("window.event.keyCode < 48 || window.event.keyCode > 57");
        return false;
    }
    else if (obj.value.length == 1 || obj.value.length == 4) {
        //alert("obj.value.length == 1 || obj.value.length == 4");
        obj.value += String.fromCharCode(window.event.keyCode, 47);
        return false;
    } else {
        //alert("else");
        return true
    }
}


//Validate time colon between hour and mins

function ValidateColon(obj) {

    //if (window.event.keyCode == 8) {
    if (obj.value.length == 2) {
        obj.value = obj.value + ':' + '00';
    }
    else if (obj.value.length == 4) {
        obj.value = obj.value.substring(0, 2) + ':' + obj.value.substring(2);
        return true;
    } else {
        return true;
    }
}