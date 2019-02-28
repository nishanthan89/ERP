function successMessage(msg, type) {    

    switch (type) {
        case 'Success':
            //sweetAlert(msg);
            var msgDiv = '<div class="alert alert-success alert-dismissable" id="success-alert">' +
                            '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>' + msg +
                        '</div>';
            $('#message').html(msgDiv);
            $('#message').show();
            $("#success-alert").fadeTo(2000, 500).delay(8000, function () {
                $("#message").hide();
            });
            break;
        case 'Error':
            //sweetAlert(msg);
            var msgDiv = '<div class="alert alert-danger alert-dismissable">' +
                            '<button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>' + msg +
                        '</div>';
            $('#message').html(msgDiv);
            $('#message').show();
            break;
        case 'Info':
            break;

    }
    
}