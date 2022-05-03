$(window).load(function () {
    $('#tblImportCSVData').DataTable({
        "searching": true,
        "lengthChange": false,
        "aaSorting": [],
        "aoColumnDefs": [{ 'bSortable': false, 'aTargets': [0] }],
        "bInfo": false,
        "language": { "emptyTable": "No data available in table" },
        "paging": false,
        "scrollY": "400px",
        "scrollCollapse": true
    });
    TotalRecord('tblImportCSVData'); //count total no. of record.
    BindNewSrcForDownloadCsv(); //set by default root for download sample csv file.
    $('#chkAllExportData').prop('checked', true);
    SelectAllCSV();//set by default select all csv data for upload.
});

//this function use for count no. of record.
function TotalRecord(tblid) {
    var totalRecord = $('#' + tblid+' tbody tr').length;
    var lblttl = '<label class="pull-left">Total record: ' + totalRecord + '</label>';
    $(lblttl).insertBefore($('#' + tblid +'_filter label'));
    $('#' + tblid +'_filter').css('width', '100%');
}

//this function use for check all export Data.
$(document).on('change', '#chkAllExportData', function () { SelectAllCSV(); });

//this function use for select all csv data for upload.
function SelectAllCSV() {
    $('#tblImportCSVData tbody tr').each(function () {
        $(this).find('td:eq(0) input[type=checkbox]').prop('checked', $('#chkAllExportData').is(':checked'));
    });
}

//this function use for chk and uncheck select all checkbox.
$(document).on('change', '.chkExportData', function () {
    var checkedln = 0, uncheckedln = 0;
    $('#tblImportCSVData tbody tr').each(function () {
        checkedln += $(this).find('td:eq(0) input[type=checkbox]:checked').length;
        uncheckedln += $(this).find('td:eq(0) input[type=checkbox]:not(:checked)').length;
    });
    $('#chkAllExportData').prop('checked', (uncheckedln > 0) ? false : true);
});

//this function use for generate dynamic selected data in JSON format.
function GetJSONFromSelectedCSVData() {
    var selectDataArr = [];
    var columns = [];
    $('#tblImportCSVData thead th').each(function () { columns.push($(this).text()); });
    $('#tblImportCSVData tbody tr').each(function () {
        var $tr = $(this);
        if ($tr.find('td:eq(0) .chkExportData').is(':checked')) {
            var jsonData = {};
            for (var i = 1; i <= $tr.find('td:not(:first)').length; i++)
                jsonData[columns[i]] = $tr.find('td:eq(' + i + ')').text();
            selectDataArr.push(jsonData);
        }
    });
    return selectDataArr;
}

//this function use for match CSV data from exist assets data.
$(document).on('click', '#btnUploadData', function () {
    var selectDataArr = GetJSONFromSelectedCSVData();
    var filetype = ($('#ddlFiletype')[0].selectedIndex > 0) ? $('#ddlFiletype').val() : '';
    $.post('/Import/UploadData', { selectjsondata: JSON.stringify(selectDataArr), filetype: filetype }, function (response) {
        if (response != '') {
            $('#dvFilterDataAfterUpload').empty().html(response);
        }
    });
    return false;
});

//Download CSV File.
function DownloadCSVFile(csvdata,fileName) {
    var hiddenElement = document.createElement('a');
    hiddenElement.href = 'data:text/csv;charset=utf-8,' + encodeURI(csvdata);
    hiddenElement.target = '_blank';
    hiddenElement.download = fileName+'.csv';
    hiddenElement.click();
}

//this function use for change filetype for download sample file for CSV data upload.
$(document).on('change', '#ddlFiletype', function () { BindNewSrcForDownloadCsv(); });

//this function use for download sample file for CSV data upload.
function BindNewSrcForDownloadCsv() {
    if ($('#ddlFiletype')[0].selectedIndex > 0) {
        var href = $('#DownloadSampleFile').attr('data-src');
        if (href.indexOf('?') > -1) {
            href = href.substr(0, href.lastIndexOf('?'));
        }
        href = href + '?filetype=' + $('#ddlFiletype').val();
        $('#DownloadSampleFile').attr('href', href);
    }
    else $('#DownloadSampleFile').attr('href', 'javascript:');
}

//$(document).on('click', '#btnSavedData', function () {
//    $.post('/Import/SaveCSVFilterNewAssets')
//        .done(function (response) {
//            if (response != '') alert(response);
//        })
//        .fail(function (response) {
//        });
//});

$(document).on('click', '#btnUpdateData', function () {
    $.post('/Import/UpdateExistedAssets')
        .done(function (response) {
            if (response != '') $('#lblmsg').text(response);
        })
        .fail(function (response) {
        });
});