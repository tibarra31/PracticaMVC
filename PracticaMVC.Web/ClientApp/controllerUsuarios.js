	var scopeModal;
	var app = angular.module('app'); 

	app.controller("ctrlUsuarios", function ($scope, $http) {
		$scope.titulo = "Administrador de Usuarios (AngularJS)";
		$scope.tituloRegistro = "Registro de Usuarios (AngularJS)";
		$scope.filtroUsuarios = "";
		$scope.filtroNombreUsuario = "";
		$scope.ListUsuarios = [];
		$scope.ListPerfiles = [];
		$scope.ListPerfilesRoles = [];
		$scope.ListPermisos = [];
		$scope.ListPermisosUsuario = [];
		$scope.ListPermisos = [];
		$scope.ObjUsuarios = {
			IdUsuario: 0,
			Usuario: "",
			Password: "",
			IdPerfil: 0,
			IdPerfilRol: 0,
			Listado_Permisos: []
		};
		$scope.Listado_Permisos = {
			IdPermiso: 0,
			Permiso: ""
		}
		$scope.ObjMensajes = {
			tipo: "",
			titulo: "",
			mensaje: ""
		};

		$scope.IdPermiso = 0;
		$scope.params = new URLSearchParams(document.location.search);

		$scope.getDataInfo = function () {
			var objFiltros = {
				filtroUsuarios: $scope.filtroUsuarios ? $scope.filtroUsuarios : null,
				filtroNombreUsuario: $scope.filtroNombreUsuario ? $scope.filtroNombreUsuario : null,
			}
			$http.post("/UsuariosAngular/GetUsuariosAngular", objFiltros).success(function (respuesta) {
				if (respuesta.ExecutionOK) {
					$scope.ListUsuarios = respuesta.Data

					if ($scope.params.get('IdUsuario') != null) {
						$scope.VerDetalleUsuario(parseInt($scope.params.get('IdUsuario')))
					}

					if ($scope.ObjMensajes.mensaje != "") {
						ShowNotificacion($scope.ObjMensajes.tipo, $scope.ObjMensajes.titulo, $scope.ObjMensajes.mensaje, "4", "0");
						$scope.ObjMensajes.tipo = "";
						$scope.ObjMensajes.titulo = "";
						$scope.ObjMensajes.mensaje = "";
                    }
				}
			});
		};
		$scope.getDataInfo();

		$scope.VerDetalleUsuario = function (IdUsuarioSeleccionado) {
			var IdUsuario_ = IdUsuarioSeleccionado;
			$http.post("/UsuariosAngular/GetUsuariosAngularByIdUsuario", { IdUsuario: IdUsuario_ }).success(function (respuesta) {
				if (respuesta.ExecutionOK) {
					scopeModal = angular.element($('#modal-bodymodalUsuariosPermisos').children()[0]).scope();
					scopeModal.content = angular.element($($('#modal-bodymodalUsuariosPermisos').children()[0]).children()[0]).scope();
					$scope.ObjUsuarios.IdUsuario = respuesta.Data.IdUsuario;
					$scope.ObjUsuarios.Usuario = respuesta.Data.Usuario;
					$scope.ObjUsuarios.Password = respuesta.Data.Password;
					$scope.ObjUsuarios.IdPerfil = respuesta.Data.IdPerfil;
					$scope.ObjUsuarios.IdPerfilRol = respuesta.Data.IdPerfilRol;
					$scope.ListPerfiles = respuesta.Data.Perfiles;
					$scope.ListPerfilesRoles = respuesta.Data.PerfilesRoles;
					scopeModal.content.ListPermisos = respuesta.Data.permisosUsuario.PermisosUsuarios;
					$scope.ObjUsuarios.Listado_Permisos = respuesta.Data.Listado_Permisos;

					console.log($scope.ListPermisos)
				}
			});
		}

		$scope.GuardarUsuario = function(){
			console.log($scope.ObjUsuarios)
			if ($scope.params.get('IdUsuario') == null) {
				$scope.ObjUsuarios.IdUsuario = 0;
			}
			$http.post("/UsuariosAngular/GuardarUsuario", { ObjUsuarios: $scope.ObjUsuarios }).success(function (respuesta) {
				if (respuesta.ExecutionOK) {
					$scope.ObjMensajes.tipo = "success"
					$scope.ObjMensajes.titulo = "Info"
					$scope.ObjMensajes.mensaje = "Usuario " + ($scope.ObjUsuarios.IdUsuario == 0 ? " Guardado " : " Modificado ") + "con Exito" 
					window.location = '/UsuariosAngular/Index';
				}
				else {
					$scope.ObjMensajes.tipo = "error"
					$scope.ObjMensajes.titulo = "Error"
					$scope.ObjMensajes.mensaje = respuesta.Message
                }
			});
        }

		$scope.VerDetalle = function (row) {
			window.location = '/UsuariosAngular/Detalle?IdUsuario=' + row.IdUsuario;
		}

		$scope.Eliminar = function (row) {
			console.log(row);
		}

		$scope.gridUsuarios = {
			data: 'ListUsuarios',
			enableRowSelection: true,
			enableCellEditOnFocus: true,
			multiSelect: false,
			rowTemplate: '<div  ng-style="{\'cursor\': row.cursor, \'z-index\': col.zIndex() }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell {{col.cellClass}}" ng-cell></div>',
			columnDefs: [
				{ field: 'IdUsuario', displayName: 'Id Usuario', enableCellEdit: false, width: '10%' },
				{ field: 'Usuario', displayName: 'Usuario', enableCellEdit: false, width: '30%' },
				{ field: 'Perfil', displayName: 'Perfil', enableCellEdit: false, width: '20%' },
				{ field: 'RolPerfil', displayName: 'RolPerfil', enableCellEdit: false, width: '20%' },
				{
					field: 'Col',
					width: '20%',
					displayName: 'Acciones',
					cellTemplate: '<div align="center">' +
						'<button class="btn btn-xs green-jungle btn-icon-circle" type="button" ng-click="VerDetalle(row.entity)" > <i class="fa fa-search mr-0 ml-0"></i> </button>' +
						'<button class="btn btn-xs red btn-icon-circle" type="button" data-original-title="Ver Detalle" ng-click="EliminarUsuario(row.entity)"> <i class="fa fa-times mr-0 ml-0"></i> </button></div>', enableCellEdit: false
				}
			]
		};

		$scope.gridPermisos = {
			data: 'ObjUsuarios.Listado_Permisos',
			enableRowSelection: true,
			enableCellEditOnFocus: true,
			multiSelect: false,
			rowTemplate: '<div  ng-style="{\'cursor\': row.cursor, \'z-index\': col.zIndex() }" ng-repeat="col in renderedColumns" ng-class="col.colIndex()" class="ngCell {{col.cellClass}}" ng-cell></div>',
			columnDefs: [
				{ field: 'IdPermiso', displayName: 'Id Permiso', enableCellEdit: false, width: '20%' },
				{ field: 'Permiso', displayName: 'Permiso', enableCellEdit: false, width: '60%' },
				{
					field: 'Col',
					width: '20%',
					displayName: 'Acciones',
					cellTemplate: '<div align="center">' +
						'<button class="btn btn-xs red btn-icon-circle" type="button" data-original-title="Ver Detalle" ng-click="EliminarPermiso(row)"> <i class="fa fa-times mr-0 ml-0"></i> </button></div>', enableCellEdit: false
				}
			]
		};

		$scope.GuardarPermiso = function () {
			scopeModal = angular.element($('#modal-bodymodalUsuariosPermisos').children()[0]).scope();
			scopeModal.content = angular.element($($('#modal-bodymodalUsuariosPermisos').children()[0]).children()[0]).scope();
			var itemPermiso = scopeModal.content.ListPermisos.filter(function (item) {
				return item.Valor === scopeModal.content.IdPermiso;
			})[0];


			if ($scope.ObjUsuarios.Listado_Permisos.length == 0) {
				$scope.ObjUsuarios.Listado_Permisos.push({
					IdUsuarioPermiso: 0,
					IdPermiso: itemPermiso.Valor,
					Permiso: itemPermiso.Texto
				});
			}
			else {
				var itemPush = $scope.ObjUsuarios.Listado_Permisos.filter(function (item) {
					return item.IdPermiso === scopeModal.content.IdPermiso;

				})[0];

				if (!itemPush) {
					$scope.ObjUsuarios.Listado_Permisos.push({
						IdUsuarioPermiso: 0,
						IdPermiso: itemPermiso.Valor,
						Permiso: itemPermiso.Texto
					});
				}

            }
			scopeModal.content.IdPermiso = 0;
			window.cerrarModal("modalUsuariosPermisos");
			console.log(itemPermiso)
		}

		$scope.EliminarUsuario = function (row) {
			$http.post("/UsuariosAngular/DeleteUsuario", { IdUsuario: row.IdUsuario }).success(function (respuesta) {
				if (respuesta.ExecutionOK) {
					$scope.ObjMensajes.tipo = "success"
					$scope.ObjMensajes.titulo = "Info"
					$scope.ObjMensajes.mensaje = "Usuario eliminado con Exito"
					$scope.getDataInfo();
				}
				else {
					$scope.ObjMensajes.tipo = "error"
					$scope.ObjMensajes.titulo = "Error"
					$scope.ObjMensajes.mensaje = respuesta.Message
				}
			});
		};

		$scope.EliminarPermiso = function (row) { 
			var itemPermiso = $scope.ObjUsuarios.Listado_Permisos.filter(function (item) {
				return item.IdPermiso === row.entity.IdPermiso;
			})[0];

			if (itemPermiso) {
				$scope.ObjUsuarios.Listado_Permisos.splice(row.rowIndex, 1);
            }
        }

		$scope.llenaCombos = function () {
			//Llena el array del combo Perfiles
			$http.post("/UsuariosAngular/GetComboPerfiles").success(function (respuesta) {
				$scope.ListPerfiles = respuesta
			});
			//Llena el array del combo Permisos
			$http.post("/UsuariosAngular/GetComboPermisos").success(function (respuesta) {
				$scope.ListPermisos = respuesta
			});

		};

		$scope.llenaComboRolesPerfiles = function (IdPerfil_) {
			//Llena el array del combo Perfiles
			var IdPerfil = $scope.ObjUsuarios.IdPerfil;
			if (IdPerfil == undefined || IdPerfil == 0 || IdPerfil == null) {
				return;
			}
			$http.post("/UsuariosAngular/GetComboRolesPerfiles", { IdPerfil: IdPerfil }).success(function (respuesta) {
				$scope.ListPerfilesRoles = respuesta
				$scope.ObjUsuarios.IdPerfilRol = 0;
			});
		};

		$scope.llenaCombos();


		$scope.AbrirModalUsuariosPermisos = function () {
			abrirModal("modalUsuariosPermisos", "Permisos de Usuario", false);
		}
	});
 