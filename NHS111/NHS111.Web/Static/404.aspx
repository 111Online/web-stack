<%@ Page Language="C#" %>
<%
    Response.StatusCode = 404;  
    string GaPropertyId = ConfigurationManager.AppSettings["GoogleAnalyticsContainerId"];
%>


<!DOCTYPE html>
<html lang="">
<head>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>Page not found - NHS 111 Online</title>

    <!-- css -->
    <link href="http://nhsalpha.herokuapp.com/public/stylesheets/govuk-template.css?0.12.0" media="screen" rel="stylesheet" type="text/css" />
    <link href="/Content/css_NhsUK/application.css" media="screen" rel="stylesheet" type="text/css"/>

    <!-- Javascript -->
    <script src="/scripts/vendor/modernizr-custom.js"></script>
    <script src="/scripts/jquery2.1.1.min.js"></script>
    <script src="/Scripts/jquery1.11.2-ui.min.js"></script>
    <script src="/scripts/bootstrap.min.js"></script>
    <script src="/scripts/jquery.cookie.js"></script>
    
    <script>
        $(function () {
            var userId = $.cookie("userId");
            if (!userId) $.cookie("userId", '<%=Guid.NewGuid()%>', { path: '/' });
        });
    </script>
    
</head>
<body class="">
        <!-- toggle cookie banner -->
<link href="/Content/css_NhsUK/banner.css" media="screen" rel="stylesheet" type="text/css" />
<div class="notification-banner notification-banner__hidden" id="global-cookies-banner" role="alert">
    <p class="notification-banner--inner">
        NHS 111 uses cookies to make the site simpler. <a href="/Help/Cookies" target="_blank">Find out more about cookies</a>
    </p>
</div>
<script>
                $(function () {
                    var seenCookieMessage = $.cookie("nhs111_seen_cookie_message");
                    if (!seenCookieMessage) $('#global-cookies-banner').css({ display: "block" });
                    $('#global-cookies-banner').click(function () {
                        $.cookie("nhs111_seen_cookie_message", 1, { path: '/', expires: 28 });
                        $('#global-cookies-banner').hide();
                    });
                });
</script>
    
    


    <!-- Google Tag Manager -->
    <noscript>
        <iframe src='//www.googletagmanager.com/ns.html?id=<%=GaPropertyId  %>'
                height='0' width='0' style='display:none;visibility:hidden'></iframe>
        </noscript>
        <script>
            (function(w, d, s, l, i) {
                w[l] = w[l] || []; w[l].push({
                    'gtm.start':
                new Date().getTime(), event: 'gtm.js'
            }); var f = d.getElementsByTagName(s)[0],
    j = d.createElement(s), dl = l != 'dataLayer' ? '&l=' + l : ''; j.async = true; j.src =
            '//www.googletagmanager.com/gtm.js?id=' + i + dl; f.parentNode.insertBefore(j, f);
        })(window, document, 'script', 'dataLayer', '<%=GaPropertyId %>');</script>
    <!-- End Google Tag Manager -->

    <script type="text/javascript">document.body.className = ((document.body.className) ? document.body.className + ' js-enabled' : 'js-enabled');</script>
    <div id="skiplink-container">
        <div>
            <a href="#content" class="skiplink">Skip to main content</a>
        </div>
    </div>
    <header role="banner" id="global-header">
        <div class="header-container">
            <a href="/" class="header-logo" title="Go to the NHS.UK homepage">
                <img src="/content/images/nhs-rev-logotype.jpg" alt="">
            </a>
        </div>
    </header>
    
    <!--end header-->

    <section class="outer-section"></section>

    <main id="content" role="main">
            <!-- toggle disclaimer banner-->
