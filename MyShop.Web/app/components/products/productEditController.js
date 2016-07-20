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

        $scope.moreImages = [];

        function loadProductDetail() {
            apiService.get('/api/product/getbyid/' + $stateParams.id, null, function (resutl) {
                $scope.product = resutl.data;
                if ($scope.product.MoreImages != null) {
                    $scope.moreImages = JSON.parse($scope.product.MoreImages);
                }
            }, function (error) {
                notificationService.displayError(error.data);
            });
        }
        loadProductDetail();
        $scope.ckeditorOptions = {
            language: 'vi',
            height: '200px'
        }
        $scope.UpdateProduct = UpdateProduct;
        $scope.GetSeoTitle = GetSeoTitle;

        function GetSeoTitle() {
            $scope.product.Alias = commonService.getSeoTitle($scope.product.Name);
        }

        function UpdateProduct() {
            $scope.product.MoreImages = JSON.stringify($scope.moreImages);
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

        $scope.ChooseImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.product.Image = fileUrl;
                })
            }
            finder.popup();
        }
        $scope.ChooseMoreImage = function () {
            var finder = new CKFinder();
            finder.selectActionFunction = function (fileUrl) {
                $scope.$apply(function () {
                    $scope.moreImages.push(fileUrl);
                })

            }
            finder.popup();
        }
    }

})(angular.module('myshop.products'));