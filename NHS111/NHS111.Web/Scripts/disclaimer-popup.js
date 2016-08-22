var content =
    '<link href="~/Content/css_NhsUK/phase-banner.css" media="screen" rel="stylesheet" type="text/css" /> \
    <div id="disclaimerModal" class="disclaimerModal modal fade" tabindex="-1" role="dialog" aria-labelledby="alphaWelcome" aria-hidden="true"> \
        <div class="modal-dialog"> \
            <div class="modal-content"> \
                <div class="modal-header"> \
                    <h4 class="modal-title" id="alphaWelcome"> \
                            <img src="/content/images/nhs-rev-logotype.jpg" alt=""> \
                        <span style="vertical-align:middle;">111 Online</span> \
                    </h4> \
                </div> \
                <div class="modal-body"> \
                    <div class="phase-banner">  \
                        <p> \
                            <strong class="phase-tag alpha">Alpha</strong> \
                                <span> \
                                    This is a test service - your feedback will help us improve it. \
                                </span> \
                        </p> \
                    </div> \
                    <br /> \
                    <p> \
                        This is an experimental prototype site put together by the NHS 111 Online team. \
                        It’s designed with patients at the heart of the service, and shows how the digital NHS could better link with real world services. \
                    </p> \
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
    $("body").append(content);
    $('#disclaimerModal').modal(
        {
            backdrop: 'static',
            keyboard: false
        });
    $('#acceptDisclaimer').click(function () {
        $('.disclaimer.alert').hide();
    });
});