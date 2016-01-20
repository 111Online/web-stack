var content = '<div id="disclaimerModal" class="disclaimerModal modal fade" tabindex="-1" role="dialog" aria-labelledby="alphaWelcome" aria-hidden="true"> \
    <div class="modal-dialog"> \
        <div class="modal-content"> \
            <div class="modal-header"> \
                <h4 class="modal-title" id="alphaWelcome">Welcome to NHS 111</h4> \
            </div> \
            <div class="modal-body"> \
                <p> \
                    This is an experimental prototype site put together by the NHS 111 digital team. \
                    It’s designed with patients at the heart of the service, and shows how the digital NHS could better link with real world services. \
                </p> \
                <div class="alert alert-warning"> \
                    <span class="exclaimaition" aria-hidden="true"></span> \
                    <p> \
                        Content on this site has <span style="text-decoration: underline;">not</span> been clinically verified, and should not be relied upon for advice. \
                    </p> \
                </div> \
                <p><strong>If you need medical advice visit <a href="http://www.nhs.uk">www.nhs.uk</a></strong> \
                </p> \
            </div> \
            <div class="modal-footer"> \
                <button type="button" class="btn btn-primary" data-dismiss="modal">Accept and close this window</button> \
            </div> \
        </div> \
    </div> \
</div>';

$(function () {
    var disclaimeraccepted = $.cookie("acceptedDisclaimer");
    $("body").append(content);
    if (!disclaimeraccepted) $('#disclaimerModal').modal('show');
    $('#aboutPrototype').click(function () {
        $('#disclaimerModal').modal('show');
    });
    $('#disclaimerModal').on('hidden.bs.modal', function () {
        $.cookie("acceptedDisclaimer", 1, { path: '/' });
    });
    $('.hide-disclaimer').click(function () {
        $('.disclaimer.alert').hide();
    });
    
});