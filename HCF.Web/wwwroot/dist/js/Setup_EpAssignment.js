var selectedUserIds = [];
var uniqueselecteduserids = [];

$(document).ready(function () {

    var epListTable = $('#ep_myTable').DataTable({
        bInfo: false,
        searching: true,
        stateSave: true,
        pageLength: 20,
        lengthChange: false,
        language: {
            emptyTable: "No data available in table",
            search: "_INPUT_",
            searchPlaceholder: "Search..."
        },
        dom: "Blfrtip",
        buttons: [{
            extend: 'csv',
            title: 'EP Assignment Report',            
            exportOptions: {
                columns: [3, 4, -2, -1],
                orthogonal: 'export'
            }
        }],
        columnDefs: [{
            targets: 2,
            searchable: false,
            orderable: false,
            width: '1%',
            className: 'dt-body-center',
            render: function (data, type, full, meta) {
                return '<input type="checkbox" class="checkep" value="' + data + '"><span></span>';
            }
        }],
        rowCallback: function (row, data) {
            // Set the checked state of the checkbox in the table
            var rowId = $('input.checkep', row).attr('value');
            if ($.inArray(rowId, rows_selected) !== -1) {
                $('input.checkep', row).prop('checked', true);
                $(row).addClass('selected');
            }
            $("#selectedCount").text(rows_selected.length);
        },
        scroller: {
            loadingIndicator: true
        }
    });

    $('#ep_myTable tbody').on('click', 'input[type="checkbox"]', function (e) {
        var $row = $(this).closest('tr');
        var rowId = $row.find('input[type="checkbox"]').attr("value");
        var index = $.inArray(rowId, rows_selected);
        if (this.checked && index === -1) {
            rows_selected.push(rowId);
        } else if (!this.checked && index !== -1) {
            rows_selected.splice(index, 1);
        }
        if (this.checked) {
            $row.addClass('selected');
        } else {
            $row.removeClass('selected');
        }

        $("#selectedCount").text(rows_selected.length);
        e.stopPropagation();
    });

    $('.ddlMultiUser').selectpicker('refresh');


    $('input:radio').on('change', function () {
        rows_selected = [];
        $('#ep_myTable tbody input[type="checkbox"]:checked').prop('checked', false);
        $("tr").removeClass("selected");
        $('#assign-select-all').prop('checked', false);
        epListTable.draw();
    });

    $.fn.dataTable.ext.search.push(
        function (settings, searchData, index, rowData, counter) {
            var categoryIds = $('input:radio[name="EPCat"]:checked').map(function () {
                return this.value;
            }).get();
            if (categoryIds.length === 0 || categoryIds[0] === "0") {
                return true;
            }
            if (categoryIds.indexOf(searchData[1]) !== -1) {
                return true;
            }
            return false;
        }
    );

    $('body').on('click', '.addEpAssignee', function () {
        var modelContainer = $("#modal-container");
        modelContainer.empty();
        modelContainer.attr('data-EpDetailID', '')
        var assignedUsers = $(this).attr("data-userlist");
        var epdetId = $(this).attr('id').split('_')[1];
        $.ajax({
            url: "/Organization/GetEpAssignee?userids=" + assignedUsers + "&epId=" + epdetId,
            method: "GET",
            success: function (data) {
                debugger;
                modelContainer.attr('data-EpDetailID', epdetId);
                modelContainer.html($(data));
                modelContainer.fadeIn('fast');
                modelContainer.modal('show');


            }
        })
    });

    $('body').off('click', '#demoOne >li input[type=checkbox]').on('click', '#demoOne >li input[type=checkbox]', function () {
        if (selectedUserIds.includes($(this).val()) === false)
            selectedUserIds.push(parseInt($(this).val()));


    });

    function getUnique(array) {
        var uniqueArray = [];

        // Loop through array values
        for (i = 0; i < array.length; i++) {
            if (uniqueArray.indexOf(array[i]) === -1) {
                uniqueArray.push(array[i]);
            }
        }
        return uniqueArray;
    }

    var checkAll = () => {

        var status = $("#example-select-all").is(":checked");
        $('.checkEpDetails').each(function (index) {
            $(this).prop("checked", status);
        });
    }

    $('#ep_myTable').on('draw.dt', function () {
        checkAll();
    });

    $('.descriptions').popover({
        trigger: 'focus'
    });



    //$("#filterUnAssignedEp").click(function () {
    //    $(".trmain").removeClass("hide");
    //    if ($(this).is(':checked')) {
    //        $(".trmain").addClass("hide");
    //        $(".trmain").each(function () {
    //            if ($(this).data("usercount") == "0") {
    //                $(this).removeClass("hide");
    //            }
    //        })
    //    }
    //});


    $("#filterUnAssignedEp").click(function () {
        //$(".trmain").removeClass("hide");
        debugger;
        if ($(this).is(':checked')) {
            $.fn.dataTable.ext.search.push(
                function (settings, data, dataIndex) {
                    return $(epListTable.row(dataIndex).node()).attr('data-usercount') == 0;
                }
            );
        } else {
            $.fn.dataTable.ext.search.pop();
            epListTable.draw();
        }
        epListTable.draw();
    });


    $('#assignEpTo').hide();

    $('#chk_assign_responsibility').on('change', function () {
        if ($(this).is(':checked')) {
            $('#assignEpTo').show();
            disableDropdown();
        } else {
            $('#assignEpTo').hide();
            $(".ddlAssignToMultiUser").val('default');
            $(".ddlAssignToMultiUser").selectpicker("refresh");
        }
    });

    $("#btnAssignResponsibility").click(function () {

        if ($('#ddlMultiUser').val() == null || $('#ddlAssignToMultiUser').val() == null) {
            AlertWarningMsg('Please select user!!');
            return false;
        }

        var from_user_ids = $('#ddlMultiUser').val();//.join(',');   //Commented by avinash on date 19-11-2021.
        console.log(from_user_ids);
        var to_user_ids = $('#ddlAssignToMultiUser').val().join(',');

        var ajaxurl = assignRepoUrl + '?fromUsers=' + from_user_ids + '&toUsers=' + to_user_ids;
        var output = $.map($('.checkep:checked'), function (n, i) {
            return n.value;
        });

        if (output.length == 0) {
            AlertWarningMsg("Selected user currently don't have any assigned EPs");
            return false;
        }
        $.ajax({
            url: ajaxurl,
            method: 'GET',
            success: function (data) {
                successAlert('EP assigned successfully.');
                $('#chk_assign_responsibility').trigger("click");
                loadView();
            },
            error: function () {

            }
        })
    });

    $("#btnAssignEps").click(function () {
        var selectedUser = $('#ddlMultiUser').val();
        if (selectedUser != null && selectedUser != '') {
            if ($("#assign-select-all").is(":checked")) {
                SaveEpUserMultipleUsersIds(0, selectedUser, true, 0);
            } else if (rows_selected.length === 0) {
                SaveEpUserMultipleUsersIds(0, selectedUser, false, 0);
            } else {
                var output = rows_selected.join(',');
                SaveEpUserMultipleUsersIds(0, selectedUser, true, output);
            }

        } else {
            AlertWarningMsg("Please select User from the above select box.");
        }
    });

    $("#assign-select-all").on('change', function () {
        var totalRows = $("tr.trmain").length;
        var totalHideRoles = $("tr.hide").length;
        if (totalRows > totalHideRoles) {
            if (this.checked) {
                $('#ep_myTable tbody input[type="checkbox"]:not(:checked)').trigger('click');
            } else {
                $('#ep_myTable tbody input[type="checkbox"]:checked').trigger('click');
            }
        }
    });

    function checkEPCheckBox(data) {
        ;
        rows_selected = [];
        $("tr").removeClass("selected");
        $('.checkep').each(function (k, v) {
            var epDetId = $(v).val();
            $(v).prop('checked', false);
            var index = data.indexOf(parseInt(epDetId));
            if (index > -1) {
                $(this).trigger('click');//prop('checked', true);         
            } else {
                $(this).prop('checked', false);
            }
        });
    }

    function loadView() {
        debugger;
        $("body").addClass("loading");
        $.ajax({
            url: loadViewUrl,
            method: 'GET',
            success: function (data) {
                $("#partialView").html("");
                $("#partialView").html(data);
            },
            error: function () {
                console.log("error");
            }
        })
    }

    function SaveEpUserMultipleUsersIds(EpDetailID, UserIds, status, epDetails) {
        // var catId = $("input[name='EPCat']:checked").val();
        debugger;
        var EpDetAssignee = [];
        rows_selected.forEach(function (epId) {
            UserIds.forEach(function (userId) {
                epUser = {
                    UserId: parseInt(userId),
                    EPDetailId: parseInt(epId)
                };
                EpDetAssignee.push(epUser);
            });
        });

        if (EpDetAssignee.length == 0) {
            UserIds.forEach(function (userId) {
                epUser = {
                    UserId: parseInt(userId),
                    EPDetailId: -1
                };
                EpDetAssignee.push(epUser);
            });
        }
        if (EpDetAssignee.length == 0) {
            return;
        }
        callSaveMethod(EpDetAssignee, "user", 0);
    }

    function callSaveMethod(epUserList, mode, isRemoveAll) {



        var userIds = $("#ddlMultiUser").val();
        if (userIds == null)
            userIds = 0;
        var catId = $("input[name=EPCat]:radio:checked").val();
        $.ajax({
            url: "/Organization/AddEPAssignees?catId=" + catId + "&mode=" + mode + "&userIds=" + userIds + "&isRemoveAll=" + isRemoveAll,
            method: "POST",
            data: { epAssignee: epUserList, __RequestVerificationToken: $('input[name=' + $.Constants.RequestVerificationToken + ']').val() },
            success: function (data) {
                successAlert("User(s) EPs updated successfully.");
                $("#partialView").html("");
                $("#partialView").html(data);
            }
        });
    }

    function CheckBoxBasedOnUser() {
        //epListTable.draw();
        var selectedUser = $('#ddlMultiUser').val();
        console.log(selectedUser);
        debugger;

        if (selectedUser != null && selectedUser != '') {
            disableDropdown();
            $.get(urlAction_GetUsersEPs + '?userIds=' + selectedUser, function (data) {
                console.log(data);
                checkEPCheckBox(data);
                setEpCountText(0);
            });
        } else {
            rows_selected = [];
            $(".trmain").removeClass("selected");
            $("#assign-select-all").prop('checked', false);;
            $('.checkep').prop('checked', false);
            $("#EPCat[value='0']").prop("checked", true);
            setEpCountText(0);
        }
    }



    //$.fn.dataTable.ext.search.push(
    //    function (settings, searchData, index, rowData, counter) {
    //        var selectedUser = $('#ddlMultiUser').val();

    //        if (selectedUser.length === 0) {
    //            return true;
    //        }

    //        var seearchData = searchData[8].split(',');
    //        console.log(seearchData)

    //        return selectedUser.some(r => seearchData.indexOf(r) >= 0)
    //    }
    //);


    function disableDropdown() {
        var selectedUser = $('#ddlMultiUser').val();
        var alreadySelectedUser = $('#ddlAssignToMultiUser').val();
        if (selectedUser != null && selectedUser != '') {
            console.log(selectedUser);
            // var alreadySelectedUser = document.getElementById("ddlAssignToMultiUser");
            var selectobject = document.getElementById("ddlAssignToMultiUser").getElementsByTagName("option");
            for (z = 0; z < selectobject.length; z++) {
                //console.log(selectobject[z]);
                var toFind = selectobject[z].value;
                var a = selectedUser.indexOf(toFind);
                if (a > -1) {
                    //selectobject[z].removeClass("selected");
                    selectobject[z].disabled = true;
                } else {
                    selectobject[z].disabled = false;
                }
            }
            if (selectedUser != null && alreadySelectedUser != null) {

                var newRecords = alreadySelectedUser.filter(function (val) {
                    return selectedUser.indexOf(val) == -1;
                });
                $('#ddlAssignToMultiUser').val(newRecords);
            }
            $('#ddlAssignToMultiUser').selectpicker('render');
        }
    }

    $('body').on('change', '#ddlMultiUser', function () {
        CheckBoxBasedOnUser();
    });

    function setEpCountText(loadDll) {
        // debugger;
        var totalEPs = $('.checkep').length;
        var totalCheckedEPs = $('.checkep:checked').length;
        $("#selectedCount").text(rows_selected.length);


        filerEps = $('input.checkep:checked').map(function () {
            return $(this).val();
        });
        if (totalEPs == totalCheckedEPs) {
            $("#assign-select-all").prop("checked", true);
        } else
            $("#assign-select-all").prop("checked", false);
    }

    $('body').off('click', '#saveEpAssignee').on('click', '#saveEpAssignee', function () {
        $("#IsCompleted").val("0");
        var EpDetailID = $('#modal-container').attr('data-EpDetailID');
        var checkboxes = $('#modal-container ul>li>input:checked');
        uniqueselecteduserids = getUnique(selectedUserIds);
        var epUsers = [];
        for (var i = 0; i < checkboxes.length; i++) {
            let userId = $(checkboxes).eq(i).val();
            var EpDetAssignee = {
                EPDetailId: parseInt(EpDetailID),
                UserId: parseInt(userId)
            }
            epUsers.push(EpDetAssignee);
        }
        var isRemoveAll = 0;
        if (epUsers.length == 0) {
            var EpDetAssignee = {
                EPDetailId: parseInt(EpDetailID),
                UserId: -1
            }
            epUsers.push(EpDetAssignee);
            isRemoveAll = 1;
        }
        callSaveMethod(epUsers, "ep", isRemoveAll);
        //$.ajax({
        //    url: "/Organization/AddEPAssignees?catId=-2&mode=ep",
        //    method: "POST",
        //    data: JSON.stringify({ EpAssignee: epUsers }),
        //    contentType: 'application/json',
        //    success: function (data) {
        //        //debugger;
        //        $('#loadepuser_' + EpDetailID).html(data);
        //        $('#loadepuser_' + EpDetailID).fadeIn('fast');
        //        $('#addEpAssigneeModel').modal('hide');
        //        successAlert('Saved Successfully');
        //        var modelContainer = $("#modal-container");
        //        modelContainer.empty();
        //        if ($('.modal-backdrop').hasClass('fade in')) {
        //            $('.modal-backdrop').removeClass('fade in')
        //            $('.modal-backdrop').addClass('fade out')
        //            $('#modal-container').removeClass('in');
        //        }
        //    }
        //});
    });

    function getUnique(array) {
        var uniqueArray = [];

        // Loop through array values
        for (i = 0; i < array.length; i++) {
            if (uniqueArray.indexOf(array[i]) === -1) {
                uniqueArray.push(array[i]);
            }
        }
        return uniqueArray;
    }

});




var loadUsersEps = (selectedUser) => {
    debugger;
    var url = CRxUrls.organization_epAssignment;
    var catId = localStorage.getItem("EPCatId")
    $.get(url + '?userId=' + selectedUser + '&categoryId=' + catId, function (data) {

        //$('#partialView').html(data);
        //$('#partialView').fadeIn('fast');
        checkEpbyUserIds()
        getSelectedEPCount()
    });
}

function loadData(userId) {

    var url = CRxUrls.organization_epAssignment;
    var catId = localStorage.getItem("EPCatId")
    $.get(url + '?userId=' + userId + '&categoryId=' + catId, function (data) {

        //$('#partialView').html(data);
        //$('#partialView').fadeIn('fast');
        checkEpbyUser()
    });
}