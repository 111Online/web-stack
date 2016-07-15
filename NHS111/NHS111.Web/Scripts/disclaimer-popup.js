var content =
    '<link href="~/Content/css_NhsUK/phase-banner.css" media="screen" rel="stylesheet" type="text/css" /> \
    <div id="disclaimerModal" class="disclaimerModal modal fade" tabindex="-1" role="dialog" aria-labelledby="alphaWelcome" aria-hidden="true"> \
        <div class="modal-dialog"> \
            <div class="modal-content"> \
                <div class="modal-header"> \
                    <h4 class="modal-title" id="alphaWelcome"> \
                        <a href="/" class="header-logo" title="Go to the NHS.UK homepage"> \
                            <img src="/content/images/nhs-rev-logotype.svg" alt=""> \
                        </a> \
                        NHS 111 Online \
                    </h4> \
                </div> \
                <div class="modal-body"> \
                    <div class="phase-banner">  \
                        <p> \
                            <strong class="phase-tag alpha">Alpha</strong> \
                                <span> \
                                    This is a new service - your feedback will help us improve it. \
                                </span> \
                        </p> \
                    </div> \
                    <br /> \
                    <p> \
                        This is an experimental prototype site put together by the NHS 111 Online team. \
                        It’s designed with patients at the heart of the service, and shows how the digital NHS could better link with real world services. \
                    </p> \
                    <div class="alert alert-warning"> \
                        <span class="exclaimaition" aria-hidden="true"></span> \
                        <p> \
                            Content on this site has <strong>not</strong> been clinically verified, and should not be relied upon for advice. \
                        </p> \
                    </div> \
                    <p><strong>If you need medical advice visit <a href="http://www.nhs.uk">www.nhs.uk</a> or call 111.</strong> \
                    </p> \
                </div> \
                <div class="modal-footer"> \
                    <button type="button" class="button" data-dismiss="modal" id="acceptDisclaimer">Accept and close this window</button> \
                </div> \
            </div> \
        </div> \
    </div>';

$(function () {
    var disclaimeraccepted = $.cookie("nhs111_accepted_disclaimer_message");
    $("body").append(content);
    if (!disclaimeraccepted) $('#disclaimerModal').modal(
        {
            backdrop: 'static',
            keyboard: false
        });
    $('#acceptDisclaimer').click(function () {
        $.cookie("nhs111_accepted_disclaimer_message", 1, { path: '/' });
        $('.disclaimer.alert').hide();
    });
});