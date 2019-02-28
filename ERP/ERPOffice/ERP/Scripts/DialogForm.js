
$(function () {
    // Don't allow browser caching of forms
    $.ajaxSetup({ cache: false });


      // Wire up the click event of any dialog links
    $('.dialogLinkDeleteOption').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
        $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {
                    "Delete": function () {
                        // Manually submit the form                        
                        var form = $('form', this);
                        $(form).submit();
                    },
                    "Cancel": function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
            $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });

    // Wire up the click event of any dialog links
    $('.dialogLink').live('click', function () {
    
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
        $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                height: dialogHeight,
                width: dialogWidth,
                title: dialogTitle,
                buttons: {
                    "Save": function () {
                        //// Manually submit the form                        
                        var form = $('form', this);
                        $(form).submit();
                        
                    },
                    "Cancel": function () {                       
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
            $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
       
    });

    $('.dialogLinkAddDriver').live('click', function () {

        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
            $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                height: dialogHeight,
                width: dialogWidth,
                title: dialogTitle,
                buttons: {
                    "Save": function () {
                        //// Manually submit the form                        
                        var form = $('form', this);
                        $(form).submit();

                    },
                    "Cancel": function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
            $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;

    });

      // Wire up the click event of any dialog links
    $('.dialogLinkList').live('click', function () {
    
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
        $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                height: dialogHeight,
                width: dialogWidth,
                title: dialogTitle,

                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
            $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
       
    });


    
     $('.dialogLinkApprove').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
       $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {
                    "Approve": function () {
                        // Manually submit the form                        
                        var form = $('form', this);
                        $(form).submit();
                    }
//                    "Cancel": function () {
//                        $(this).dialog('close');
//                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
           $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });

    // Wire up the click event of any dialog links    
    $('.dialogLinkEdit').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
       $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {
                    "Update": function () {
                        // Manually submit the form                        
                        var form = $('form', this);
                        $(this).dialog("destroy");
                        $(form).submit();
                    },
                    "Cancel": function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
           $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });



    // Wire up the click event of any dialog links
    $('.dialogLinkUpdate').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
       $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {
                    "Update": function () {
                        // Manually submit the form                        
                        var form = $('form', this);
                        $(form).submit();
                    },
                    "Cancel": function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
           $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });

    $('.dialogLinknotepad').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        //function CloseAndRefresh() {
        //    opener.location.reload(true);
        //    self.submit();
        //}

        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
            $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {
                    "Update": function () {
                        // Manually submit the form                        
                        var form = $('form', this);
                        //$(this).dialog("destroy");
                        //CloseAndRefresh();
                        $(form).submit();
                    },
                    "Cancel": function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });

            $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });
    

    $('.dialogLinkpodnotepad').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        //function CloseAndRefresh() {
        //    opener.location.reload(true);
        //    self.submit();
        //}

        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
            $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {                    
                    "Cancel": function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });

            $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });


    // Wire up the click event of any dialog links
    $('.dialogLinknotepadPDA').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
            $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {
                     "Cancel": function () {
                        // Manually submit the form                        
                        var form = $('form', this);
                        $(this).dialog('close');
                    },

                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
            $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });

    // Wire up the click event of any dialog links
    $('.dialogLinkDelete').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
        $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {
                    "Delete": function () {
                        // Manually submit the form                        
                        var form = $('form', this);
                        $(this).dialog("destroy");
                        $(form).submit();
                    },
                    "Cancel": function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
            
            $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });   
    

    $('.dialogLinkRemove').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
            $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {
                    "Remove": function () {
                        // Manually submit the form                        
                        var form = $('form', this);
                        $(form).submit();
                    },
                    "Cancel": function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
            $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });

    // Wire up the click event of any dialog links
    $('.dialogLinkClose').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
        $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {
                    "Cancel": function () {
                        // Manually submit the form                        
                        var form = $('form', this);
                        $(this).dialog('close');
                    },

                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
            $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });
});

 // Wire up the click event of any dialog links
    $('.dialogLinkAdd').live('click', function () {
        var element = $(this);

        // Retrieve values from the HTML5 data attributes of the link        
        var dialogTitle = element.attr('data-dialog-title');
        var updateTargetId = '#' + element.attr('data-update-target-id');
        var updateUrl = element.attr('data-update-url');
        var dialogHeight = element.attr('DialogHeight');
        var dialogWidth = element.attr('DialogWidth');

        // Generate a unique id for the dialog div
        var dialogId = 'uniqueName-' + Math.floor(Math.random() * 1000)
        var dialogDiv = "<div id='" + dialogId + "'></div>";


        // Load the form into the dialog div
        $(dialogDiv).load(this.href, function () {
       $('#Loader').showLoading();
            $(this).dialog({
                modal: true,
                resizable: false,
                title: dialogTitle,
                height: dialogHeight,
                width: dialogWidth,
                buttons: {
                    "Update": function () {
                        // Manually submit the form                        
                        var form = $('form', this);
                        $(form).submit();
                    },
                    "Cancel": function () {
                        $(this).dialog('close');
                    }
                },
                close: function () {
                    // Remove all qTip tooltips
                    $('.qtip').remove();

                    // It turns out that closing a jQuery UI dialog
                    // does not actually remove the element from the
                    // page but just hides it. For the server side 
                    // validation tooltips to show up you need to
                    // remove the original form the page
                    $('#' + dialogId).remove();
                }
            });
           $('#Loader').hideLoading();
            // Enable client side validation
            $.validator.unobtrusive.parse(this);

            // Setup the ajax submit logic
            wireUpForm(this, updateTargetId, updateUrl);
        });
        return false;
    });


   



function wireUpForm(dialog, updateTargetId, updateUrl) {

    $('form', dialog).submit(function () {
    $('#Loader').showLoading();
        // Do not submit if the form
        // does not pass client side validation
        if (!$(this).valid())
            {
            $('#Loader').hideLoading();
            return false;            
            }

        // Client side validation passed, submit the form
        // using the jQuery.ajax form
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                // Check whether the post was successful
                if (result.success) {
                    // Close the dialog 
                    $(dialog).dialog('close');

                    // Reload the updated data in the target div
                    $(updateTargetId).load(updateUrl);
                    $('#Loader').hideLoading();
                } else {
                    // Reload the dialog to show model errors                    
                    $(dialog).html(result);
                    
                    // Enable client side validation
                    $.validator.unobtrusive.parse(dialog);
                    
                    // Setup the ajax submit logic
                    wireUpForm(dialog, updateTargetId, updateUrl);
                    $('#Loader').hideLoading();
                }

                if (result.isRedirect) {
                 $(dialog).dialog('close');
                 window.location.href = result.redirectUrl;
                 $('#Loader').hideLoading();
                        }
                        
            }
        });        
        return false;
        
    });

   
 

}

  