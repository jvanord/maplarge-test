; // Core App Features

// Page View Model
function pageViewModel(model) {

	// Init
	var self = this;
	model = $.extend({ loading: true }, model); // defaults

	// Properties
	self.loading = ko.observable(!!model.loading);
	self.path = ko.observable();

	// Methods
	self.loadCurrentPath = function () {
		self.path(location.hash.substr(1));
		self.loading(false);
	};
};

// API Controller
var Api = (function ($) {
	var settings = {
		baseUri: null
	};
	var me = {};

	// Initialize
	me.init = function (options) {
		if (!$) throw new Error('jQuery failed to load!');
		if (!options || !options.baseUri) throw new Error('Invalid API base URI!')
		settings = $.extend(settings, options);
	}

	return me;
})(window.jQuery);