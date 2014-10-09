(function() {
	'use strict';
	var navApp = angular.module('artNavApp', ['lodash']);

	var NavListController = function(categoryService) {
		var _this = this;
		_this.navs = [];
		_this.categoryService = categoryService;
		_this.categoryService.getCategories().then(function(categories){
			_this.navs = categories.data;
		});
		_this.timesOfDay = ['Morning', 'Afternoon', 'Evening'];
		_this.partsOfWeek = ['Weekday', 'Weekend'];
		_this.daysOfWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
		_this.origClassInfoArray = [];
		_this.classInfoArray = [];
		_this.clickedItems = {};
		/*
		//debug in view
		_this.log = function(variable) {
			console.log(variable);
		};
		*/
	};
	NavListController.prototype.updateList = function(keyword){
		var _this = this;
		var liIdKeyword = keyword.replace(/\s/g, '');
		if ($('li#'+ liIdKeyword).hasClass('li-clicked')){
			_this.clickedItems[liIdKeyword] = false;
			_this.origClassInfoArray = _.reject(_this.origClassInfoArray, function(arr) { 
				return _.contains(arr.KeyWord, keyword); 
			});
		}else{
			_this.clickedItems[liIdKeyword] = true;
			_.forEach(_this.navs, function(arr) { 
				var catprod = _.find(arr.CategoryProductionKeywords, { 'keyword': keyword });
				if (typeof catprod !== 'undefined'){
					var prodNo, prodTitle, smallImg, Url, keyWordObjects;
					if (catprod.prod_season_no === 0){
						prodNo = catprod.pkg_no;
						prodTitle = _.find(_this.navs, { 'PackageNo': prodNo }).Title;
						smallImg = _.find(_this.navs, { 'PackageNo': prodNo }).SmallImage;
						Url = _.find(_this.navs, { 'PackageNo': prodNo }).URL;
						keyWordObjects = _.find(_this.navs, { 'PackageNo': prodNo }).CategoryProductionKeywords;
					}else{
						prodNo = catprod.prod_season_no;
						prodTitle = _.find(_this.navs, { 'ProdSeasonNo': prodNo }).Title;
						smallImg = _.find(_this.navs, { 'ProdSeasonNo': prodNo }).SmallImage;
						Url = _.find(_this.navs, { 'ProdSeasonNo': prodNo }).URL;
						keyWordObjects = _.find(_this.navs, { 'ProdSeasonNo': prodNo }).CategoryProductionKeywords;
					}
					var keyWords = _.pluck(keyWordObjects, 'keyword');
					var classInfoObj = {};
					classInfoObj.Title = prodTitle;
					classInfoObj.KeyWord = keyWords;
					classInfoObj.SmallImage = smallImg;
					classInfoObj.URL = Url;
					classInfoObj.ProdNo = prodNo;
					_this.origClassInfoArray.push(classInfoObj);
				}
			});	
		}
		_this.highlightMenu($('li#'+ liIdKeyword));
		_this.classInfoArray = _this.origClassInfoArray;
		_this.reduceList();
	};
	NavListController.prototype.reduceList = function(){
		var _this = this;
		var dateparts = $('select.datepart.ng-dirty');
		var arrayToTest = _this.origClassInfoArray;
		$.each(dateparts, function(){
			var datepart = $(this).find('option:selected').text();
			if (datepart !== ''){
				arrayToTest = _.filter(arrayToTest, function(arr) { 
					return _.contains(arr.KeyWord, datepart); 
				});
			}
		});
		_this.classInfoArray = arrayToTest;
	};
	NavListController.prototype.getAllClasses = function(category){
		var _this = this;
		var liIdKeyword = category.replace(/\s/g, '');
		var categoryClasses = $('li#'+ liIdKeyword).find('li');
		var categoryClicked = !$('li#'+ liIdKeyword).hasClass('li-clicked');
		$.each(categoryClasses, function(){
			if ($(this).children().length <= 0){
				if ($(this).hasClass('li-clicked') !== categoryClicked){
					_this.updateList($(this).text());	
				}
			}
		});
	};
	NavListController.prototype.highlightMenu = function(liStart){
		var _this = this;
		var siblings = liStart.parent().children();
		var siblingClickedCount = 0;
		$.each(siblings, function(){
			var siblingId = $(this)[0].id;
			if (_this.clickedItems[siblingId]){
				siblingClickedCount++;
			}
		});
		var newLiStart = liStart.parent().parent('li');
		if (newLiStart.length !== 0){
			if (siblings.length === siblingClickedCount){
				_this.clickedItems[newLiStart[0].id] = true;
			}else{
				_this.clickedItems[newLiStart[0].id] = false;
			}
			_this.highlightMenu(newLiStart);
		}
	};

	var CategoryService = function(http) {
		var _this = this;
		_this.http = http;
	};
	CategoryService.$inject = ['$http'];
    CategoryService.prototype.getCategories = function() {
    	var _this = this;
		return _this.http.get('/Categories_json_class_art_art.json').success(function(data){
			return data;
		});
	};
    navApp.service('categoryService', CategoryService);

	NavListController.$inject = ['categoryService'];
	navApp.controller('NavListController', NavListController);
})();

(function() {
	'use strict';
	var lodash = angular.module('lodash', []);
	lodash.factory('_', function() {
		return window._; 
	});
})();