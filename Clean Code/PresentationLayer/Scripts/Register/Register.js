$(document).ready(function () {
	stripIDs('#sessions input');
	stripIDs('#certifications input');

	$('.continue, .back').click(function () {
		var button = $(this);
		button.closest('fieldset').slideUp('slow', function () {
			var step = button.data('target');
			if (step == 4) $('h2').slideUp();
			$('#step-' + step).slideDown('slow');
		});
		return false;
	});

	$('#HasBlog').change(function () {
		if ($(this).is(':checked')) {
			$('#blog-url-wrapper').slideDown();
		} else {
			$('#blog-url-wrapper').slideUp();
		}
	});

	$('#add-session').click(function () {
		//create new session row by copying last row
		var rowCopy = $('.session:last').clone(false).appendTo($('#sessions')).hide();

		//Set numbering and input names appropriately for cloned elements.
		var $numSessions = $('#NumSessions');
		setElementName(rowCopy.find('input'), 'Sessions[' + $numSessions.val() + '].Name');
		setElementName(rowCopy.find('textarea'), 'Sessions[' + $numSessions.val() + '].Description');

		//Clear values for cloned elements
		rowCopy.find('input').val('');
		rowCopy.find('textarea').html('');

		//Store the new number of sessions in a hidden field.
		$numSessions.val(parseInt($numSessions.val()) + 1);

		//Set the session number header
		var sessionNumber = rowCopy.find('.session-number');
		sessionNumber.html(parseInt(sessionNumber.html()) + 1);

		//show the result
		rowCopy.slideDown();

		if (parseInt($numSessions.val()) > 3) $('#add-session').hide(); //allowing 4 sessions max
		return false;
	});

	//This generates an extra input below the current one being typed in if the one being typed in is the last input on the list.
	//This way the user doesn't even have to click "add" to keep adding items.
	$('#certifications').on('keyup', 'input', function (event) {
		var lastInputName = $('#certifications input:last').attr('name');
		var isLastInput = lastInputName == $(this).attr('name');
		if (!isLastInput) return; //nothing to do here. We're only adding another input box if the user is changing the last one.
		if ($(this).val() == '') return; //don't add another box until someone actually inputs a value.

		//create new session row by copying last row
		var rowCopy = $('.certification:last').clone(false).appendTo($('#certifications')).hide();

		//Set numbering and input names appropriately for cloned elements.
		var $numCerts = $('#NumCerts');
		setElementName(rowCopy.find('input'), 'Certifications[' + $numCerts.val() + '].Name');

		//Clear values for cloned elements
		rowCopy.find('input').val('');

		//Store the new number of sessions in a hidden field.
		$numCerts.val(parseInt($numCerts.val()) + 1);

		//show the result
		rowCopy.slideDown('fast');

		if (parseInt($numCerts.val()) > 3) $('#add-certification').hide(); //allowing 10 certs max
		event.preventDefault();
	});

	//Necessary to avoid bug in IE 6/7 per http://stackoverflow.com/questions/2094618/changing-name-attr-of-cloned-input-element-in-jquery-doesnt-work-in-ie6-7
	function setElementName(elems, name) {
		if ($.browser.msie === true) {
			$(elems).each(function () {
				this.mergeAttributes(document.createElement("<input name='" + name + "'/>"), false);
			});
		} else {
			$(elems).attr('name', name);
		}
	}

	//Strip ids in session fields since we're not using them and they just make cloning trickier
	function stripIDs(selector) {
		$.each($(selector), function () {
			$(this).attr('id', '');
		});
	}
});