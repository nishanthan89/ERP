
    $(document).ready(function () {
        var someVarName = localStorage.getItem("myClass");
        if (someVarName != 'skin-green sidebar-mini sidebar-collapse') {
            $("#myElement").removeClass("sidebar-collapse");
            $("#myElement").addClass("skin-green sidebar-mini");
        }
        else {
            //$("#myElement").removeClass("skin-green sidebar-mini");
            $("#myElement").addClass("sidebar-collapse");
        }
    });

$('#myElement').click(function () {
    var y = document.getElementById('myElement').className;
    localStorage.setItem("myClass", y); 
    //var someVarName = localStorage.getItem("myClass");
    //alert(someVarName)
});
