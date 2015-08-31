﻿$(function () {
    
    $.ajaxSetup({ cache: false });
    //on click to open modal
    $("a[data-modal]").on("click", function (e) {
         

        // hide dropdown if any
        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');
        
        //when modal is fnshed load
        $('#myModalContent').load(this.href, function () {
            //invoke bootstrap-switch method for the modals
            try {
                /*++*/$('[type="checkbox"]').bootstrapSwitch(); 
                //$("[name='my-checkbox']").bootstrapSwitch();
            } catch (eXC) { 
            }
             
            $('[type="role"]').multiselect();
                
             
            $('#myModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');

            bindForm(this);
        });

        return false;
    });


});
//called when save is pressed
function bindForm(dialog) {
    
    $('form', dialog).submit(function () {
        //window.alert("sub");
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    //Refresh
                    location.reload();
                } else {
                    $('#myModalContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}