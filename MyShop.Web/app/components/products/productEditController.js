//(function (app) {
//    app.controller('productEditController', productEditController);

//    function productEditController() {

//    }

//})(angular.module('myshop.products'));

(function (app) {
    app.controller('productEditController', productEditController);

    productEditController.$inject = ['$scope', 'apiService', 'notificationService', '$state', '$stateParams', 'commonService'];

    function productEditController($scope, apiService, notificationService, $state, $stateParams, commonService) {
        $scope.product = {
            UpdateDate: new Date(),
            Status: true,
        }

        function loadProductDetail() {
            apiService.get('/api/product/getbyid/' + $stateParams.id, null, function (resutl) {
                $scope.product = resutl.data;
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        loadProductDetail();
        $scope.ckeditorOptions = {
            language: 'vi',
            height:'200px'
        }
        $scope.UpdateProduct = UpdateProduct;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        function UpdateProduct() {
            apiService.put('/api/product/update', $scope.product,
                function (result) {
                    notificationService.displaySuccess(result.data.Name + ' đã được cập nhật.');
                    $state.go('products');
                }, function (error) {
                    notificationService.displayError('Cập nhật không thành công.');
                });
        }

        function loadCategoryID() {
            apiService.get('/api/product/getallcategory', null, function (result) {
                $scope.categoryId = result.data;
            }, function () {
                console.log('Cannot get list category');
            });
        }
        loadCategoryID();
    }

})(angular.module('myshop.products'));