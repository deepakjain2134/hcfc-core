var datatable = $('#inboxTable').dataTable({
    "searching": true,
    "lengthChange": false,
    "bPaginate": $('#inboxTable tbody tr').length > 10,
    "bInfo": false,
    "aaSorting": [],
    "language": {
        emptyTable: "No data available in table",
        search: "_INPUT_", //To remove Search Label
        searchPlaceholder: "Search Date, From and Email Subject "
    },
    'aoColumnDefs': [{
        'bSortable': false,
        'aTargets': [-1, -2, -3, -4, -5, 1, 2] /* 1st one, start by the right */
    }],
    dom: "<'row'<'col-sm-12 text-right inboxlistAction'fp>>" +
        "<'row'<'col-sm-12'tr>>" +
        "<'row'<'col-sm-5'i><'col-sm-7 text-right'p>>"
});

$(document).on('click', 'div.inbox_file', function () {
    var href = $(this).attr("href");
    window.open(href, "_blank");
});

$(document).on("click", "a.delete", function (e) {
    e.preventDefault();
    var documentRepoId = $(this).data("id");
    $("tr").removeClass("trselected");
    $(this).parents("td").parents("tr").addClass('trselected');
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this Mail!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: false
    },
        function () {
            $.ajax({
                type: "GET",
                url: CRxUrls.Goal_DeleteDocuments + "?documentRepoId=" + documentRepoId,
                success: function (data) {
                    $("#tr" + documentRepoId).remove();
                    var datatable2 = $('#inboxTable').DataTable();
                    datatable2.row('.trselected').remove().draw(false);
                    swal("Deleted!", "Your mail has been deleted.", "success");
                },
                dataType: "json",
                traditional: true
            });
        });
});

$(document).on('click', ".description", function () {
    var html = $(this).attr("text");
    openDescription(html);
});

function openDescription(html) {
    swal({
        title: 'Description',
        text: html,
        html: true
    });
}

function fileDragStart(e) {
    e.dataTransfer.effectAllowed = "move";
    var json = {
        id: e.target.getAttribute("id"),
        fileName: e.target.getAttribute("alt"),
        filePath: e.target.getAttribute("href"),
        path: e.target.getAttribute("href"),
        fileId: e.target.getAttribute("fileId"),
        mailId: e.target.getAttribute("mailId"),
        doctypeId: e.target.getAttribute("doctypeid"),
        fileurl: e.target.getAttribute("fileurl"),
        documentId: e.target.getAttribute("documentid"),
        dtype: 'attachment',

    };
    e.dataTransfer.setData("text", JSON.stringify(json));
    console.log(JSON.stringify(json));
}

function inboxdragOver(e) {
    e.preventDefault();
    e.stopPropagation();
    var id = e.target.id;
    $("#" + id).addClass("trOverlay");
    $("#" + e.target.id).find("ul.doc_listing").hide();
}

function inboxdragleave(e) {
    $("td").removeClass("trOverlay");
    $("div").removeClass("trOverlay");
    $("ul.doc_listing").show();
}

function inboxdrop(e) {
    $("td").removeClass("trOverlay");
    $("div").removeClass("trOverlay");
    e.stopPropagation();
    e.preventDefault();
    var sourceData = JSON.parse(e.dataTransfer.getData("text"));
    var fileId = sourceData.fileId;
    var filePath = sourceData.path;
    var fileName = sourceData.fileName;
    var dtype = sourceData.dtype;
    var mailId = sourceData.mailId;
    var ctrId = e.target.id;
    var ctrTr = $("#" + ctrId).attr("trid");
    var docTypeId = $("#" + ctrId).attr("type");
    $("#" + ctrId).find("ul.doc_listing").show();
    if ("tr" + mailId === ctrTr) {
        if (dtype == "attachment") {
            var newFile = {
                DocTypeId: docTypeId,
                Id: fileId,
                FileName: fileName,
                FilePath: filePath,
                UploadedBy: 4
            };
            $("#" + ctrId).find("ul.doc_listing").append("<li class='list'></li>")
            $("#" + ctrId).find("ul.doc_listing li:last").append(document.getElementById("file" + fileId));
            $(".inbox_file[fileId='" + fileId + "']").removeAttr('Class').attr('Class', 'inbox_file');
            if (docTypeId == 106) {
                $(".inbox_file[fileId='" + fileId + "']").addClass('policy_file');
            } else if (docTypeId == 107) {
                $(".inbox_file[fileId='" + fileId + "']").addClass('report_file');
            } else if (docTypeId == 108) {
                $(".inbox_file[fileId='" + fileId + "']").addClass('miscdocs_file');
            } else if (docTypeId == -1) {
                $(".inbox_file[fileId='" + fileId + "']").addClass('sampledocs_file');
            }
            saveDocumentMaster(newFile);
        } else
            swalalert("please drop only attachment.");
    } else {
        $(".inbox_file").removeClass("trOverlay");
        return false;
    }
}

function saveDocumentMaster(newFile) {
    $.ajax({
        url: CRxUrls.Documents_UpdateAttachDocType,
        method: "POST",
        data: JSON.stringify({ file: newFile }),
        contentType: 'application/json',
        success: function () {
            $('li.list').each(function () {
                if ($(this).html().trim().length == 0) {
                    $(this).remove();
                }
            });
            $(".inbox_file").removeClass("trOverlay");           
        }
    });
}


function OpenReplyDocModal(docId) {
    $("#modal-container").empty();   
    const url = CRxUrls.Common_ReplyDocument + "?documentRepoId=" + docId + "&mode=inbox";
    $.get(url, function (data) {
        $("#modal-container").html(data);
        $("#modal-container").fadeIn("fast");
        $("#modal-container").modal("show");
    });
}