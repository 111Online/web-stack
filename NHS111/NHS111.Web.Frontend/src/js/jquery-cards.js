/**
 * This file has the JavaScript for interacting with the map from service list goto cards.
 * entry-map.js is the file for the map itself
 */
jQuery(function () {
    $('.card__directions--fallback').hide()
    $('.card__directions').show()

    // This ensures the map loads properly and that JS is enabled
    var gotoDetails = $("details:not([open])").has(".cards--goto")
    if (gotoDetails.length) gotoDetails.one("click", loadMap)
    else if ($(".cards--goto").length) loadMap()

    function loadMap() {
        var iframe = document.createElement('iframe')
        iframe.src = '/map/'
        iframe.setAttribute('role', 'presentation')
        iframe.className += ' service-map'
        $(iframe).insertBefore('.cards--goto')

        $(iframe).one('load', function () {
            $(".cards--goto .card, .cards--goto .card__marker-link").css('display', 'block')
            $(".cards--goto .card, .cards--goto .card__marker-link").on("click", function () {
                var index = $(this).data('index')
                if (index == undefined) index = $(this).parent().data('index')
                setActive(index)
                $('iframe')[0].contentWindow.frames.setActive(index)
            })
            $(iframe).show()
            $(iframe)[0].contentWindow.frames.initialise(mapServices, currentPostcode)
        })
    }

    // These functions below are exposed globally so that the map inside iframe
    // is able to interact with the cards.

    window.viewMaps = function (index) {
        $('.card[data-index=' + index + '] form').submit()
    }

    window.setActive = function (index) {
        $('.cards--goto .card').attr('style', '');
        $($('.cards--goto .card')[index]).attr('style', 'border-color: #005eb8;');
    }

    window.getDirections = function (index) {
        var form = $(`.card[data-index=${index}] form`)
        var arr = form.serializeArray()
        var data = {}
        arr.map((val, index) => {
            data[val.name] = val.value
        })

        $.ajax({
            url: location.origin + '/Outcome/LogSelectedService',
            data: data,
            method: 'POST'
        })
    }

    $('.card__directions').on('click', function (e) {
        getDirections($(this).parents('.card').data('index'))
    })
})
