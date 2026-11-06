// jQuery-powered live price preview and form feedback.
$(function () {
    const form = $('#bookingForm');
    // Instant filtering of tool cards (the GET button also supports server-side search).
    $('#liveSearch').on('input', function () {
        const term = String($(this).val() || '').trim().toLowerCase();
        let visible = 0;
        $('.tool-card').each(function () {
            const match = String($(this).attr('data-search') || '').includes(term);
            $(this).toggle(match);
            if (match) visible++;
        });
        $('#visibleToolCount').text(visible);
    });
    if (!form.length) return;
    const hourly = parseFloat(form.attr('data-hourly') || '0');
    const update = function () {
        const start = new Date($('#Input_StartAt').val());
        const end = new Date($('#Input_EndAt').val());
        const hours = (end - start) / 3600000;
        if (!Number.isFinite(hours) || hours <= 0) {
            $('#quote').text('Choose an end time after the start time');
        } else {
            $('#quote').text('OMR ' + (Math.round(hours * hourly * 1000) / 1000).toFixed(3)
                + ' (' + hours.toFixed(2) + ' hours)');
        }
    };
    $('#Input_StartAt, #Input_EndAt').on('input change', update);
    update();
});
