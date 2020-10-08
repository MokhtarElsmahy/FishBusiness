$((function () {
    var url;
    var redirectUrl;
    var target;

    $('body').append(`
            <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                <div class="modal-header">
                   <h6 id="deleteText" style="display:none">جارى الحذف برجاء الانتظار</h6>
                </div>
                <div class="modal-body delete-modal-body">
                    
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" id="cancel-delete">الغاء</button>
                    <button type="button" class="btn btn-danger" id="confirm-delete">حذف</button>
                </div>
                </div>
            </div>
            </div>`);

    //Delete Action
    $(".delete").on('click', (e) => {
        e.preventDefault();
        
        target = e.target;
        var Id = $(target).data('id');
        var controller = $(target).data('controller');
        var action = $(target).data('action');
        var bodyMessage = $(target).data('body-message');
        redirectUrl = $(target).data('redirect-url');

        url = "/" + controller + "/" + action + "?Id=" + Id;
        $(".delete-modal-body").text(bodyMessage);
        $("#deleteModal").modal('show');
    });

    $("#confirm-delete").on('click', () => {
        $("#deleteText").show()
        $.get(url)
            .done((result) => {
                $("#deleteText").hide()
                Swal.fire({
                    position: 'top-end',
                    icon: 'success',
                    title: 'تم الحذف بنجاح',
                    showConfirmButton: false,
                    timer: 3000
                });

                if (!redirectUrl) {
                    return $(target).parent().parent().hide("slow");
                }
             
                window.location.href = redirectUrl;
            })
            .fail((error) => {
                Swal.fire({
                    icon: 'error',
                    title: 'حدث خطأ',
                    text: 'حاول مرة اخري',
                });


                if (redirectUrl)
                    window.location.href = redirectUrl;
            }).always(() => {
                $("#deleteModal").modal('hide');
            });
    });

}()));