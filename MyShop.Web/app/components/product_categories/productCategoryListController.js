(function (app) {
    app.controller('productCategoryListController', productCategoryListController);

    productCategoryListController.$inject = ['$scope', 'apiService', 'notificationService', '$ngBootbox'];

    function productCategoryListController($scope, apiService, notificationService, $ngBootbox) {

        $scope.productCategories = [];
        $scope.page = 0;
        $scope.pagesCount = 0;
        $scope.getProductCategories = getProductCategories;
        $scope.keyword = '';

        $scope.search = search;
        $scope.deleteProductCategory = deleteProductCategory;


        function search() {
            getProductCategories();
        }

        function deleteProductCategory(id) {
            $ngBootbox.confirm("Bạn có muốn xóa không ??").then(function () {
                var config = {
                    params: {
                        id: id
                    }
                }
                apiService.del('/api/productcategory/delete', config, function () {
                    notificationService.displaySuccess("Xóa thành công !!!");
                    search();
                }, function () {
                    notificationService.displayWarning("Xóa không thành công !!!");
                })
            });
        }
        function getProductCategories(page) {
            page = page || 0;
            var config = {
                params: {
                    keyword: $scope.keyword,
                    page: page,
                    pageSize: 5,

                }
            }
            apiService.get('/api/productcategory/getall', config, function (result) {
                if ($scope.keyword != '') {
                    if (result.data.TotalCount == 0) {
                        notificationService.displayWarning("Không có bản ghi nào được tìm thấy !!!");
                    }
                    else {
                        notificationService.displaySuccess("Đã tìm thấy " + result.data.TotalCount + " bản ghi !!!")
                    }
                }

                $scope.productCategories = result.data.Items;
                $scope.page = result.data.Page;
                $scope.pagesCount = result.data.TotalPages;
                $scope.totalCount = result.data.TotalCount;
            }, function () {
                console.log("Can not load data from server");
            });
        }
        $scope.getProductCategories();
    }
})(angular.module('myshop.product_categories'));