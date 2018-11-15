; // Core App Features

function pageViewModel(model) {
	var self = this;
	model = $.extend({ loading: true }, model); // defaults
	self.loading = ko.observable(!!model.loading);
};
