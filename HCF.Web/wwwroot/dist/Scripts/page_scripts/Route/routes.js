var openNewAsset = function (locationId, assetId, serialno) {
    debugger;
    var url = CRxUrls.fireExtinguisher_AddNewAssets;
    $.get(url + "?floorAssetsId=0&locationId=" + locationId + "&AssetId=" + assetId + "&SerialNo=" + serialno, function (data) {
        $('#addNewAssets').modal('show');
        $('#loadpartialAddNew').html(data);
        $('#loadpartialAddNew').fadeIn('fast');
    });
};

var AddAssetTlocation = function (floorAssetId, locationId, orgFloorAssetsId, assetId) {
    $.ajax({
        type: "GET",
        url: CRxUrls.fireExtinguisher_AddAssetTlocation + "?FloorAssetId=" + floorAssetId + "&locationId=" + locationId + "&orgFloorAssetsId=" + orgFloorAssetsId + "&assetId=" + assetId,
        success: function (data) {
            if (data.isClosedPopUp == 1) {
                swal(data.msg);
                $('#extinguisherInsPopUp').modal('hide');
                if ($("#isloaddata").val() == 1) {
                    loadData();
                }
            } else {
                swal(data.msg);
            }
        },
        dataType: "json",
        traditional: true
    });
}

var GoToInspectionPage = function (floorId, routeId, assetId, floorAssetId, locationId, inspType) {
    debugger;
    var drpInspTypes = inspType; //7;
    var url = CRxUrls.fireExtinguisher_SerachIndex;
    url = url + '?floorId=' + floorId + "&inspType=" + drpInspTypes + "&assetId=" + assetId + "&routeId=" + routeId + "&floorAssetId=" + floorAssetId + "&locationId=" + locationId;
    window.location.href = url;
}

//Remove FloorAssets From CurrentLocation
$(document).on('click', 'a.delete', function (e) {
    e.preventDefault();
    var orgFloorAssetsId = $(this).data("id");
    swal({
        html: true,
        title: "Are you sure?",
        text: "You are removing an asset from this stop. <br /><br /> This will not erase the stop.",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: true
    },
        function () {
            $.ajax({
                type: "GET",
                url: CRxUrls.fireExtinguisher_RemoveFloorAssetsFromCurrentLocation + "?orgFloorAssetsId=" + orgFloorAssetsId,
                success: function (data) {
                    debugger;
                    swal(data.msg);                    
                },
                dataType: "json",
                traditional: true
            });
        });
});
     //Remove FloorAssets From CurrentLocation