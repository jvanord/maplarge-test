
/*
	Core App Features

	Note: This prototype is using HTML5 and ECMA 6 and for clarity no shims or 
	feature detection is being used for backward compatability.
	
	Normally this would be organized in more than one file, but this is a prototype.
*/

// Page View Model
function pageViewModel(model) {

	// Init
	var self = this;
	model = $.extend({ loading: true }, model); // defaults

	// Properties
	self.loading = ko.observable(!!model.loading);
	self.path = ko.observable();
	self.children = ko.observableArray();
	self.files = ko.observableArray();
	self.error = ko.observable();

	// Methods
	self.loadCurrentPath = function () {
		self.loading(true);
		var path = decodeURIComponent(location.hash.substr(1));
		var preloaded = Preloader.get(path);
		if (preloaded)
			setPathAndChildren(preloaded);
		else
			Api.getPath(path, getPathCallback);
	};
	self.onPathClick = function (path) {
		self.loading(true);
		self.error(null);
		history.pushState({ path }, '', '#' + path)
		var preloaded = Preloader.get(path);
		if (preloaded)
			setPathAndChildren(preloaded);
		else
			Api.getPath(path, getPathCallback);
	};
	self.onDownloadClick = function (path, file) {
		if (!path || !file) return console.error('Can\'t resolve file path.', {path,file});
		var fullPath = [path, file].join('\\').replace('\\\\', '\\');
		window.open('/default/download?path=' + encodeURIComponent(fullPath));
	};
	self.onNewDirClick = function () {
		var name = prompt('What would you like to call your new directory?');
		if (!name) return;
		self.loading(true);
		self.error(null);
		Api.newPath(self.path(), name, function (data, error) {
			if (error) setError(error);
			Preloader.remove(self.path()); // clear cache
			self.loadCurrentPath(); // refresh
		});
	};
	self.onDeleteDirClick = function (path) {
		if (!path) return;
		self.loading(true);
		if (!confirm('This action will delete this folder AND ALL OF ITS CONTENTS. Are you absolutely sure?'))
			return self.loading(false);
		self.error(null);
		Api.deletePath(path, function (data, error) {
			if (error)
				setError(error);
			else {
				Preloader.remove(self.path()); // clear cache
				self.loadCurrentPath(); // refresh
			}
		});
	};

	// "Private" Functions
	function getPathCallback(pathInfo, error) {
		if (error)
			setError(error);
		else
			setPathAndChildren(pathInfo);
	}
	function setPathAndChildren(pathInfo) {
		if (!!pathInfo) {
			self.path(pathInfo.path);
			self.children(pathInfo.children || []);
			self.files(pathInfo.files || []);
			for (var i = 0; i < pathInfo.children.length; i++) {
				Preloader.loadAsync(pathInfo.children[i]);
			}
		}
		else {
			self.path(null);
			self.children([]);
		}
		self.loading(false);
	}
	function setError(err) {
		if (!err) return self.error(null);
		var message =
			typeof err === 'string'
				? err
				: !!err.responseJSON
					? err.responseJSON.message || err.responseJSON.Message
					: null;
		self.error(message || err.statusText || 'Unidentified Error');
	}
};

// API Controller
var Api = (function ($) {
	var settings = {
		baseUri: null,
		directoryUri: '/dir'
	};
	var me = {};

	// Initialize
	me.init = function (options) {
		if (!$) throw new Error('jQuery failed to load!');
		if (!options || !options.baseUri) throw new Error('Invalid API base URI!')
		settings = $.extend(settings, options);
	};

	// Get Directory
	me.getPath = function (path, callback) {
		var preloaded = Preloader.get(path);
		if (!!preloaded) {
			if (typeof callback === 'function') callback.call(preloaded);
			return;
		}
		$.get(settings.baseUri + settings.directoryUri, { path: path || null })
			.done(function (data) {
				if (typeof callback === 'function') callback.call(null, data);
				Preloader.add(data);
			})
			.fail(function (error) { if (typeof callback === 'function') callback.call(null, null, error); });
	};

	// Create Directory
	me.newPath = function (path, name, callback) {
		$.post(settings.baseUri + settings.directoryUri, { path: path, name: name })
			.done(function (data) { if (typeof callback === 'function') callback.call(null, data); })
			.fail(function (error) { if (typeof callback === 'function') callback.call(null, null, error); });
	};

	// Delete Directory
	me.deletePath = function (path, callback) {
		$.ajax({
			url: settings.baseUri + settings.directoryUri + '?' + $.param({ path: path }),
			type: 'DELETE'
		})
			.done(function (data) { if (typeof callback === 'function') callback.call(null, data); })
			.fail(function (error) { if (typeof callback === 'function') callback.call(null, null, error); });
	};

	return me;
})(window.jQuery);

// Preloading for Performance
var Preloader = (function () {
	var items = [];
	function get(path) { return items.find(p => p.path === (path || '\\')); }
	function add(item) { 
		if (!item || !item.path) return;
		var match = items.find(p => p.path === item.path);
		if (match) match = item;
		else items.push(item);
	}
	function remove(path) { 
		if (!path) return;
		items = items.filter(p => p.path !== path);
	}
	function loadAsync(path) { Api.getPath(path, loadAsyncCallback); }
	function loadAsyncCallback(data, error) { if (data) Preloader.add(data); }
	return { get: get, add: add, remove: remove, loadAsync: loadAsync };
})();
