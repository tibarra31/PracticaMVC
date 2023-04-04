var app = angular.module('app', ['ngRoute', 'ui.bootstrap', 'ngGrid', 'ngLocale']);
//Directiva modificada con comentario
//para el proyecto principal de Github
//12
app.directive('smModal', function () {
	return {
		restrict: 'E',
		transclude: true,
		required: 'name',
		scope: {
			titulo: '@',
			name: '@',
			width: '@'
		},
		template: '<link href="../Content/css/animate.css" rel="stylesheet" type="text/css">'
			+ '<div id="divBloqueo_{{name}}" ng-show="modalShow.{{name}}" class="smModal animated modal fade">'
			+ '<div id="content_{{name}}" class="modal-content" style="margin-left:auto;margin-right: auto;margin-top:6%;width:{{width}}">'
			+ '<div class="modal-header" id="modal-header{{name}}"><div id="h4{{name}}" class="h4" style="margin: auto;float:left;width:90%">{{titulo}}</div><div style="float:right;width:10%;text-align:center;">'
			+ '<button id="btn_close_{{name}}" class="btn btn-danger btn-xs" ng-click="modalClose()"><span class="glyphicon glyphicon-remove"></span></button>'
			+ '</div></div>'
			+ '<div class="modal-body" id="modal-body{{name}}"><div ng-transclude></div></div>'
			+ '</div>'
			+ '</div>',
		controller: function ($scope) {
			$scope.modalShow = {};
			$scope.width = $scope.width ? $scope.width : '500px';
			$scope.modalClose = function () {
				window.cerrarModal($scope.name);
			}
		},
		controllerAs: 'modal',
		link: function (scope, element, attrs, ctrl, transclude) {
			window.abrirModal = function (modal, title, sn_btncerrar, fnc) {
				var sn_btncerrar = (!sn_btncerrar && sn_btncerrar != undefined) ? false : true;

				if (sn_btncerrar)
					$("#btn_close_" + modal).css('display', 'none');
				else
					$("#btn_close_" + modal).css('display', '');

				scopeModal = angular.element($('#modal-body' + modal).children()[0]).scope();
				scopeModal.content = angular.element($($('#modal-body' + modal).children()[0]).children()[0]).scope();
				var div = $($('#modal-body' + modal).children()[0]).children(".modal-footer");
				scopeModal.width = scopeModal.width ? scopeModal.width : '500px';
				$('#h4' + modal).html(title ? title : attrs.titulo);
				$('#content_' + modal).css("width", scopeModal.width);

				$('#divBloqueo_' + modal).removeClass().addClass('smModal show animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () { });
				$('#content_' + modal).removeClass().addClass('modal-content fadeIn animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () { });

				var btn = $(div[0]).children("#" + modal + "btnClose_");


				setTimeout(function () {
					$(document).bind('keyup', function (e) {
						if ($('#btn_close_' + modal).is(':visible') && e.keyCode == 27) {
							window.cerrarModal(modal);
							if (fnc)
								fnc();
						}
					});
				}, 400);


			};

			//cierra el modal
			window.cerrarModal = function (modal) {

				$('#content_' + modal).removeClass().addClass('modal-content fadeOut animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () { });

				setTimeout(function () {
					$('#divBloqueo_' + modal).removeClass().addClass('smModal hide animated').one('webkitAnimationEnd mozAnimationEnd MSAnimationEnd oanimationend animationend', function () { });
				}, 500);

				if (scopeModal.funcionCerrar) {
					scopeModal.funcionCerrar();
				}
			};

			window.fnc_modalIsOpen = function (modal) {
				if (!modal) return false;
				return $('#divBloqueo_' + modal).attr('class').match(/show/);
			}
		}
	};
});

