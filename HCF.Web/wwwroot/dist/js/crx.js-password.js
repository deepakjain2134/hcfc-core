var count = 0;

var PasswordValidator = new function () {
    this.minSize = 7;
    this.maxSize = 15;
    this.lengthConfigured = true;
    this.uppercaseConfigured = true;
    this.LowercaseConfigured = true;
    this.digitConfigured = true;
    this.specialConfigured = true;
    this.prohibitedConfigured = true;

    this.specialCharacters = ['_', '#', '%', '*', '@'];
    this.prohibitedCharacters = ['$', '&', '=', '!'];


    this.elementnumber = 0;

    this.removePasswordValidator = function (passwordField, verifyField) {
        var confirm_passwordCtr = document.getElementById(verifyField);
        var passwordCtr = document.getElementById(passwordField);
        confirm_passwordCtr.setCustomValidity('');
        passwordCtr.setCustomValidity('');
    };

    this.setup = function (passwordField, verifyField) {
        console.log(passwordField);
        console.log(verifyField);
        this.elementnumber++;
        var passwordFieldEle = $('#' + passwordField);
        this.addPasswordField(passwordFieldEle);
        if (verifyField !== undefined) {
            var verifyFieldEle = $('#' + verifyField);
            this.addVerifyField(verifyFieldEle, $(passwordFieldEle).attr('id'));
        }
    };

    this.addPasswordField = function (passwordElement) {
        num = this.elementnumber;
        //Set Popover Attributes
        $(passwordElement).attr("data-placement", "left");
        $(passwordElement).attr("data-toggle", "popover");
        $(passwordElement).attr("data-trigger", "focus");
        $(passwordElement).attr("data-html", "true");
        $(passwordElement).attr("title", "Password Requirements");
        $(passwordElement).attr("onfocus", "PasswordValidator.onFocus(this," + num + ")");
        $(passwordElement).attr("onblur", "PasswordValidator.onBlur(this," + num + ")");
        $(passwordElement).attr("onkeyup", "PasswordValidator.checkPassword(this," + num + ")");

        //Create progress bar container
        var progressBardiv = document.createElement("div");

        progressBardiv.id = "progress" + num;
        $(progressBardiv).addClass("progress");

        //Progress bar element
        var progressBar = document.createElement("div");
        $(progressBar).addClass("progress-bar");
        $(progressBar).addClass("bg-info");
        progressBar.id = "progressBar" + num;
        $(progressBar).attr("role", "progressbar");
        $(progressBar).attr("aria-valuenow", "100");
        $(progressBar).attr("aria-valuemin", "0");
        $(progressBar).attr("aria-valuemax", "100");
        $(progressBar).css("width", "0%");
        $(progressBardiv).append(progressBar);

        //Add popover data including the progress bar
        $(passwordElement).attr("data-content", '&bull; Between 10-12 Characters <br/>&bull; An upper Case Letter<br/> &bull; A Number<br/> &bull; At Least 1 of the Following (_,-,#,%,*,+)<br/> &bull; None of the Following ($,&,=,!,@) <br/>' + progressBardiv.outerHTML);
    };

    //TODO: Add validation to check the repeat password field
    this.addVerifyField = function (verifyElement, passwordElementID) {
        $(verifyElement).attr("data-placement", "auto");
        $(verifyElement).attr("data-toggle", "popover");
        $(verifyElement).attr("data-trigger", "focus");
        $(verifyElement).attr("data-content", "Password Do Not Match!");
        $(verifyElement).attr("data-html", "true");
        $(verifyElement).attr("onfocus", "PasswordValidator.checkVerify(this,'" + passwordElementID + "')");
        $(verifyElement).attr("onkeyup", "PasswordValidator.checkVerify(this,'" + passwordElementID + "')");

    };

    //TODO: Check to see if the 2 passwords are the same
    this.checkVerify = function (verifyElement, passwordElementID) {
        var confirmPassword = $(verifyElement).attr("id");
        var confirm_passwordCtr = document.getElementById(confirmPassword);
        if ($('#' + passwordElementID).val() != confirm_passwordCtr.value) {
            confirm_passwordCtr.setCustomValidity("Confirm Password Don't Match");
        } else {
            confirm_passwordCtr.setCustomValidity('');
        }
    }

    this.checkPassword = function (e, num) {
        // var id = e.id;
        // var num = id.match(/\d/g);
        // num = num.join("");
        count = 0;
        var password = e.value;
        //var passwordCtr = e.id;      
        var passwordCtr = document.getElementById(e.id);
        // console.log(passwordCtr);

        var length = this.lengthConfigured ? this.checkLength(password) : '';
        var upper = this.uppercaseConfigured ? this.checkUpperCase(password) : '';
        var lower = this.LowercaseConfigured ? this.checkLowerCase(password) : '';
        var digit = this.digitConfigured ? this.checkDigit(password) : '';
        var special = this.specialConfigured ? this.checkSpecialCharacters(password) : '';
        var prohibited = this.prohibitedConfigured ? this.checkProhibitedCharacter(password) : '';
        if (length.length + upper.length + lower.length + digit.length + special.length + prohibited.length === 0) {
            $(e).popover('hide');
            $(e).addClass("is-invalid");
            passwordCtr.setCustomValidity("");
            return true;
        } else {
            $(e).popover('toggle');
            $(e).removeClass("is-valid");
            setProgressBar(count, e, num);
            var popover = $(e).attr("data-content", length + upper + lower + digit + special + prohibited + ' <br/>'/* + document.getElementById("progress" + num).outerHTML*/).data('bs.popover');
            popover.setContent();
            passwordCtr.setCustomValidity("the password is not strong enough");
            return false;
        }
    };

    this.checkSpecialCharacters = function (string) {
        // var specialChar = new RegExp("[_\\-#%*\\+]");
        var specialChar = new RegExp("[" + this.specialCharacters.join('') + "]");

        if (specialChar.test(string) == false) {
            return addPopoutLine("At Least 1 of the Following (" + this.specialCharacters.join(',') + ")");
        }
        else {
            count++;
            return "";
        }
    };

    this.checkProhibitedCharacter = function (string) {
        // var specialChar = new RegExp("[$&=!@]");//= /[$&=!@]/;
        var specialChar = new RegExp("[" + this.prohibitedCharacters.join('') + "]");

        if (specialChar.test(string) == true) {
            return addPopoutLine("None of the Following (" + this.prohibitedCharacters.join(',') + ")");
        }
        else {
            count++;
            return "";
        }
    };

    this.checkDigit = function checkDigit(string) {
        var hasNumber = /\d/;
        if (hasNumber.test(string) == false) {
            return addPopoutLine("A Number");
        }
        else {
            count++;
            return "";
        }
    };

    this.checkUpperCase = function (string) {
        if (string.replace(/[^A-Z]/g, "").length == 0) {
            return addPopoutLine("An Upper Case Letter");
        }
        else {
            count++;
            return "";
        }
    };




    this.checkLowerCase = function (string) {
        if (string.replace(/[^a-z]/g, "").length == 0) {
            return addPopoutLine("A Lower Case Letter");
        }
        else {
            count++;
            return "";
        }
    };


    this.checkLength = function (string) {
        if (string.length > this.maxSize || string.length < this.minSize) {
            return addPopoutLine("Between " + this.minSize + "-" + this.maxSize + " Characters");
        }
        else {
            count++;
            return "";
        }

    };

    function setProgressBar(percent, e, num) {
        percentNum = (percent / 5) * 100;
        percent = percentNum.toString() + "%";
        $(`#progressBar${num}`).css("width", percent);
    };

    function addPopoutLine(string) {
        return `&bull;${string}<br/>`;
    };

    this.onFocus = function (e, num) {
        this.checkPassword(e, num);
    };

    this.onBlur = function (e, num) {
        if (this.checkPassword(e, num) == false) {
            $(".popover").hide();
        }
    };
};


$(document).on("click", "button.close", function () {
    $(".popover").popover("hide");
});