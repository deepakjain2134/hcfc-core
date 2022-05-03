var fileExtensionas = ['pdf', 'xlsx', 'xls', 'doc', 'docx', 'csv'];
var certificateExt = ['pdf', 'jpg', 'png', 'jpeg'];
var imageExt = ['jpg', 'jpeg', 'png'];
function Uploadfile(control, type) {
    debugger;
    var tempfileName = control.getAttribute("tempfileName");
    var tempfilecontent = control.getAttribute("tempfilecontent");
    var tempName = control.getAttribute("tempName");
    if (control.files.length > 0) {
        var file = control.files[0];
        var fileName = file.name;
        var name = fileName.split(".")[0];
        var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);
        if (checkExtension(fileExtension.toLowerCase(), type)) {
            infoAlert("Please upload " + type + " in these formats only :" + getFileExtensions(type), "")
            /*infoAlert("for " + type + " Only formats are allowed : " + getFileExtensions(type), "");*/
        } else {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {
                var array = reader.result.split(",");
                var prename = "";
                if (window.location.href.indexOf("addtpcra") > -1) {
                    prename = "TIcraLog."
                }
                $("input[name='" + prename + tempfileName + "'][type=hidden]").val(fileName);
                $("input[name='" + prename + tempfilecontent + "'][type=hidden]").val(array[1]);
                $("input[name='" + prename + tempName + "'][type=hidden]").val(name);
                $("input[name='" + prename + tempName + "'][type=text]").val(name);
            };
            reader.onerror = function (error) {
                console.log(error);
            };
        }
    }
}

function checkExtension(fileExtension, type) {
    var status = true;
    switch (type) {
        case 'image':
            status = imageExt.indexOf(fileExtension) > -1;
            break;
        case 'certificate':
            status = certificateExt.indexOf(fileExtension) > -1;
            break;
        default:
            status = fileExtensionas.indexOf(fileExtension) > -1;
            break;
    }
    return !status;
}

function getFileExtensions(type) {
    var extensions = "";
    switch (type) {
        case 'image':
            extensions = imageExt.join(', ');
            break;
        case 'certificate':
            extensions = certificateExt.join(', ');
            break;
        default:
            extensions = fileExtensionas.join(', ');
            break;
    }
    return extensions;
}