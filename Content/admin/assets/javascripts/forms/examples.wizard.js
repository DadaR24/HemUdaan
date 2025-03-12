/*
Name: 			Forms / Wizard - Examples
Written by: 	Okler Themes - (http://www.okler.net)
Theme Version: 	1.5.1
*/

(function($) {

	'use strict';

	/*
	Wizard #1
	*/
	var $w1finish = $('#w1').find('ul.pager li.finish'),
		$w1validator = $("#w1 form").validate({
			highlight: function (element) {
				$(element).closest('.form-group').removeClass('has-success').addClass('has-error');
			},
			success: function (element) {
				$(element).closest('.form-group').removeClass('has-error');
				$(element).remove();
			},
			errorPlacement: function (error, element) {
				element.parent().append(error);
			}
		});

	$w1finish.on('click', function (ev) {
		ev.preventDefault();
		var validated = $('#w1 form').valid();
		if (validated) {
			new PNotify({
				title: 'Congratulations',
				text: 'You completed the wizard form.',
				type: 'custom',
				addclass: 'notification-success',
				icon: 'fa fa-check',
				stack: {
					dir1: 'Top',
					dir2: 'left',
					firstpos1: 25,
					firstpos2: 25
				}
			});
		}
	});


	$('#w1').bootstrapWizard({
		tabClass: 'wizard-steps',
		nextSelector: 'ul.pager li.next',
		previousSelector: 'ul.pager li.previous',
		firstSelector: null,
		lastSelector: null,
		onNext: function( tab, navigation, index, newindex ) {
			var validated = $('#w1 form').valid();
			if( !validated ) {
				$w1validator.focusInvalid();
				return false;
			}
		},
		onTabClick: function( tab, navigation, index, newindex ) {
			if ( newindex == index + 1 ) {
				return this.onNext( tab, navigation, index, newindex);
			} else if ( newindex > index + 1 ) {
				return false;
			} else {
				return true;
			}
		},
		onTabChange: function( tab, navigation, index, newindex ) {
			var totalTabs = navigation.find('li').size() - 1;
			$w1finish[ newindex != totalTabs ? 'addClass' : 'removeClass' ]( 'hidden' );
			$('#w1').find(this.nextSelector)[ newindex == totalTabs ? 'addClass' : 'removeClass' ]( 'hidden' );
		}
	});

	/*
	Wizard #2
	*/
	var $w2finish = $('#w2').find('ul.pager li.finish'),
		$w2validator = $("#w2 form").validate({
		highlight: function(element) {
			$(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		},
		success: function(element) {
			$(element).closest('.form-group').removeClass('has-error');
			$(element).remove();
		},
		errorPlacement: function( error, element ) {
			element.parent().append( error );
		}
	});

	$w2finish.on('click', function( ev ) {
		ev.preventDefault();
		var validated = $('#w2 form').valid();
		if ( validated ) {
			new PNotify({
				title: 'Congratulations',
				text: 'You completed the wizard form.',
				type: 'custom',
				addclass: 'notification-success',
				icon: 'fa fa-check'
			});
		}
	});

	$('#w2').bootstrapWizard({
		tabClass: 'wizard-steps',
		nextSelector: 'ul.pager li.next',
		previousSelector: 'ul.pager li.previous',
		firstSelector: null,
		lastSelector: null,
		onNext: function( tab, navigation, index, newindex ) {
			var validated = $('#w2 form').valid();
			if( !validated ) {
				$w2validator.focusInvalid();
				return false;
			}
		},
		onTabClick: function( tab, navigation, index, newindex ) {
			if ( newindex == index + 1 ) {
				return this.onNext( tab, navigation, index, newindex);
			} else if ( newindex > index + 1 ) {
				return false;
			} else {
				return true;
			}
		},
		onTabChange: function( tab, navigation, index, newindex ) {
			var totalTabs = navigation.find('li').size() - 1;
			$w2finish[ newindex != totalTabs ? 'addClass' : 'removeClass' ]( 'hidden' );
			$('#w2').find(this.nextSelector)[ newindex == totalTabs ? 'addClass' : 'removeClass' ]( 'hidden' );
		}
	});

	/*
	Wizard #3
	*/
	var $w3finish = $('#w3').find('ul.pager li.finish'),
		$w3validator = $("#w3 form").validate({
		highlight: function(element) {
			$(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		},
		success: function(element) {
			$(element).closest('.form-group').removeClass('has-error');
			$(element).remove();
		},
		errorPlacement: function( error, element ) {
			element.parent().append( error );
		}
	});

	$w3finish.on('click', function( ev ) {
		ev.preventDefault();
		var validated = $('#w3 form').valid();
		if ( validated ) {
			new PNotify({
				title: 'Congratulations',
				text: 'You completed the wizard form.',
				type: 'custom',
				addclass: 'notification-success',
				icon: 'fa fa-check'
			});
		}
	});

	$('#w3').bootstrapWizard({
		tabClass: 'wizard-steps',
		nextSelector: 'ul.pager li.next',
		previousSelector: 'ul.pager li.previous',
		firstSelector: null,
		lastSelector: null,
		onNext: function( tab, navigation, index, newindex ) {
			var validated = $('#w3 form').valid();
			if( !validated ) {
				$w3validator.focusInvalid();
				return false;
			}
		},
		onTabClick: function( tab, navigation, index, newindex ) {
			if ( newindex == index + 1 ) {
				return this.onNext( tab, navigation, index, newindex);
			} else if ( newindex > index + 1 ) {
				return false;
			} else {
				return true;
			}
		},
		onTabChange: function( tab, navigation, index, newindex ) {
			var $total = navigation.find('li').size() - 1;
			$w3finish[ newindex != $total ? 'addClass' : 'removeClass' ]( 'hidden' );
			$('#w3').find(this.nextSelector)[ newindex == $total ? 'addClass' : 'removeClass' ]( 'hidden' );
		},
		onTabShow: function( tab, navigation, index ) {
			var $total = navigation.find('li').length - 1;
			var $current = index;
			var $percent = Math.floor(( $current / $total ) * 100);
			$('#w3').find('.progress-indicator').css({ 'width': $percent + '%' });
			tab.prevAll().addClass('completed');
			tab.nextAll().removeClass('completed');
		}
	});

	/*
	Wizard #4
	*/
	var $w4finish = $('#w4').find('ul.pager li.finish'),
		$w4validator = $("#w4 form").validate({
		highlight: function(element) {
			$(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		},
		success: function(element) {
			$(element).closest('.form-group').removeClass('has-error');
			$(element).remove();
		},
		errorPlacement: function( error, element ) {
			element.parent().append( error );
		}
	});

	$w4finish.on('click', function( ev ) {
		ev.preventDefault();
		var validated = $('#w4 form').valid();
		if ( validated ) {
			new PNotify({
				title: 'Congratulations',
				text: 'You completed the wizard form.',
				type: 'custom',
				addclass: 'notification-success',
				icon: 'fa fa-check'
			});
		}
	});

	$('#w4').bootstrapWizard({
		tabClass: 'wizard-steps',
		nextSelector: 'ul.pager li.next',
		previousSelector: 'ul.pager li.previous',
		firstSelector: null,
		lastSelector: null,
		onNext: function( tab, navigation, index, newindex ) {
			var validated = $('#w4 form').valid();
			if( !validated ) {
				$w4validator.focusInvalid();
				return false;
			}
		},
		onTabClick: function( tab, navigation, index, newindex ) {
			if ( newindex == index + 1 ) {
				return this.onNext( tab, navigation, index, newindex);
			} else if ( newindex > index + 1 ) {
				return false;
			} else {
				return true;
			}
		},
		onTabChange: function( tab, navigation, index, newindex ) {
			var $total = navigation.find('li').size() - 1;
			$w4finish[ newindex != $total ? 'addClass' : 'removeClass' ]( 'hidden' );
			$('#w4').find(this.nextSelector)[ newindex == $total ? 'addClass' : 'removeClass' ]( 'hidden' );
		},
		onTabShow: function( tab, navigation, index ) {
			var $total = navigation.find('li').length - 1;
			var $current = index;
			var $percent = Math.floor(( $current / $total ) * 100);
			$('#w4').find('.progress-indicator').css({ 'width': $percent + '%' });
			tab.prevAll().addClass('completed');
			tab.nextAll().removeClass('completed');
		}
	});


	/*
	Wizard #5
	*/
	var $w5finish = $('#w5').find('ul.pager li.finish'),
		$w5validator = $("#w5 form").validate({
		highlight: function(element) {
			$(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		},
		success: function(element) {
			$(element).closest('.form-group').removeClass('has-error');
			$(element).remove();
		},
		errorPlacement: function( error, element ) {
			element.parent().append( error );
		}
	});

	$w5finish.on('click', function( ev ) {
		ev.preventDefault();
		var validated = $('#w5 form').valid();
		if ( validated ) {
			new PNotify({
				title: 'Congratulations',
				text: 'You completed the wizard form.',
				type: 'custom',
				addclass: 'notification-success',
				icon: 'fa fa-check'
			});
		}
	});

	$('#w5').bootstrapWizard({
		tabClass: 'wizard-steps',
		nextSelector: 'ul.pager li.next',
		previousSelector: 'ul.pager li.previous',
		firstSelector: null,
		lastSelector: null,
		onNext: function( tab, navigation, index, newindex ) {
			var validated = $('#w5 form').valid();
			if( !validated ) {
				$w5validator.focusInvalid();
				return false;
			}
		},
		onTabChange: function( tab, navigation, index, newindex ) {
			var $total = navigation.find('li').size() - 1;
			$w5finish[ newindex != $total ? 'addClass' : 'removeClass' ]( 'hidden' );
			$('#w5').find(this.nextSelector)[ newindex == $total ? 'addClass' : 'removeClass' ]( 'hidden' );
		},

		onTabShow: function( tab, navigation, index ) {
			var $total = navigation.find('li').length;
			var $current = index + 1;
			var $percent = ( $current / $total ) * 100;
			$('#w5').find('.progress-bar').css({ 'width': $percent + '%' });
		}
	});

	/*
        Wizard #6
        */
	var $w6finish = $('#w6').find('ul.pager li.finish'),
		$w6validator = $("#w6 form").validate({
		    highlight: function(element) {
		        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		    },
		    success: function(element) {
		        $(element).closest('.form-group').removeClass('has-error');
		        $(element).remove();
		    },
		    errorPlacement: function( error, element ) {
		        element.parent().append( error );
		    }
		});

	$w6finish.on('click', function( ev ) {
	    ev.preventDefault();
	    var validated = $('#w6 form').valid();
	    if ( validated ) {
	        new PNotify({
	            title: 'Congratulations',
	            text: 'You completed the wizard form.',
	            type: 'custom',
	            addclass: 'notification-success',
	            icon: 'fa fa-check'
	        });
	    }
	});

	$('#w6').bootstrapWizard({
	    tabClass: 'wizard-steps',
	    nextSelector: 'ul.pager li.next',
	    previousSelector: 'ul.pager li.previous',
	    firstSelector: null,
	    lastSelector: null,
	    onNext: function( tab, navigation, index, newindex ) {
	        var validated = $('#w6 form').valid();
	        if( !validated ) {
	            $w6validator.focusInvalid();
	            return false;
	        }
	    },
	    onTabChange: function( tab, navigation, index, newindex ) {
	        var $total = navigation.find('li').size() - 1;
	        $w6finish[ newindex != $total ? 'addClass' : 'removeClass' ]( 'hidden' );
	        $('#w6').find(this.nextSelector)[ newindex == $total ? 'addClass' : 'removeClass' ]( 'hidden' );
	    },

	    onTabShow: function( tab, navigation, index ) {
	        var $total = navigation.find('li').length;
	        var $current = index + 1;
	        var $percent = ( $current / $total ) * 100;
	        $('#w6').find('.progress-bar').css({ 'width': $percent + '%' });
	    }
	});

    /*
        Wizard #7
        */
	var $w7finish = $('#w7').find('ul.pager li.finish'),
		$w7validator = $("#w7 form").validate({
		    highlight: function (element) {
		        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		    },
		    success: function (element) {
		        $(element).closest('.form-group').removeClass('has-error');
		        $(element).remove();
		    },
		    errorPlacement: function (error, element) {
		        element.parent().append(error);
		    }
		});

	$w7finish.on('click', function (ev) {
	    ev.preventDefault();
	    var validated = $('#w7 form').valid();
	    if (validated) {
	        new PNotify({
	            title: 'Congratulations',
	            text: 'You completed the wizard form.',
	            type: 'custom',
	            addclass: 'notification-success',
	            icon: 'fa fa-check'
	        });
	    }
	});

	$('#w7').bootstrapWizard({
	    tabClass: 'wizard-steps',
	    nextSelector: 'ul.pager li.next',
	    previousSelector: 'ul.pager li.previous',
	    firstSelector: null,
	    lastSelector: null,
	    onNext: function (tab, navigation, index, newindex) {
	        var validated = $('#w7 form').valid();
	        if (!validated) {
	            $w7validator.focusInvalid();
	            return false;
	        }
	    },
	    onTabChange: function (tab, navigation, index, newindex) {
	        var $total = navigation.find('li').size() - 1;
	        $w7finish[newindex != $total ? 'addClass' : 'removeClass']('hidden');
	        $('#w7').find(this.nextSelector)[newindex == $total ? 'addClass' : 'removeClass']('hidden');
	    },

	    onTabShow: function (tab, navigation, index) {
	        var $total = navigation.find('li').length;
	        var $current = index + 1;
	        var $percent = ($current / $total) * 100;
	        $('#w7').find('.progress-bar').css({ 'width': $percent + '%' });
	    }
	});

    /*  Wizard #8  */
	var $w8finish = $('#w8').find('ul.pager li.finish'),
		$w8validator = $("#w8 form").validate({
		    highlight: function (element) {
		        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		    },
		    success: function (element) {
		        $(element).closest('.form-group').removeClass('has-error');
		        $(element).remove();
		    },
		    errorPlacement: function (error, element) {
		        element.parent().append(error);
		    }
		});

	$w8finish.on('click', function (ev) {
	    ev.preventDefault();
	    var validated = $('#w8 form').valid();
	    if (validated) {
	        new PNotify({
	            title: 'Congratulations',
	            text: 'You completed the wizard form.',
	            type: 'custom',
	            addclass: 'notification-success',
	            icon: 'fa fa-check'
	        });
	    }
	});

	$('#w8').bootstrapWizard({
	    tabClass: 'wizard-steps',
	    nextSelector: 'ul.pager li.next',
	    previousSelector: 'ul.pager li.previous',
	    firstSelector: null,
	    lastSelector: null,
	    onNext: function (tab, navigation, index, newindex) {
	        var validated = $('#w8 form').valid();
	        if (!validated) {
	            $w8validator.focusInvalid();
	            return false;
	        }
	    },
	    onTabChange: function (tab, navigation, index, newindex) {
	        var $total = navigation.find('li').size() - 1;
	        $w8finish[newindex != $total ? 'addClass' : 'removeClass']('hidden');
	        $('#w8').find(this.nextSelector)[newindex == $total ? 'addClass' : 'removeClass']('hidden');
	    },

	    onTabShow: function (tab, navigation, index) {
	        var $total = navigation.find('li').length;
	        var $current = index + 1;
	        var $percent = ($current / $total) * 100;
	        $('#w8').find('.progress-bar').css({ 'width': $percent + '%' });
	    }
	});

    /*  Wizard #9  */
	var $w9finish = $('#w9').find('ul.pager li.finish'),
		$w9validator = $("#w9 form").validate({
		    highlight: function (element) {
		        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		    },
		    success: function (element) {
		        $(element).closest('.form-group').removeClass('has-error');
		        $(element).remove();
		    },
		    errorPlacement: function (error, element) {
		        element.parent().append(error);
		    }
		});

	$w9finish.on('click', function (ev) {
	    ev.preventDefault();
	    var validated = $('#w9 form').valid();
	    if (validated) {
	        new PNotify({
	            title: 'Congratulations',
	            text: 'You completed the wizard form.',
	            type: 'custom',
	            addclass: 'notification-success',
	            icon: 'fa fa-check'
	        });
	    }
	});

	$('#w9').bootstrapWizard({
	    tabClass: 'wizard-steps',
	    nextSelector: 'ul.pager li.next',
	    previousSelector: 'ul.pager li.previous',
	    firstSelector: null,
	    lastSelector: null,
	    onNext: function (tab, navigation, index, newindex) {
	        var validated = $('#w9 form').valid();
	        if (!validated) {
	            $w9validator.focusInvalid();
	            return false;
	        }
	    },
	    onTabChange: function (tab, navigation, index, newindex) {
	        var $total = navigation.find('li').size() - 1;
	        $w9finish[newindex != $total ? 'addClass' : 'removeClass']('hidden');
	        $('#w9').find(this.nextSelector)[newindex == $total ? 'addClass' : 'removeClass']('hidden');
	    },

	    onTabShow: function (tab, navigation, index) {
	        var $total = navigation.find('li').length;
	        var $current = index + 1;
	        var $percent = ($current / $total) * 100;
	        $('#w9').find('.progress-bar').css({ 'width': $percent + '%' });
	    }
	});

    /*  Wizard #10  */
	var $w10finish = $('#w10').find('ul.pager li.finish'),
		$w10validator = $("#w10 form").validate({
		    highlight: function (element) {
		        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		    },
		    success: function (element) {
		        $(element).closest('.form-group').removeClass('has-error');
		        $(element).remove();
		    },
		    errorPlacement: function (error, element) {
		        element.parent().append(error);
		    }
		});

	$w10finish.on('click', function (ev) {
	    ev.preventDefault();
	    var validated = $('#w10 form').valid();
	    if (validated) {
	        new PNotify({
	            title: 'Congratulations',
	            text: 'You completed the wizard form.',
	            type: 'custom',
	            addclass: 'notification-success',
	            icon: 'fa fa-check'
	        });
	    }
	});

	$('#w10').bootstrapWizard({
	    tabClass: 'wizard-steps',
	    nextSelector: 'ul.pager li.next',
	    previousSelector: 'ul.pager li.previous',
	    firstSelector: null,
	    lastSelector: null,
	    onNext: function (tab, navigation, index, newindex) {
	        var validated = $('#w10 form').valid();
	        if (!validated) {
	            $w10validator.focusInvalid();
	            return false;
	        }
	    },
	    onTabChange: function (tab, navigation, index, newindex) {
	        var $total = navigation.find('li').size() - 1;
	        $w10finish[newindex != $total ? 'addClass' : 'removeClass']('hidden');
	        $('#w10').find(this.nextSelector)[newindex == $total ? 'addClass' : 'removeClass']('hidden');
	    },

	    onTabShow: function (tab, navigation, index) {
	        var $total = navigation.find('li').length;
	        var $current = index + 1;
	        var $percent = ($current / $total) * 100;
	        $('#w10').find('.progress-bar').css({ 'width': $percent + '%' });
	    }
	});

    /*  Wizard #11  */
	var $w11finish = $('#w11').find('ul.pager li.finish'),
		$w11validator = $("#w11 form").validate({
		    highlight: function (element) {
		        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		    },
		    success: function (element) {
		        $(element).closest('.form-group').removeClass('has-error');
		        $(element).remove();
		    },
		    errorPlacement: function (error, element) {
		        element.parent().append(error);
		    }
		});

	$w11finish.on('click', function (ev) {
	    ev.preventDefault();
	    var validated = $('#w11 form').valid();
	    if (validated) {
	        new PNotify({
	            title: 'Congratulations',
	            text: 'You completed the wizard form.',
	            type: 'custom',
	            addclass: 'notification-success',
	            icon: 'fa fa-check'
	        });
	    }
	});

	$('#w11').bootstrapWizard({
	    tabClass: 'wizard-steps',
	    nextSelector: 'ul.pager li.next',
	    previousSelector: 'ul.pager li.previous',
	    firstSelector: null,
	    lastSelector: null,
	    onNext: function (tab, navigation, index, newindex) {
	        var validated = $('#w11 form').valid();
	        if (!validated) {
	            $w11validator.focusInvalid();
	            return false;
	        }
	    },
	    onTabChange: function (tab, navigation, index, newindex) {
	        var $total = navigation.find('li').size() - 1;
	        $w11finish[newindex != $total ? 'addClass' : 'removeClass']('hidden');
	        $('#w11').find(this.nextSelector)[newindex == $total ? 'addClass' : 'removeClass']('hidden');
	    },

	    onTabShow: function (tab, navigation, index) {
	        var $total = navigation.find('li').length;
	        var $current = index + 1;
	        var $percent = ($current / $total) * 100;
	        $('#w11').find('.progress-bar').css({ 'width': $percent + '%' });
	    }
	});

    /*  Wizard #12  */
	var $w12finish = $('#w12').find('ul.pager li.finish'),
		$w12validator = $("#w12 form").validate({
		    highlight: function (element) {
		        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		    },
		    success: function (element) {
		        $(element).closest('.form-group').removeClass('has-error');
		        $(element).remove();
		    },
		    errorPlacement: function (error, element) {
		        element.parent().append(error);
		    }
		});

	$w12finish.on('click', function (ev) {
	    ev.preventDefault();
	    var validated = $('#w12 form').valid();
	    if (validated) {
	        new PNotify({
	            title: 'Congratulations',
	            text: 'You completed the wizard form.',
	            type: 'custom',
	            addclass: 'notification-success',
	            icon: 'fa fa-check'
	        });
	    }
	});

	$('#w12').bootstrapWizard({
	    tabClass: 'wizard-steps',
	    nextSelector: 'ul.pager li.next',
	    previousSelector: 'ul.pager li.previous',
	    firstSelector: null,
	    lastSelector: null,
	    onNext: function (tab, navigation, index, newindex) {
	        var validated = $('#w11 form').valid();
	        if (!validated) {
	            $w12validator.focusInvalid();
	            return false;
	        }
	    },
	    onTabChange: function (tab, navigation, index, newindex) {
	        var $total = navigation.find('li').size() - 1;
	        $w12finish[newindex != $total ? 'addClass' : 'removeClass']('hidden');
	        $('#w12').find(this.nextSelector)[newindex == $total ? 'addClass' : 'removeClass']('hidden');
	    },

	    onTabShow: function (tab, navigation, index) {
	        var $total = navigation.find('li').length;
	        var $current = index + 1;
	        var $percent = ($current / $total) * 100;
	        $('#w12').find('.progress-bar').css({ 'width': $percent + '%' });
	    }
	});

    /*  Wizard #13  */
	var $w13finish = $('#w13').find('ul.pager li.finish'),
		$w13validator = $("#w13 form").validate({
		    highlight: function (element) {
		        $(element).closest('.form-group').removeClass('has-success').addClass('has-error');
		    },
		    success: function (element) {
		        $(element).closest('.form-group').removeClass('has-error');
		        $(element).remove();
		    },
		    errorPlacement: function (error, element) {
		        element.parent().append(error);
		    }
		});

	$w13finish.on('click', function (ev) {
	    ev.preventDefault();
	    var validated = $('#w13 form').valid();
	    if (validated) {
	        new PNotify({
	            title: 'Congratulations',
	            text: 'You completed the wizard form.',
	            type: 'custom',
	            addclass: 'notification-success',
	            icon: 'fa fa-check'
	        });
	    }
	});

	$('#w13').bootstrapWizard({
	    tabClass: 'wizard-steps',
	    nextSelector: 'ul.pager li.next',
	    previousSelector: 'ul.pager li.previous',
	    firstSelector: null,
	    lastSelector: null,
	    onNext: function (tab, navigation, index, newindex) {
	        var validated = $('#w11 form').valid();
	        if (!validated) {
	            $w13validator.focusInvalid();
	            return false;
	        }
	    },
	    onTabChange: function (tab, navigation, index, newindex) {
	        var $total = navigation.find('li').size() - 1;
	        $w13finish[newindex != $total ? 'addClass' : 'removeClass']('hidden');
	        $('#w13').find(this.nextSelector)[newindex == $total ? 'addClass' : 'removeClass']('hidden');
	    },

	    onTabShow: function (tab, navigation, index) {
	        var $total = navigation.find('li').length;
	        var $current = index + 1;
	        var $percent = ($current / $total) * 100;
	        $('#w13').find('.progress-bar').css({ 'width': $percent + '%' });
	    }
	});

	}).apply(this, [jQuery]);