<link href="/Content/css_NhsUK/phase-banner.css" media="screen" rel="stylesheet" type="text/css" />
<div id="phase-banner">
    <div class="inner-block">
        <div class="phase-banner">
            <p>
                <strong class="phase-tag alpha">Alpha</strong>
                <span>
                    This is a test service.  Content on this site has <strong>not</strong> been clinically verified, and should not be relied upon for advice. <br />  <strong>If you need medical advice visit <a href="http://www.nhs.uk">www.nhs.uk</a> or call 111.</strong>
                </span>
            </p>
        </div>
    </div>
</div>

        





<h2 class="heading-large">Sorry, that page can't be found.</h2>
<p>If you clicked a link on one of our pages, we'll change it so this doesn't happen again.</p>
<p>You can return to the <a href="/">homepage</a> or use the back button in your browser to return to where you were.</p>

    </main>
    <section class="feedback-section">
    

<div class="feedback-wrapper">
    <div class="feedback-container feedback-container--open">
        <div class="feedback-banner">
            <a href="#FeedbackForm" class="feedback-dismiss"></a>
            <form id="FeedbackForm" action="/Feedback/SubmitFeedback">
                <div id="FeedbackDetails">
                    <h3 class="feedback-message feedback-title">
                        Help us improve 111 online
                    </h3>
                    <p class="feedback-message">
                        If you’d like to give your views on helping us improve 111 online, please use the form below.
                    </p>
                    <div class="feedback-form">
                        <textarea Value="" class="feedback-input" cols="20" id="Text" maxlength="8000" name="Text" placeholder="Feedback" rows="2">
</textarea>
                        <a class="feedback-btn feedback-submit" id="btnFeedback" onclick="SubmitFeedback(this);">Submit</a>
                    </div>
                </div>
                <input id="UserId" name="UserId" type="hidden" value="" />
                <input id="PageId" name="PageId" type="hidden" value="/static/404.aspx" />
            </form>
        </div>
    </div>
</div>
<script>

    $('.feedback-dismiss').click(function () {
        $('.feedback-wrapper').slideToggle();
    });

    $('.feedback-input').focus(function () {
        $(this).animate({
            //width: "70%",
            //height: "70%"
        });
    });

    $(document).ready(function () {
        var $userId = $("#FeedbackForm #UserId");
        $userId.val($.cookie("userId"));
    });

    $("#Text").keypress(function (event) {
        var keycode = (event.keyCode ? event.keyCode : event.which);
        var RETURNKEY = 13;
        if (keycode === RETURNKEY) {
            $("#btnFeedback").click();
            return false;
        }
    });

    function SubmitFeedback(btnClicked) {
        if ($("#Text").val() !== "") {
            var $form = $(btnClicked).parents("form");
            $form[0][0].value = $('<div/>').text($form[0][0].value).html();
            var detailsDiv = $("#FeedbackDetails");
            $(detailsDiv).slideToggle(600);
            $.ajax({
                type: "POST",
                url: $form.attr("action"),
                data: $form.serialize(),
                error: function (xhr, status, error) {
                    alert(error);
                    $(detailsDiv).slideToggle(600);
                    //do something about the error
                },
                success: function (response) {
                    $(detailsDiv).html('<p>' + response.Message + '</p>');
                    $(detailsDiv).slideToggle(600);
                }
            });
            return false;
        }
    }
</script>

</section>
    <footer id="footer" role="contentinfo">
        <div class="footer-wrapper">
            <nav>
                <ul>
                    <li><a href="/Help/Cookies" target="_blank">Cookies</a></li>
                    <li><a href="/Help/Privacy" target="_blank">Privacy statement</a></li>
                </ul>
            </nav>
            <div class="text">
                <p>
                    <small>
                        &copy; Crown copyright<br>
                        Content available under <a href="https://www.nationalarchives.gov.uk/doc/open-government-licence/version/3/">Open Government Licence v3.0</a>, except where otherwise stated.
                    </small>
                </p>
            </div>
        </div>
    </footer>

    <div id="global-app-error" class="app-error hidden"></div>

</body>
</html>
