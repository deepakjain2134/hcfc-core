/**
* jQuery Cookie plugin
*
* Copyright (c) 2010 Klaus Hartl (stilbuero.de)
* Dual licensed under the MIT and GPL licenses:
* http://www.opensource.org/licenses/mit-license.php
* http://www.gnu.org/licenses/gpl.html
*
*/
jQuery.cookie = function (key, value, options) {

    // key and at least value given, set cookie...
    if (arguments.length > 1 && String(value) !== "[object Object]") {
        options = jQuery.extend({}, options);

        if (value === null || value === undefined) {
            options.expires = -1;
        }

        if (typeof options.expires === 'number') {
            var days = options.expires, t = options.expires = new Date();
            t.setDate(t.getDate() + days);
        }

        value = String(value);

        return (document.cookie = [
            encodeURIComponent(key), '=',
            options.raw ? value : encodeURIComponent(value),
            options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
            options.path ? '; path=' + options.path : '',
            options.domain ? '; domain=' + options.domain : '',
            options.secure ? '; secure' : ''
        ].join(''));
    }

    // key and possibly options given, get cookie...
    options = value || {};
    var result, decode = options.raw ? function (s) { return s; } : decodeURIComponent;
    return (result = new RegExp('(?:^|; )' + encodeURIComponent(key) + '=([^;]*)').exec(document.cookie)) ? decode(result[1]) : null;
};


; (function ($) {

    let Transfer = function (element, options) {
        this.$element = element;
        // default options
        this.defaults = {
            // data item name
            itemName: "item",
            // group data item name
            groupItemName: "groupItem",
            // group data array name
            groupArrayName: "groupArray",
            // data value name
            valueName: "value",
            // tab text
            tabNameText: "items",
            // right tab text
            rightTabNameText: "selected items",
            // search placeholder text
            searchPlaceholderText: "search",
            // total text
            totalText: "total",
            // items data array
            dataArray: [],
            // group data array
            groupDataArray: []
        };
        // merge options
        this.settings = $.extend(this.defaults, options);

        // tab text
        this.tabNameText = this.settings.tabNameText;
        // right tab text
        this.rightTabNameText = this.settings.rightTabNameText;
        // search placeholder text
        this.searchPlaceholderText = this.settings.searchPlaceholderText;
        // default total number text template
        this.default_total_num_text_template = this.settings.totalText + ": {total_num}";
        // default zero item
        this.default_right_item_total_num_text = get_total_num_text(this.default_total_num_text_template, 0);
        // item total number
        this.item_total_num = this.settings.dataArray.length;
        // group item total number
        this.group_item_total_num = get_group_items_num(this.settings.groupDataArray, this.settings.groupArrayName);
        // use group
        this.isGroup = this.group_item_total_num > 0;
        // inner data
        this._data = new InnerMap();

        // Id
        this.id = (getId())();
        // id selector for the item searcher
        this.itemSearcherId = "#listSearch_" + this.id;
        // id selector for the group item searcher
        this.groupItemSearcherId = "#groupListSearch_" + this.id;
        // id selector for the right searcher
        this.selectedItemSearcherId = "#selectedListSearch_" + this.id;

        // class selector for the transfer-double-list-ul
        this.transferDoubleListUlClass = ".transfer-double-list-ul-" + this.id;
        // class selector for the transfer-double-list-li
        this.transferDoubleListLiClass = ".transfer-double-list-li-" + this.id;
        // class selector for the left checkbox item
        this.checkboxItemClass = ".checkbox-item-" + this.id;
        // class selector for the left checkbox item label
        this.checkboxItemLabelClass = ".checkbox-name-" + this.id;
        // class selector for the left item total number label
        this.totalNumLabelClass = ".total_num_" + this.id;
        // id selector for the left item select all
        this.leftItemSelectAllId = "#leftItemSelectAll_" + this.id;

        // class selector for the transfer-double-group-list-ul
        this.transferDoubleGroupListUlClass = ".transfer-double-group-list-ul-" + this.id;
        // class selector for the transfer-double-group-list-li
        this.transferDoubleGroupListLiClass = ".transfer-double-group-list-li-" + this.id;
        // class selector for the group select all
        this.groupSelectAllClass = ".group-select-all-" + this.id;
        // class selector fro the transfer-double-group-list-li-ul-li
        this.transferDoubleGroupListLiUlLiClass = ".transfer-double-group-list-li-ul-li-" + this.id;
        // class selector for the group-checkbox-item
        this.groupCheckboxItemClass = ".group-checkbox-item-" + this.id;
        // class selector for the group-checkbox-name
        this.groupCheckboxNameLabelClass = ".group-checkbox-name-" + this.id;
        // class selector for the left group item total number label
        this.groupTotalNumLabelClass = ".group_total_num_" + this.id;
        // id selector for the left group item select all
        this.groupItemSelectAllId = "#groupItemSelectAll_" + this.id;

        // class selector for the transfer-double-selected-list-ul
        this.transferDoubleSelectedListUlClass = ".transfer-double-selected-list-ul-" + this.id;
        // class selector for the transfer-double-selected-list-li
        this.transferDoubleSelectedListLiClass = ".transfer-double-selected-list-li-" + this.id;
        // class selector for the right select checkbox item
        this.checkboxSelectedItemClass = ".checkbox-selected-item-" + this.id;
        // id selector for the right item select all
        this.rightItemSelectAllId = "#rightItemSelectAll_" + this.id;
        // class selector for the
        this.selectedTotalNumLabelClass = ".selected_total_num_" + this.id;
        // id selector for the add button
        this.addSelectedButtonId = "#add_selected_" + this.id;
        // id selector for the delete button
        this.deleteSelectedButtonId = "#delete_selected_" + this.id;
    }

    $.fn.transfer = function (options) {
        // new Transfer
        let transfer = new Transfer(this, options);
        // init
        transfer.init();

        return {
            // get selected items
            getSelectedItems: function () {
                return get_selected_items(transfer)
            }
        }
    }

    /**
     * init
     */
    Transfer.prototype.init = function () {
        // generate transfer
        this.$element.append(this.generate_transfer());

        if (this.isGroup) {
            // fill group data
            this.fill_group_data();

            // left group checkbox item click handler
            this.left_group_checkbox_item_click_handler();
            // group select all handler
            this.group_select_all_handler();
            // group item select all handler
            this.group_item_select_all_handler();
            // left group items search handler
            this.left_group_items_search_handler();

        } else {
            // fill data
            this.fill_data();

            // left checkbox item click handler
            this.left_checkbox_item_click_handler();
            // left item select all handler
            this.left_item_select_all_handler();
            // left items search handler
            this.left_items_search_handler();
        }

        // right checkbox item click handler
        this.right_checkbox_item_click_handler();
        // move the pre-selection items to the right handler
        this.move_pre_selection_items_handler();
        // move the selected item to the left handler
        this.move_selected_items_handler();
        // right items search handler
        this.right_items_search_handler();
        // right item select all handler
        this.right_item_select_all_handler();
    }


    Transfer.prototype.generate_transfer = function () {
        let html =
            '<div class="transfer-double" id="transfer_double_' + this.id + '">'
            + '<div class="transfer-double-header"></div>'
            + '<div class="transfer-double-content clearfix">'
            + this.generate_left_part()
            + '<div class="transfer-double-content-middle">'
            + '<div class="btn-select-arrow" id="add_selected_' + this.id + '"><i class="fa fa-step-forward" aria-hidden="true"></i></div>'
            + '<div class="btn-select-arrow" id="delete_selected_' + this.id + '"><i class="fa fa-step-backward" aria-hidden="true"></i></div>'
            + '</div>'
            + this.generate_right_part()
            + '</div>'
            + '<div class="transfer-double-footer"></div>'
            + '</div>';
        return html;
    }

  
    Transfer.prototype.generate_left_part = function () {
        return '<div class="transfer-double-content-left">'
            + '<div class="transfer-double-content-param">'
            + '<div class="param-item">' + (this.isGroup ? this.tabNameText : this.tabNameText) + '</div>'
            + '</div>'
            + (this.isGroup ? this.generate_group_items_container() : this.generate_items_container())
            + '</div>'
    }


    Transfer.prototype.generate_group_items_container = function () {
        return '<div class="transfer-double-list transfer-double-list-' + this.id + '">'
            + '<div class="transfer-double-list-header">'
            + '<div class="transfer-double-list-search">'
            + '<input class="transfer-double-list-search-input" type="text" id="groupListSearch_' + this.id + '" placeholder="' + this.searchPlaceholderText + '" value="" />'
            + '</div>'
            + '</div>'
            + '<div class="transfer-double-list-content">'
            + '<div class="transfer-double-list-main">'
            + '<ul class="transfer-double-group-list-ul transfer-double-group-list-ul-' + this.id + '">'
            + '</ul>'
            + '</div>'
            + '</div>'
            + '<div class="transfer-double-list-footer">'
            + '<div class="checkbox-group">'
            + '<input type="checkbox" class="checkbox-normal" id="groupItemSelectAll_' + this.id + '"><label for="groupItemSelectAll_' + this.id + '" class="group_total_num_' + this.id + '"></label>'
            + '</div>'
            + '</div>'
            + '</div>'
    }

   
    Transfer.prototype.generate_items_container = function () {
        return '<div class="transfer-double-list transfer-double-list-' + this.id + '">'
            + '<div class="transfer-double-list-header">'
            + '<div class="transfer-double-list-search">'
            + '<input class="transfer-double-list-search-input" type="text" id="listSearch_' + this.id + '" placeholder="' + this.searchPlaceholderText + '" value="" />'
            + '</div>'
            + '</div>'
            + '<div class="transfer-double-list-content">'
            + '<div class="transfer-double-list-main">'
            + '<ul class="transfer-double-list-ul transfer-double-list-ul-' + this.id + '">'
            + '</ul>'
            + '</div>'
            + '</div>'
            + '<div class="transfer-double-list-footer">'
            + '<div class="checkbox-group">'
            + '<input type="checkbox" class="checkbox-normal" id="leftItemSelectAll_' + this.id + '"><label for="leftItemSelectAll_' + this.id + '" class="total_num_' + this.id + '"></label>'
            + '</div>'
            + '</div>'
            + '</div>'
    }

    Transfer.prototype.generate_right_part = function () {
        return '<div class="transfer-double-content-right">'
            + '<div class="transfer-double-content-param">'
            + '<div class="param-item">' + this.rightTabNameText + '</div>'
            + '</div>'
            + '<div class="transfer-double-selected-list">'
            + '<div class="transfer-double-selected-list-header">'
            + '<div class="transfer-double-selected-list-search">'
            + '<input class="transfer-double-selected-list-search-input" type="text" id="selectedListSearch_' + this.id + '" placeholder="' + this.searchPlaceholderText + '" value="" />'
            + '</div>'
            + '</div>'
            + '<div class="transfer-double-selected-list-content">'
            + '<div class="transfer-double-selected-list-main">'
            + '<ul class="transfer-double-selected-list-ul transfer-double-selected-list-ul-' + this.id + '">'
            + '</ul>'
            + '</div>'
            + '</div>'
            + '<div class="transfer-double-list-footer">'
            + '<div class="checkbox-group">'
            + '<input type="checkbox" class="checkbox-normal" id="rightItemSelectAll_' + this.id + '">'
            + '<label for="rightItemSelectAll_' + this.id + '" class="selected_total_num_' + this.id + '"></label>'
            + '</div>'
            + '</div>'
            + '</div>'
            + '</div>'
    }

   
    Transfer.prototype.fill_data = function () {
        this.$element.find(this.transferDoubleListUlClass).empty();
        this.$element.find(this.transferDoubleListUlClass).append(this.generate_left_items());

        this.$element.find(this.transferDoubleSelectedListUlClass).empty();
        this.$element.find(this.transferDoubleSelectedListUlClass).append(this.generate_right_items());

        // render total num
        this.$element.find(this.totalNumLabelClass).empty();
        this.$element.find(this.totalNumLabelClass).append(get_total_num_text(this.default_total_num_text_template, this._data.get("left_total_count")));

        // render right total num
        this.$element.find(this.selectedTotalNumLabelClass).empty();
        this.$element.find(this.selectedTotalNumLabelClass).append(get_total_num_text(this.default_total_num_text_template, this._data.get("right_total_count")));

        // callable
        applyCallable(this);
    }

    /**
     * fill group data
     */
    Transfer.prototype.fill_group_data = function () {
        this.$element.find(this.transferDoubleGroupListUlClass).empty();
        this.$element.find(this.transferDoubleGroupListUlClass).append(this.generate_left_group_items());

        this.$element.find(this.transferDoubleSelectedListUlClass).empty();
        this.$element.find(this.transferDoubleSelectedListUlClass).append(this.generate_right_group_items());

        let self = this;
        let left_total_count = 0;
        this._data.forEach(function (key, value) {
            if (Object.prototype.toString.call(value) === '[object Object]') {
                left_total_count += value["left_total_count"]
            }
            value["left_total_count"] == 0 ? self.$element.find("#" + key).prop("disabled", true).prop("checked", true) : void (0)
        })

        // render total num
        this.$element.find(this.groupTotalNumLabelClass).empty();
        this.$element.find(this.groupTotalNumLabelClass).append(get_total_num_text(this.default_total_num_text_template, left_total_count));

        // render right total num
        this.$element.find(this.selectedTotalNumLabelClass).empty();
        this.$element.find(this.selectedTotalNumLabelClass).append(get_total_num_text(this.default_total_num_text_template, this._data.get("right_total_count")));

        // callable
        applyCallable(this);
    }

 
    Transfer.prototype.generate_left_items = function () {
        let html = "";
        let dataArray = this.settings.dataArray;
        let itemName = this.settings.itemName;
        let valueName = this.settings.valueName;

        for (let i = 0; i < dataArray.length; i++) {

            let selected = dataArray[i].selected || false;
            let right_total_count = this._data.get("right_total_count") || 0;
            this._data.get("right_total_count") == undefined ? this._data.put("right_total_count", right_total_count) : void (0)
            selected ? this._data.put("right_total_count", ++right_total_count) : void (0)

            html +=
                '<li class="transfer-double-list-li transfer-double-list-li-' + this.id + ' ' + (selected ? 'selected-hidden' : '') + '">' +
                '<div class="checkbox-group">' +
                '<input type="checkbox" value="' + dataArray[i][valueName] + '" class="checkbox-normal checkbox-item-'
                + this.id + '" id="itemCheckbox_' + i + '_' + this.id + '">' +
                '<label class="checkbox-name-' + this.id + '" for="itemCheckbox_' + i + '_' + this.id + '">' + dataArray[i][itemName] + '</label>' +
                '</div>' +
                '</li>'
        }

        this._data.put("left_pre_selection_count", 0);
        this._data.put("left_total_count", dataArray.length - this._data.get("right_total_count"));

        return html;
    }

  
    Transfer.prototype.generate_left_group_items = function () {
        let html = "";
        let id = this.id;
        let groupDataArray = this.settings.groupDataArray;
        let groupItemName = this.settings.groupItemName;
        let groupArrayName = this.settings.groupArrayName;
        let itemName = this.settings.itemName;
        let valueName = this.settings.valueName;


        for (let i = 0; i < groupDataArray.length; i++) {
            if (groupDataArray[i][groupArrayName] && groupDataArray[i][groupArrayName].length > 0) {

                let _value = {};
                _value["left_pre_selection_count"] = 0
                _value["left_total_count"] = groupDataArray[i][groupArrayName].length
                this._data.put('group_' + i + '_' + this.id, _value);

                html +=
                    '<li class="transfer-double-group-list-li transfer-double-group-list-li-' + id + '">'
                    + '<div class="checkbox-group">' +
                    '<input type="checkbox" class="checkbox-normal group-select-all-' + id + '" id="group_' + i + '_' + id + '">' +
                    '<label for="group_' + i + '_' + id + '" class="group-name-' + id + '">' + groupDataArray[i][groupItemName] + '</label>' +
                    '</div>';

                html += '<ul class="transfer-double-group-list-li-ul transfer-double-group-list-li-ul-' + id + '">'
                for (let j = 0; j < groupDataArray[i][groupArrayName].length; j++) {

                    let selected = groupDataArray[i][groupArrayName][j].selected || false;
                    let right_total_count = this._data.get("right_total_count") || 0;
                    this._data.get("right_total_count") == undefined ? this._data.put("right_total_count", right_total_count) : void (0)
                    selected ? this._data.put("right_total_count", ++right_total_count) : void (0)

                    let groupItem = this._data.get('group_' + i + '_' + this.id);
                    selected ? groupItem["left_total_count"] -= 1 : void (0)

                    html += '<li class="transfer-double-group-list-li-ul-li transfer-double-group-list-li-ul-li-' + id + ' ' + (selected ? 'selected-hidden' : '') + '">' +
                        '<div class="checkbox-group">' +
                        '<input type="checkbox" value="' + groupDataArray[i][groupArrayName][j][valueName] + '" class="checkbox-normal group-checkbox-item-' + id + ' belongs-group-' + i + '-' + id + '" id="group_' + i + '_checkbox_' + j + '_' + id + '">' +
                        '<label for="group_' + i + '_checkbox_' + j + '_' + id + '" class="group-checkbox-name-' + id + '">' + groupDataArray[i][groupArrayName][j][itemName] + '</label>' +
                        '</div>' +
                        '</li>';
                }
                html += '</ul></li>'
            }
        }

        return html;
    }

  
    Transfer.prototype.generate_right_items = function () {
        let html = "";
        let dataArray = this.settings.dataArray;
        let itemName = this.settings.itemName;
        let valueName = this.settings.valueName;
        let selected_count = 0;

        this._data.put("right_pre_selection_count", selected_count);
        this._data.put("right_total_count", selected_count);

        for (let i = 0; i < dataArray.length; i++) {
            if (dataArray[i].selected || false) {
                this._data.put("right_total_count", ++selected_count);
                html += this.generate_item(this.id, i, dataArray[i][valueName], dataArray[i][itemName]);
            }
        }

        if (this._data.get("right_total_count") == 0) {
            $(this.rightItemSelectAllId).prop("checked", true).prop("disabled", "disabled");
        }

        return html;
    }


    Transfer.prototype.generate_right_group_items = function () {
        let html = "";
        let groupDataArray = this.settings.groupDataArray;
        let groupArrayName = this.settings.groupArrayName;
        let itemName = this.settings.itemName;
        let valueName = this.settings.valueName;
        let selected_count = 0;

        this._data.put("right_pre_selection_count", selected_count);
        this._data.put("right_total_count", selected_count);

        for (let i = 0; i < groupDataArray.length; i++) {
            if (groupDataArray[i][groupArrayName] && groupDataArray[i][groupArrayName].length > 0) {
                for (let j = 0; j < groupDataArray[i][groupArrayName].length; j++) {
                    if (groupDataArray[i][groupArrayName][j].selected || false) {
                        this._data.put("right_total_count", ++selected_count);
                        html += this.generate_group_item(this.id, i, j, groupDataArray[i][groupArrayName][j][valueName], groupDataArray[i][groupArrayName][j][itemName]);
                    }
                }
            }
        }

        if (this._data.get("right_total_count") == 0) {
            $(this.rightItemSelectAllId).prop("checked", true).prop("disabled", "disabled");
        }

        return html;
    }

 
    Transfer.prototype.left_checkbox_item_click_handler = function () {
        let self = this;
        self.$element.on("click", self.checkboxItemClass, function () {
            let pre_selection_num = 0;
            $(this).is(":checked") ? pre_selection_num++ : pre_selection_num--

            let left_pre_selection_count = self._data.get("left_pre_selection_count");
            self._data.put("left_pre_selection_count", left_pre_selection_count + pre_selection_num);

            if (self._data.get("left_pre_selection_count") > 0) {
                $(self.addSelectedButtonId).addClass("btn-arrow-active");
            } else {
                $(self.addSelectedButtonId).removeClass("btn-arrow-active");
            }

            if (self._data.get("left_pre_selection_count") < self._data.get("left_total_count")) {
                $(self.leftItemSelectAllId).prop("checked", false);
            } else if (self._data.get("left_pre_selection_count") == self._data.get("left_total_count")) {
                $(self.leftItemSelectAllId).prop("checked", true);
            }
        });
    }


    Transfer.prototype.left_group_checkbox_item_click_handler = function () {
        let self = this;
        self.$element.on("click", self.groupCheckboxItemClass, function () {
            let pre_selection_num = 0;
            let total_pre_selection_num = 0;
            let remain_left_total_count = 0

            $(this).is(":checked") ? pre_selection_num++ : pre_selection_num--

            let groupIndex = $(this).prop("id").split("_")[1];
            let groupItem = self._data.get('group_' + groupIndex + '_' + self.id);
            let left_pre_selection_count = groupItem["left_pre_selection_count"];
            groupItem["left_pre_selection_count"] = left_pre_selection_count + pre_selection_num

            self._data.forEach(function (key, value) {
                if (Object.prototype.toString.call(value) === '[object Object]') {
                    total_pre_selection_num += value["left_pre_selection_count"]
                    remain_left_total_count += value["left_total_count"]
                }
            });

            if (total_pre_selection_num > 0) {
                $(self.addSelectedButtonId).addClass("btn-arrow-active");
            } else {
                $(self.addSelectedButtonId).removeClass("btn-arrow-active");
            }

            if (groupItem["left_pre_selection_count"] < groupItem["left_total_count"]) {
                self.$element.find("#group_" + groupIndex + "_" + self.id).prop("checked", false);
            } else if (groupItem["left_pre_selection_count"] == groupItem["left_total_count"]) {
                self.$element.find("#group_" + groupIndex + "_" + self.id).prop("checked", true);
            }

            if (total_pre_selection_num == remain_left_total_count) {
                $(self.groupItemSelectAllId).prop("checked", true);
            } else {
                $(self.groupItemSelectAllId).prop("checked", false);
            }
        });
    }


    Transfer.prototype.group_select_all_handler = function () {
        let self = this;
        $(self.groupSelectAllClass).on("click", function () {
            // group index
            let groupIndex = ($(this).attr("id")).split("_")[1];
            let groups = self.$element.find(".belongs-group-" + groupIndex + "-" + self.id);
            let left_pre_selection_count = 0;
            let left_total_count = 0;

            // a group is checked
            if ($(this).is(':checked')) {
                // active button
                $(self.addSelectedButtonId).addClass("btn-arrow-active");
                for (let i = 0; i < groups.length; i++) {
                    if (!groups.eq(i).is(':checked') && groups.eq(i).parent("div").parent("li").css("display") != "none") {
                        groups.eq(i).prop("checked", true);
                    }
                }

                let groupItem = self._data.get($(this).prop("id"));
                groupItem["left_pre_selection_count"] = groupItem["left_total_count"];

                self._data.forEach(function (key, value) {
                    if (Object.prototype.toString.call(value) === '[object Object]') {
                        left_pre_selection_count += value["left_pre_selection_count"];
                        left_total_count += value["left_total_count"];
                    }
                })

                if (left_pre_selection_count == left_total_count) {
                    $(self.groupItemSelectAllId).prop("checked", true);
                }
            } else {
                for (let j = 0; j < groups.length; j++) {
                    if (groups.eq(j).is(':checked') && groups.eq(j).parent("div").parent("li").css("display") != "none") {
                        groups.eq(j).prop("checked", false);
                    }
                }

                self._data.get($(this).prop("id"))["left_pre_selection_count"] = 0;

                self._data.forEach(function (key, value) {
                    if (Object.prototype.toString.call(value) === '[object Object]') {
                        left_pre_selection_count += value["left_pre_selection_count"];
                        left_total_count += value["left_total_count"];
                    }
                })

                if (left_pre_selection_count != left_total_count) {
                    $(self.groupItemSelectAllId).prop("checked", false);
                }

                if (left_pre_selection_count == 0) {
                    $(self.addSelectedButtonId).removeClass("btn-arrow-active");
                }
            }
        });
    }

    /**
     * group item select all handler
     */
    Transfer.prototype.group_item_select_all_handler = function () {
        let self = this;
        $(self.groupItemSelectAllId).on("click", function () {
            let groupCheckboxItems = self.$element.find(self.groupCheckboxItemClass);
            if ($(this).is(':checked')) {
                for (let i = 0; i < groupCheckboxItems.length; i++) {
                    if (groupCheckboxItems.parent("div").parent("li").eq(i).css('display') != "none" && !groupCheckboxItems.eq(i).is(':checked')) {
                        groupCheckboxItems.eq(i).prop("checked", true);
                        let groupIndex = groupCheckboxItems.eq(i).prop("id").split("_")[1];
                        if (!self.$element.find(self.groupSelectAllClass).eq(groupIndex).is(':checked')) {
                            self.$element.find(self.groupSelectAllClass).eq(groupIndex).prop("checked", true);
                        }
                    }
                }

                self._data.forEach(function (key, value) {
                    if (Object.prototype.toString.call(value) === '[object Object]') {
                        value["left_pre_selection_count"] = value["left_total_count"];
                    }
                })

                $(self.addSelectedButtonId).addClass("btn-arrow-active");
            } else {
                for (let i = 0; i < groupCheckboxItems.length; i++) {
                    if (groupCheckboxItems.parent("div").parent("li").eq(i).css('display') != "none" && groupCheckboxItems.eq(i).is(':checked')) {
                        groupCheckboxItems.eq(i).prop("checked", false);
                        let groupIndex = groupCheckboxItems.eq(i).prop("id").split("_")[1];
                        if (self.$element.find(self.groupSelectAllClass).eq(groupIndex).is(':checked')) {
                            self.$element.find(self.groupSelectAllClass).eq(groupIndex).prop("checked", false);
                        }
                    }
                }

                self._data.forEach(function (key, value) {
                    if (Object.prototype.toString.call(value) === '[object Object]') {
                        value["left_pre_selection_count"] = 0;
                    }
                })

                $(self.addSelectedButtonId).removeClass("btn-arrow-active");
            }
        });
    }

    /**
     * left group items search handler
     */
    Transfer.prototype.left_group_items_search_handler = function () {
        let self = this;
        $(self.groupItemSearcherId).on("keyup", function () {
            self.$element.find(self.transferDoubleGroupListUlClass).css('display', 'block');
            let transferDoubleGroupListLiUlLis = self.$element.find(self.transferDoubleGroupListLiUlLiClass);
            if ($(self.groupItemSearcherId).val() == "") {
                for (let i = 0; i < transferDoubleGroupListLiUlLis.length; i++) {
                    if (!transferDoubleGroupListLiUlLis.eq(i).hasClass("selected-hidden")) {
                        transferDoubleGroupListLiUlLis.eq(i).parent("ul").parent("li").css('display', 'block');
                        transferDoubleGroupListLiUlLis.eq(i).css('display', 'block');
                    } else {
                        transferDoubleGroupListLiUlLis.eq(i).parent("ul").parent("li").css('display', 'block');
                    }
                }
                return;
            }

            // Mismatch
            self.$element.find(self.transferDoubleGroupListLiClass).css('display', 'none');
            transferDoubleGroupListLiUlLis.css('display', 'none');

            for (let j = 0; j < transferDoubleGroupListLiUlLis.length; j++) {
                if (!transferDoubleGroupListLiUlLis.eq(j).hasClass("selected-hidden")
                    && transferDoubleGroupListLiUlLis.eq(j).text().trim()
                        .substr(0, $(self.groupItemSearcherId).val().length).toLowerCase() == $(self.groupItemSearcherId).val().toLowerCase()) {
                    transferDoubleGroupListLiUlLis.eq(j).parent("ul").parent("li").css('display', 'block');
                    transferDoubleGroupListLiUlLis.eq(j).css('display', 'block');
                }
            }
        });
    }

    /**
     * left item select all handler
     */
    Transfer.prototype.left_item_select_all_handler = function () {
        let self = this;
        $(self.leftItemSelectAllId).on("click", function () {
            let checkboxItems = self.$element.find(self.checkboxItemClass);
            if ($(this).is(':checked')) {
                for (let i = 0; i < checkboxItems.length; i++) {
                    if (checkboxItems.eq(i).parent("div").parent("li").css('display') != "none" && !checkboxItems.eq(i).is(':checked')) {
                        checkboxItems.eq(i).prop("checked", true);
                    }
                }
                self._data.put("left_pre_selection_count", self._data.get("left_total_count"));
                $(self.addSelectedButtonId).addClass("btn-arrow-active");
            } else {
                for (let i = 0; i < checkboxItems.length; i++) {
                    if (checkboxItems.eq(i).parent("div").parent("li").css('display') != "none" && checkboxItems.eq(i).is(':checked')) {
                        checkboxItems.eq(i).prop("checked", false);
                    }
                }
                $(self.addSelectedButtonId).removeClass("btn-arrow-active");
                self._data.put("left_pre_selection_count", 0);
            }
        });
    }

    /**
     * right item select all handler
     */
    Transfer.prototype.right_item_select_all_handler = function () {
        let self = this;
        $(self.rightItemSelectAllId).on("click", function () {
            let checkboxSelectedItems = self.$element.find(self.checkboxSelectedItemClass);
            if ($(this).is(':checked')) {
                self._data.put("right_pre_selection_count", 0);
                let right_pre_selection_count = self._data.get("right_pre_selection_count");
                for (let i = 0; i < checkboxSelectedItems.length; i++) {
                    checkboxSelectedItems.eq(i).prop("checked", true);
                    self._data.put("right_pre_selection_count", ++right_pre_selection_count);
                }

                $(self.deleteSelectedButtonId).addClass("btn-arrow-active");

                if (self._data.get("right_pre_selection_count") < self._data.get("right_total_count")) {
                    $(self.rightItemSelectAllId).prop("checked", false);
                } else if (self._data.get("right_pre_selection_count") == self._data.get("right_total_count")) {
                    $(self.rightItemSelectAllId).prop("checked", true);
                }

            } else {
                for (let i = 0; i < checkboxSelectedItems.length; i++) {
                    for (let i = 0; i < checkboxSelectedItems.length; i++) {
                        checkboxSelectedItems.eq(i).prop("checked", false);
                    }
                }
                $(self.deleteSelectedButtonId).removeClass("btn-arrow-active");
                self._data.put("right_pre_selection_count", 0);
            }

        });
    }

    /**
     * left items search handler
     */
    Transfer.prototype.left_items_search_handler = function () {
        let self = this;
        $(self.itemSearcherId).on("keyup", function () {
            let transferDoubleListLis = self.$element.find(self.transferDoubleListLiClass);
            self.$element.find(self.transferDoubleListUlClass).css('display', 'block');
            if ($(self.itemSearcherId).val() == "") {
                for (let i = 0; i < transferDoubleListLis.length; i++) {
                    if (!transferDoubleListLis.eq(i).hasClass("selected-hidden")) {
                        self.$element.find(self.transferDoubleListLiClass).eq(i).css('display', 'block');
                    }
                }
                return;
            }

            transferDoubleListLis.css('display', 'none');

            for (let j = 0; j < transferDoubleListLis.length; j++) {
                if (!transferDoubleListLis.eq(j).hasClass("selected-hidden")
                    && transferDoubleListLis.eq(j).text().trim()
                        .substr(0, $(self.itemSearcherId).val().length).toLowerCase() == $(self.itemSearcherId).val().toLowerCase()) {
                    transferDoubleListLis.eq(j).css('display', 'block');
                }
            }
        });
    }

    /**
     * right checkbox item click handler
     */
    Transfer.prototype.right_checkbox_item_click_handler = function () {
        let self = this;
        self.$element.on("click", self.checkboxSelectedItemClass, function () {
            let pre_selection_num = 0;
            $(this).is(":checked") ? pre_selection_num++ : pre_selection_num--

            let right_pre_selection_count = self._data.get("right_pre_selection_count");
            self._data.put("right_pre_selection_count", right_pre_selection_count + pre_selection_num);

            if (self._data.get("right_pre_selection_count") > 0) {
                $(self.deleteSelectedButtonId).addClass("btn-arrow-active");
            } else {
                $(self.deleteSelectedButtonId).removeClass("btn-arrow-active");
            }

            if (self._data.get("right_pre_selection_count") < self._data.get("right_total_count")) {
                $(self.rightItemSelectAllId).prop("checked", false);
            } else if (self._data.get("right_pre_selection_count") == self._data.get("right_total_count")) {
                $(self.rightItemSelectAllId).prop("checked", true);
            }

        });
    }

    /**
     * move the pre-selection items to the right handler
     */
    Transfer.prototype.move_pre_selection_items_handler = function () {
        let self = this;
        $(self.addSelectedButtonId).on("click", function () {
            self.isGroup ? self.move_pre_selection_group_items() : self.move_pre_selection_items()
            // callable
            applyCallable(self);
        });
    }

    /**
     * move the pre-selection group items to the right
     */
    Transfer.prototype.move_pre_selection_group_items = function () {
        let pre_selection_num = 0;
        let html = "";
        let groupCheckboxItems = this.$element.find(this.groupCheckboxItemClass);
        for (let i = 0; i < groupCheckboxItems.length; i++) {
            if (!groupCheckboxItems.eq(i).parent("div").parent("li").hasClass("selected-hidden") && groupCheckboxItems.eq(i).is(':checked')) {
                let checkboxItemId = groupCheckboxItems.eq(i).attr("id");
                let groupIndex = checkboxItemId.split("_")[1];
                let itemIndex = checkboxItemId.split("_")[3];
                let labelText = this.$element.find(this.groupCheckboxNameLabelClass).eq(i).text();
                let value = groupCheckboxItems.eq(i).val();

                html += this.generate_group_item(this.id, groupIndex, itemIndex, value, labelText);
                groupCheckboxItems.parent("div").parent("li").eq(i).css("display", "").addClass("selected-hidden");
                pre_selection_num++;

                let groupItem = this._data.get('group_' + groupIndex + '_' + this.id);
                let left_total_count = groupItem["left_total_count"];
                let left_pre_selection_count = groupItem["left_pre_selection_count"];
                let right_total_count = this._data.get("right_total_count");
                groupItem["left_total_count"] = --left_total_count;
                groupItem["left_pre_selection_count"] = --left_pre_selection_count;
                this._data.put("right_total_count", ++right_total_count);
            }
        }

        if (pre_selection_num > 0) {
            let groupSelectAllArray = this.$element.find(this.groupSelectAllClass);
            for (let j = 0; j < groupSelectAllArray.length; j++) {
                if (groupSelectAllArray.eq(j).is(":checked")) {
                    groupSelectAllArray.eq(j).prop("disabled", "disabled");
                }
            }

            let remain_left_total_count = 0;
            this._data.forEach(function (key, value) {
                if (Object.prototype.toString.call(value) === '[object Object]') {
                    remain_left_total_count += value["left_total_count"];
                }
            })

            let groupTotalNumLabel = this.$element.find(this.groupTotalNumLabelClass);
            groupTotalNumLabel.empty();
            groupTotalNumLabel.append(get_total_num_text(this.default_total_num_text_template, remain_left_total_count));
            this.$element.find(this.selectedTotalNumLabelClass).text(get_total_num_text(this.default_total_num_text_template, this._data.get("right_total_count")));

            if (remain_left_total_count == 0) {
                $(this.groupItemSelectAllId).prop("checked", true).prop("disabled", "disabled");
            }

            if (this._data.get("right_total_count") > 0) {
                $(this.rightItemSelectAllId).prop("checked", false).removeAttr("disabled");
            }

            $(this.addSelectedButtonId).removeClass("btn-arrow-active");
            let transferDoubleSelectedListUl = this.$element.find(this.transferDoubleSelectedListUlClass);
            transferDoubleSelectedListUl.append(html);
        }
    }

    /**
     * move the pre-selection items to the right
     */
    Transfer.prototype.move_pre_selection_items = function () {
        let pre_selection_num = 0;
        let html = "";
        let self = this;
        let checkboxItems = self.$element.find(self.checkboxItemClass);
        for (let i = 0; i < checkboxItems.length; i++) {
            if (checkboxItems.eq(i).parent("div").parent("li").css("display") != "none" && checkboxItems.eq(i).is(':checked')) {
                let checkboxItemId = checkboxItems.eq(i).attr("id");
                // checkbox item index
                let index = checkboxItemId.split("_")[1];
                let labelText = self.$element.find(self.checkboxItemLabelClass).eq(i).text();
                let value = checkboxItems.eq(i).val();
                self.$element.find(self.transferDoubleListLiClass).eq(i).css("display", "").addClass("selected-hidden");
                html += self.generate_item(self.id, index, value, labelText);
                pre_selection_num++;

                let left_pre_selection_count = self._data.get("left_pre_selection_count");
                let left_total_count = self._data.get("left_total_count");
                let right_total_count = self._data.get("right_total_count");
                self._data.put("left_pre_selection_count", --left_pre_selection_count);
                self._data.put("left_total_count", --left_total_count);
                self._data.put("right_total_count", ++right_total_count);
            }
        }

        if (self._data.get("right_total_count") > 0) {
            $(self.rightItemSelectAllId).prop("checked", false).removeAttr("disabled");
        }

        if (pre_selection_num > 0) {
            let totalNumLabel = self.$element.find(self.totalNumLabelClass);
            totalNumLabel.empty();
            totalNumLabel.append(get_total_num_text(self.default_total_num_text_template, self._data.get("left_total_count")));
            self.$element.find(self.selectedTotalNumLabelClass).text(get_total_num_text(self.default_total_num_text_template, self._data.get("right_total_count")));
            if (self._data.get("left_total_count") == 0) {
                $(self.leftItemSelectAllId).prop("checked", true).prop("disabled", "disabled");
            }

            $(self.addSelectedButtonId).removeClass("btn-arrow-active");
            self.$element.find(self.transferDoubleSelectedListUlClass).append(html);
        }
    }

    /**
     * move the selected item to the left handler
     */
    Transfer.prototype.move_selected_items_handler = function () {
        let self = this;
        $(self.deleteSelectedButtonId).on("click", function () {
            self.isGroup ? self.move_selected_group_items() : self.move_selected_items()
            $(self.deleteSelectedButtonId).removeClass("btn-arrow-active");
            // callable
            applyCallable(self);
        });
    }

    /**
     * move the selected group item to the left
     */
    Transfer.prototype.move_selected_group_items = function () {
        let pre_selection_num = 0;
        let checkboxSelectedItems = this.$element.find(this.checkboxSelectedItemClass);
        for (let i = 0; i < checkboxSelectedItems.length;) {
            let another_checkboxSelectedItems = this.$element.find(this.checkboxSelectedItemClass);
            if (another_checkboxSelectedItems.eq(i).is(':checked')) {
                let checkboxSelectedItemId = another_checkboxSelectedItems.eq(i).attr("id");
                let groupIndex = checkboxSelectedItemId.split("_")[1];
                let index = checkboxSelectedItemId.split("_")[3];

                another_checkboxSelectedItems.parent("div").parent("li").eq(i).remove();
                this.$element.find("#group_" + groupIndex + "_" + this.id).prop("checked", false).removeAttr("disabled");
                this.$element.find("#group_" + groupIndex + "_checkbox_" + index + "_" + this.id)
                    .prop("checked", false).parent("div").parent("li").css("display", "").removeClass("selected-hidden");

                pre_selection_num++;

                let groupItem = this._data.get('group_' + groupIndex + '_' + this.id);
                let left_total_count = groupItem["left_total_count"];
                let right_pre_selection_count = this._data.get("right_pre_selection_count");
                let right_total_count = this._data.get("right_total_count");
                groupItem["left_total_count"] = ++left_total_count;
                this._data.put("right_total_count", --right_total_count);
                this._data.put("right_pre_selection_count", --right_pre_selection_count);

            } else {
                i++;
            }
        }
        if (pre_selection_num > 0) {
            this.$element.find(this.groupTotalNumLabelClass).empty();

            let remain_left_total_count = 0;
            this._data.forEach(function (key, value) {
                if (Object.prototype.toString.call(value) === '[object Object]') {
                    remain_left_total_count += value["left_total_count"];
                }
            })

            if (this._data.get("right_total_count") == 0) {
                $(this.rightItemSelectAllId).prop("checked", true).prop("disabled", "disabled");
            }

            this.$element.find(this.groupTotalNumLabelClass).append(get_total_num_text(this.default_total_num_text_template, remain_left_total_count));
            this.$element.find(this.selectedTotalNumLabelClass).text(get_total_num_text(this.default_total_num_text_template, this._data.get("right_total_count")));
            if ($(this.groupItemSelectAllId).is(':checked')) {
                $(this.groupItemSelectAllId).prop("checked", false).removeAttr("disabled");
            }
        }
    }

    /**
     * move the selected item to the left
     */
    Transfer.prototype.move_selected_items = function () {
        let pre_selection_num = 0;
        let self = this;

        for (let i = 0; i < self.$element.find(self.checkboxSelectedItemClass).length;) {
            let checkboxSelectedItems = self.$element.find(self.checkboxSelectedItemClass);
            if (checkboxSelectedItems.eq(i).is(':checked')) {
                let index = checkboxSelectedItems.eq(i).attr("id").split("_")[1];
                checkboxSelectedItems.parent("div").parent("li").eq(i).remove();
                self.$element.find(self.checkboxItemClass).eq(index).prop("checked", false);
                self.$element.find(self.transferDoubleListLiClass).eq(index).css("display", "").removeClass("selected-hidden");

                pre_selection_num++;

                let right_total_count = self._data.get("right_total_count");
                let right_pre_selection_count = self._data.get("right_pre_selection_count");
                self._data.put("right_total_count", --right_total_count);
                self._data.put("right_pre_selection_count", --right_pre_selection_count);

                let left_total_count = self._data.get("left_total_count");
                self._data.put("left_total_count", ++left_total_count);


            } else {
                i++;
            }
        }

        if (self._data.get("right_total_count") == 0) {
            $(self.rightItemSelectAllId).prop("checked", true).prop("disabled", "disabled");
        }


        if (pre_selection_num > 0) {
            self.$element.find(self.totalNumLabelClass).empty();
            self.$element.find(self.totalNumLabelClass).append(get_total_num_text(self.default_total_num_text_template, self._data.get("left_total_count")));
            self.$element.find(self.selectedTotalNumLabelClass).text(get_total_num_text(self.default_total_num_text_template, self._data.get("right_total_count")));
            if ($(self.leftItemSelectAllId).is(':checked')) {
                $(self.leftItemSelectAllId).prop("checked", false).removeAttr("disabled");
            }
        }
    }

    /**
     * right items search handler
     */
    Transfer.prototype.right_items_search_handler = function () {
        let self = this;
        $(self.selectedItemSearcherId).keyup(function () {
            let transferDoubleSelectedListLis = self.$element.find(self.transferDoubleSelectedListLiClass);
            self.$element.find(self.transferDoubleSelectedListUlClass).css('display', 'block');

            if ($(self.selectedItemSearcherId).val() == "") {
                transferDoubleSelectedListLis.css('display', 'block');
                return;
            }

            transferDoubleSelectedListLis.css('display', 'none');

            for (let i = 0; i < transferDoubleSelectedListLis.length; i++) {
                if (transferDoubleSelectedListLis.eq(i).text().trim()
                    .substr(0, $(self.selectedItemSearcherId).val().length).toLowerCase() == $(self.selectedItemSearcherId).val().toLowerCase()) {
                    transferDoubleSelectedListLis.eq(i).css('display', 'block');
                }
            }
        });
    }


    Transfer.prototype.generate_item = function (id, index, value, labelText) {
        return '<li class="transfer-double-selected-list-li  transfer-double-selected-list-li-' + id + ' .clearfix">' +
            '<div class="checkbox-group">' +
            '<input type="checkbox" value="' + value + '" class="checkbox-normal checkbox-selected-item-' + id + '" id="selectedCheckbox_' + index + '_' + id + '">' +
            '<label class="checkbox-selected-name-' + id + '" for="selectedCheckbox_' + index + '_' + id + '">' + labelText + '</label>' +
            '</div>' +
            '</li>';
    }


    Transfer.prototype.generate_group_item = function (id, groupIndex, itemIndex, value, labelText) {
        return '<li class="transfer-double-selected-list-li transfer-double-selected-list-li-' + id + ' .clearfix">' +
            '<div class="checkbox-group">' +
            '<input type="checkbox" value="' + value + '" class="checkbox-normal checkbox-selected-item-' + id + '" id="group_' + groupIndex + '_selectedCheckbox_' + itemIndex + '_' + id + '">' +
            '<label class="checkbox-selected-name-' + id + '" for="group_' + groupIndex + '_selectedCheckbox_' + itemIndex + '_' + id + '">' + labelText + '</label>' +
            '</div>' +
            '</li>'
    }

    function applyCallable(transfer) {
        if (Object.prototype.toString.call(transfer.settings.callable) === "[object Function]") {
            let selected_items = get_selected_items(transfer);
            console.log(selected_items, transfer);
            // send reply in case of empty array
            //if (selected_items.length > 0) {
            transfer.settings.callable.call(transfer, selected_items);
            //}
        }
    }

    function get_selected_items(transfer) {
        let selected = [];
        let transferDoubleSelectedListLiArray = transfer.$element.find(transfer.transferDoubleSelectedListLiClass);
        for (let i = 0; i < transferDoubleSelectedListLiArray.length; i++) {
            let checkboxGroup = transferDoubleSelectedListLiArray.eq(i).find(".checkbox-group");

            let item = {};
            item[transfer.settings.itemName] = checkboxGroup.find("label").text();
            item[transfer.settings.valueName] = checkboxGroup.find("input").val();
            selected.push(item);
        }
        return selected;
    }


    function get_group_items_num(groupDataArray, groupArrayName) {
        let group_item_total_num = 0;
        for (let i = 0; i < groupDataArray.length; i++) {
            let groupItemData = groupDataArray[i][groupArrayName];
            if (groupItemData && groupItemData.length > 0) {
                group_item_total_num = group_item_total_num + groupItemData.length;
            }
        }
        return group_item_total_num;
    }


    function get_total_num_text(template, total_num) {
        let _template = template;
        return _template.replace(/{total_num}/g, total_num);
    }

    /**
     * Inner Map
     */
    function InnerMap() {
        this.keys = new Array();
        this.values = new Object();

        this.put = function (key, value) {
            if (this.values[key] == null) {
                this.keys.push(key);
            }
            this.values[key] = value;
        }
        this.get = function (key) {
            return this.values[key];
        }
        this.remove = function (key) {
            for (let i = 0; i < this.keys.length; i++) {
                if (this.keys[i] === key) {
                    this.keys.splice(i, 1);
                }
            }
            delete this.values[key];
        }
        this.forEach = function (fn) {
            for (let i = 0; i < this.keys.length; i++) {
                let key = this.keys[i];
                let value = this.values[key];
                fn(key, value);
            }
        }
        this.isEmpty = function () {
            return this.keys.length == 0;
        }
        this.size = function () {
            return this.keys.length;
        }
    }

    function getId() {
        let counter = 0;
        return function (prefix) {
            let id = (+new Date()).toString(32), i = 0;
            for (; i < 5; i++) {
                id += Math.floor(Math.random() * 65535).toString(32);
            }
            return (prefix || '') + id + (counter++).toString(32);
        }
    }

}(jQuery));
//function createAlert(title, summary, details, severity, dismissible, autoDismiss, appendToId) {
//  var iconMap = {
//    info: "fa fa-info-circle",
//    success: "fa fa-thumbs-up",
//    warning: "fa fa-exclamation-triangle",
//    danger: "fa ffa fa-exclamation-circle"
//  };

//  var iconAdded = false;

//  var alertClasses = ["alert", "animated", "flipInX"];
//  alertClasses.push("alert-" + severity.toLowerCase());

//  if (dismissible) {
//    alertClasses.push("alert-dismissible");
//  }

//  var msgIcon = $("<i />", {
//    "class": iconMap[severity] // you need to quote "class" since it's a reserved keyword
//  });

//  var msg = $("<div />", {
//    "class": alertClasses.join(" ") // you need to quote "class" since it's a reserved keyword
//  });

//  if (title) {
//    var msgTitle = $("<h4 />", {
//      html: title
//    }).appendTo(msg);
    
//    if(!iconAdded){
//      msgTitle.prepend(msgIcon);
//      iconAdded = true;
//    }
//  }

//  if (summary) {
//    var msgSummary = $("<strong />", {
//      html: summary
//    }).appendTo(msg);
    
//    if(!iconAdded){
//      msgSummary.prepend(msgIcon);
//      iconAdded = true;
//    }
//  }

//  if (details) {
//    var msgDetails = $("<p />", {
//      html: details
//    }).appendTo(msg);
    
//    if(!iconAdded){
//      msgDetails.prepend(msgIcon);
//      iconAdded = true;
//    }
//  }
  

//  if (dismissible) {
//    var msgClose = $("<span />", {
//      "class": "close", // you need to quote "class" since it's a reserved keyword
//      "data-dismiss": "alert",
//      html: "<i class='fa fa-times-circle'></i>"
//    }).appendTo(msg);
//  }

//  $('#' + appendToId).empty();

//  $('#' + appendToId).prepend(msg);
  
//  if(autoDismiss){
//   setTimeout(function(){
//      msg.addClass("flipOutX");
//      setTimeout(function(){
//        msg.remove();
//      },1000);
//   }, 5000);
//  }
//}

function successAlert(details,title, summary) {
	//createAlert(title,
 //       summary,
 //       details,
 //       'success',
 //       true,
 //       true,
 //       'pageMessages');

    AlertSuccessMsg(details, "Success");
}

function dangerAlert(details, title, summary) {

    AlertWarningMsg(details, title);
    //createAlert(title,
    //    summary,
    //    details,
    //    'danger',
    //    true,
    //    true,
    //    'pageMessages');
}

function infoAlert(details, title, summary) {
    //createAlert(title,
    //    summary,
    //    details,
    //    'info',
    //    true,
    //    true,
    //    'pageMessages');

    AlertInfoMsg(details, title);
}

function warningAlert(details, title, summary) {

    AlertWarningMsg(details, title);
    //createAlert(title,
    //    summary,
    //    details,
    //    'warning',
    //    true,
    //    true,
    //    'pageMessages');
}
$(document).ready(function () {
    $(document).on({
        'show.bs.modal': function () {
            var zIndex = 1040 + (10 * $('.modal:visible').length);
            $(this).css('z-index', zIndex);
            setTimeout(function () {
                $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
            }, 0);
        },
        'hidden.bs.modal': function () {
            if ($('.modal:visible').length > 0) {
                // restore the modal-open class to the body element, so that scrolling works
                // properly after de-stacking a modal.
                setTimeout(function () {
                    $(document.body).addClass('modal-open');
                }, 0);
            }
        }
    }, '.modal');
});

$(function () {


    $(window).on("load", function () {
        $(".crx-scrollBar").mCustomScrollbar({
            setHeight: 300,
            theme: "3d-dark"
        });
    });
    $(window).on("load", function () {
        $(".crx-mainscrollBar").mCustomScrollbar({
            setHeight: 300,
            theme: "3d-thick-dark"
        });
    });

    //$(".epDescription").shorten({ "showChars": 225, "seeMore": false, "moreText": "See More" });

    //$('.descriptions').popover({ trigger: 'focus' });
    $(".descriptions").popover({ trigger: "manual", html: true, animation: false })
        .on("mouseenter", function () {
            var _this = this;
            $(this).popover("show");
            $(".popover").on("mouseleave", function () {
                $(_this).popover('hide');
            });
        }).on("mouseleave", function () {
            var _this = this;
            setTimeout(function () {
                if (!$(".popover:hover").length) {
                    $(_this).popover("hide");
                }
            }, 50);
        });

    $('#myTable').dataTable({
        searching: false,
        pageLength: 20,
        lengthChange: false,
        bPaginate: $("#myTable tbody tr").length > 20,
        bInfo: false,
        stateSave: true,
        aaSorting: [],
        language: {
            emptyTable: "No data available in table",
            search: "_INPUT_", //To remove Search Label
            searchPlaceholder: "Search..."
        },
        aoColumnDefs: [
            {
                orderSequence: ["desc", "asc"],
                aTargets: ['_all']
            }
        ],
        dom: "<'row'<'col-sm-3'l><'col-sm-3'f><'col-sm-6'p>>" +
            // "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12'<'table-responsive'tr>>>" +
            "<'row'<'col-sm-5'i><'col-sm-7 text-right'p>>"
    });



    $(".dataTables_wrapper input[type='search']").attr("maxlength", 25);


    $.fn.dataTable.moment('MM d,yyyy');

    $.fn.dataTable.moment('MM d,yyyy hh:mm A');

    $('.alphaonly').alpha();

    $(".numeric").numericInput();

    $('.phone').mask('(000) 000-0000');

    $(".phone").on("blur", function () {
        var last = $(this).val().substr($(this).val().indexOf("-") + 1);
        if (last.length === 5) {
            var move = $(this).val().substr($(this).val().indexOf("-") + 1, 1);
            var lastfour = last.substr(1, 4);
            var first = $(this).val().substr(0, 9);
            $(this).val(first + move + '-' + lastfour);
        }
    });

    $("body").on("click", ".modal-link", function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });

    $('body').on("click", ".modal-close-btn", function () {
        $('#modal-container').modal('hide');
    });

    $("#modal-container").on('hidden.bs.modal', function () {
        $(this).removeData("bs.modal");
    });

    $("#CancelModal").on("click", function () {
        return false;
    });

    $("input").on("keypress", function (e) {
        //debugger;
        if (e.which === 32 && !this.value.length) {
            e.preventDefault();
        }
        else if (/^\s/.test(this.value))
            this.value = "";
    });


    //var checkConnection = function () {
    //    var status = navigator.onLine;
    //    if (status) {
    //        return true;
    //    } else {
    //        AlertInfoMsg("No Internet Connection !!");
    //        return false;
    //    }
    //}

    //console.log("%cCRx", "font-size: 144px;    font-weight: bold;");console.log("%c by HCF compliance", "font-size: 44px;    font-weight: bold;");

    SetTimeZone();

    var isFullScreen = localStorage.getItem("isFullScreen");
    if (isFullScreen === 1) {
        toggleFullScreen(document.body);
    }

    // start datepicker

    $("img.ui-datepicker-trigger").attr('alt', "Select date").attr("title", "Select date");

    //$('.newsdatepicker').datepicker({
    //    showOn: "both",
    //    buttonImage: ImgUrls.datepicker_calendar,
    //    buttonImageOnly: true,
    //    dateFormat: $.CRx_DateFormat,
    //    maxDate: 60,
    //    beforeShow: changeYearButtons,
    //    onChangeMonthYear: changeYearButtons
    //});

    $(".datepicker").datepicker({
        showOn: "both",
        buttonImage: ImgUrls.datepicker_calendar,
        buttonImageOnly: true,
        dateFormat: $.CRx_DateFormat,
        maxDate: 0,
        beforeShow: changeYearButtons,
        onChangeMonthYear: changeYearButtons
        //   dateFormat: 'MMM d, yyyy'
    });

    //$('.date').datepicker({
    //    changeYear: false,
    //    minDate: mindate,
    //    maxDate: maxdate,
    //    dateFormat: 'D, M d'
    //    // dateFormat: 'D, M d, yy'
    //    //dateFormatD:'D,MM d,yy'
    //});

    $('.futuredatepicker').datepicker({
        showOn: "both",
        buttonImage: ImgUrls.datepicker_calendar,
        buttonImageOnly: true,
        dateFormat: $.CRx_DateFormat,
        beforeShow: changeYearButtons,
        onChangeMonthYear: changeYearButtons
    });

    $(".datepicker, .startDate, .endDate, .futuredatepicker, .timepicker , .discopy ").on("cut copy paste", function (e) { e.preventDefault(); });

    $(".datepicker, .startDate, .endDate, .futuredatepicker , .timepicker , .discopy ").keypress(function (e) { e.preventDefault(); });


    $("body").on("focus", ".fromSearch", function () {
        $(this).datepicker();
    });


    var from = $(".startDate").datepicker({
        showOn: "both",
        numberOfMonths: 1,
        buttonImage: ImgUrls.datepicker_calendar,
        buttonImageOnly: true,
        dateFormat: $.CRx_DateFormat,
        beforeShow: changeYearButtonStartDate,
        onChangeMonthYear: changeYearButtonStartDate
    }).on("change", function () {
        // debugger;

        // to.datepicker("option", "minDate", $(this).val());
        $(".endDate").datepicker("option", "minDate", $(".startDate").datepicker("getDate"));
        $(".startDate").datepicker("option", "maxDate", $(".endDate").datepicker("getDate"));
        //$(".startDate").focus();
    }),
        to = $(".endDate").datepicker({
            showOn: "both",
            numberOfMonths: 1,
            buttonImage: ImgUrls.datepicker_calendar,
            buttonImageOnly: true,
            dateFormat: $.CRx_DateFormat,
            beforeShow: changeYearButtonEndDate,
            onChangeMonthYear: changeYearButtonEndDate
        }).on("change", function () {
            from.datepicker("option", "maxDate", $(this).val());

        });


    // end date
    $('.dropdown-toggle').click(function (e) {
        e.preventDefault();
        setTimeout($.proxy(function () {
            if ('ontouchstart' in document.documentElement) {
                $(this).siblings('.dropdown-backdrop').off().remove();
            }
        }, this), 0);
    });

});
//$(function () { $(".ddlboxLive").selectpicker('render'); })

$(".ddlboxLive").selectpicker().attr('data-live-search', true).attr('data-size', 5).attr('data-width', 'auto');

$(".ddlbox").selectpicker().attr('data-live-search', false).attr('data-size', 5).attr('data-width', 'auto');


var setDateFormat = function (date) {
    var newDate = date.toLocaleDateString("en-US", { month: 'short' })
        + " " + date.toLocaleDateString("en-US", { day: 'numeric' })
        + ", " + date.toLocaleDateString("en-US", { year: 'numeric' });
    return newDate;
};


var changeYearButtons = function (ctr, op1, op2) {
    var inputDate = $('.datepicker');
    if (ctr.id) {
        inputDate = $('#' + ctr.id);
    } else {
        inputDate = $('#' + op2.id);
    }
    changeDatePickerYear(inputDate);
};

//var changeYearButtons = function () {
//    var inputDate = $('.datepicker');
//    changeDatePickerYear(inputDate);
//};

var changeYearButtonStartDate = function () {
    var inputDate = $('.startDate');
    changeDatePickerYear(inputDate);
};

var changeYearButtonEndDate = function () {
    var inputDate = $('.endDate');
    changeDatePickerYear(inputDate);
};

var changeDatePickerYear = function (inputDate) {
    setTimeout(function () {
        var nextYearClass = "btnYear nextyear";
        var prevYearClass = "btnYear prevyear";
        var widgetHeader = inputDate.datepicker("widget").find(".ui-datepicker-header");

        var nextMonth = inputDate.datepicker("widget").find(".ui-datepicker-next");
        if (nextMonth.hasClass("ui-state-disabled")) {
            nextYearClass = "btnYear nextyear disable";
        }
        var nextYrBtn = $('<a class="' + nextYearClass + '" title="NextYr"></a>');
        nextYrBtn.bind("click", function () {
            $.datepicker._adjustDate(inputDate, +1, 'Y');
        });

        var prevMonth = inputDate.datepicker("widget").find(".ui-datepicker-prev");
        if (prevMonth.hasClass("ui-state-disabled")) {
            prevYearClass = "btnYear prevyear disable";
        }
        var prevYrBtn = $('<a class="' + prevYearClass + '" title="PrevYr"></a>');
        prevYrBtn.bind("click", function () {
            $.datepicker._adjustDate(inputDate, -1, 'Y');
        });

        prevYrBtn.appendTo(widgetHeader);
        nextYrBtn.appendTo(widgetHeader);

    }, 1);

};


$(window).scroll(function () {
    var sticky = $('#myHeader'),
        scroll = $(window).scrollTop();

    if (scroll >= 100) sticky.addClass('sticky');
    else sticky.removeClass('sticky');
    $("input.hasDatepicker").blur();
});

function SetTimeZone() {
    const timezoneCookie = "timezoneoffset";
    if (!$.cookie(timezoneCookie)) {
        const testCookie = 'hcf cookie';
        $.cookie(testCookie, true);
        if ($.cookie(testCookie)) {
            $.cookie(testCookie, null);
            $.cookie(timezoneCookie, new Date().getTimezoneOffset());
            location.reload();
        }
    }
    else {
        const storedOffset = parseInt($.cookie(timezoneCookie));
        const currentOffset = new Date().getTimezoneOffset();
        if (storedOffset !== currentOffset) {
            $.cookie(timezoneCookie, new Date().getTimezoneOffset());
            location.reload();
        }
    }
}

$(".toggle_menu").click(function () {
    $(".navbar").toggle();
});

$body = $("body");

$(document).on({
    ajaxComplete: function () {
        $body.removeClass("loading");
    },
    beforeSend: function (xhr) {
        xhr.setRequestHeader("XSRF-TOKEN",
            $('input:hidden[name="__RequestVerificationToken"]').val());
    },
    ajaxError: function (event, jqxhr, settings) {
        $body.removeClass("loading");
        //debugger;
        if (jqxhr.status != "200") {
            console.log(jqxhr);
            if (jqxhr.status == "401") {
                // swalalert("Login", "your session has expired please login again ", "warning");
            } else if (jqxhr.status == 0) {
                //swalalert("Unexpected Error ", "We are not able to connect with server. Please contact the system administrator. Error Code=" + jqxhr.status, "warning");
                //swalalertLogin("Login", "An unexpected error has occurred ", "warning");
            } else {
                //swalalert("Unexpected Error ", "An unexpected error has occurred. Please contact the system administrator. Error Code=" + jqxhr.status, "warning");
            }
        }
    },
    ajaxSend: function (event, jqXHR, ajaxOptions) {

        //var myCookieCookie = getCookie("loginUserId");
        //if (myCookieCookie == null) {
        //    ///swalalert("Login", "your session has expired please login again ", "warning");
        //    jqXHR.abort();
        //    return;
        //}
    },
    ajaxStart: function (e, xhr, options) {

       // console.log("ajaxStart");

        $body.addClass("loading");
    },
    ajaxStop: function () {
        $body.removeClass("loading");
    },
    ajaxSuccess: function () {
    }

});


function getCookie(name) {
    var dc = document.cookie;
    var prefix = name + "=";
    var begin = dc.indexOf("; " + prefix);
    if (begin == -1) {
        begin = dc.indexOf(prefix);
        if (begin != 0) return null;
    }
    else {
        begin += 2;
        var end = document.cookie.indexOf(";", begin);
        if (end == -1) {
            end = dc.length;
        }
    }
    return decodeURI(dc.substring(begin + prefix.length, end));
}


//common method for inspection comment

$(document).on("click", ".commentInsp", function () {
    openActivityCommentComment($(this));

});

var openActivityCommentComment = function (control) {
    $("#Commenttxt").val("");
    var ctrId = $(control).attr("id");
    var controlName = $(control).attr("tempname");
    localStorage.setItem("hdnfield", controlName);
    localStorage.setItem("clickCtr", ctrId);
    var value = $("input[name='" + controlName + "'][type=hidden]").val();
    $("#Commenttxt").val(value);
};

$(document).on("click", "#closeComment", function () {
    $('#commentModal').modal('hide');
});

$(document).on("click", "#saveComment", function () {
    var comment = $("#Commenttxt").val();
    var hdnfield = localStorage.getItem("hdnfield");
    commenthdn = $("input[name='" + hdnfield + "'][type=hidden]");
    commenthdn.val(comment);
    var clickCtr = localStorage.getItem("clickCtr");
    if (comment.length > 0) {
        $("#" + clickCtr).addClass("text");
    } else
        $("#" + clickCtr).removeClass("text");

    $("#commentModal").modal("hide");
});

//end


function getfileUrl(src) {
    debugger;
    const filename = src.replace(/^.*[\\\/]/, '');
    const extension = filename.substr((filename.lastIndexOf('.') + 1));
    if (extension === 'pdf') {
        return src + "#toolbar=1&view=FitH";
    } else if (extension === 'doc' || extension === 'docx' || extension === 'xlsx' || extension === 'xls') {
        src = $.Constants.Browse_View_Files_In_IFrame + src + "&embedded=true";
    }
    else if (extension === 'png' || extension === 'jpeg' || extension === 'jpg') {
        src = src;
    }
    return src;
}


function loadSignView(fileNameCtr, fileContentCtr, imgPreview = "") {
    $('#signAndSave').empty();
    var url = CRxUrls.common_digitalsignature;
    $.ajax({
        url: url + "?fileName=" + fileNameCtr + "&fileContent=" + fileContentCtr + "&imgPreviewClass=" + imgPreview,
        cache: false,
        type: "GET",
        success: function (data) {
            $('#signAndSave').append(data);
            $('#signAndSave').fadeIn('fast');
        }
    });
}
//function loadSignViewNew(fileNameCtr, fileContentCtr, imgPreview = "") {
//    $('#signAndSave').empty();
//    var url = CRxUrls.common_digitalsignature;
//    $.ajax({
//        url: url + "?fileName=" + fileNameCtr + "&fileContent=" + fileContentCtr + "&imgPreviewClass=" + imgPreview ,
//        cache: false,
//        type: "GET",
//        success: function (data) {
//            $('#signAndSave').append(data);
//            $('#signAndSave').fadeIn('fast');
//        }
//    });
//}

//// Automatic Success Alert close script Start
//$(".alert-success").hide();
//$(".alert-success").fadeTo(15000, 500).slideUp(500, function () {
//    $(".alert-success").slideUp(500);
//});

//$(".alert-danger").hide();
//$(".alert-danger").fadeTo(15000, 500).slideUp(500, function () {
//    $(".alert-danger").slideUp(500);
//});
//// Automatic Success Alert close script End


function LoadRecentFiles() {
    $("#modal-container1 .close").click();
    $(".modal-backdrop.in").hide();
    var modelContainer = $("#modal-container");
    modelContainer.empty();
    const recentFiles = CRxUrls.Common_RecentFiles;
    $.get(recentFiles, function (data) {
        modelContainer.html('');
        modelContainer.html(data);
        modelContainer.fadeIn('fast');
        modelContainer.modal('show');
    });
}


function hideModel() {
    $("#modal-container1 .close").click();
    $(".modal-backdrop.in").hide();
}

function LoadDrawingPathway() {
    debugger;
    $("#modal-container .close").click();
    $(".modal-backdrop.in").hide();
    var modelContainer = $("#modal-container1");
    modelContainer.empty();
    const recentFiles = CRxUrls.Common_DrawingPathway;
    $.get(recentFiles, function (data) {
        modelContainer.html('');
        modelContainer.html(data);
        modelContainer.fadeIn('fast');
        modelContainer.modal('show');
    });
}
function SendMailPopUp() {
    var modelContainer = $("#modal-container");
    modelContainer.empty();
    const replyDocumentUrl = CRxUrls.Common_ReplyDocument;
    $.get(replyDocumentUrl, function (data) {
        modelContainer.html('');
        modelContainer.html(data);
        modelContainer.fadeIn('fast');
        modelContainer.modal('show');
    });
}

//$(".dynamic_menu").click(function () {
//    debugger;
//    if ($(this).hasClass('activeMenu')) {
//        $(this).removeClass('activeMenu');
//    } else {
//        $('.dynamic_menu').removeClass('activeMenu');
//        $(this).addClass('activeMenu');
//    }
//});

//$(".multi-column").mouseleave(function () {
//    $(".dynamic_menu").removeClass("activeMenu");
//});


var jsformat = function (str, col) {
    if (col && str) {
        col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 1);
        return str.replace(/\{\{|\}\}|\{(\w+)\}/g,
            function (m, n) {
                if (m === "{{") {
                    return "{";
                }
                if (m === "}}") {
                    return "}";
                }
                return col[n];
            });
    }
};


// auto logout user when user logout on one of the opened tab start

$(document).ready(function () {
    if (window.localStorage) {
        $('a#_linkLogout').on('click', function () {
            localStorage.setItem("app-logout", 'logout' + Math.random());
            return true;
        });
    }
});

window.addEventListener('storage', function (event) {
    if (event.key == "app-logout") {
        window.location = $('a#_linkLogout').attr('href');
    }
}, false);

// end

$(document).ready(function () {
    $(".ePDescriptions").shorten({
        moreText: 'read more',
        showChars: 120,
        lessText: 'read less'
    });
});






function AddReadMore() {
    var carLmt = 240;
    var readMoreTxt = " ... Read More";
    var readLessTxt = " Read Less";
    $(".addReadMore").each(function () {
        if ($(this).find(".firstSec").length)
            return;

        var allstr = $(this).text();
        if (allstr.length > carLmt) {
            var firstSet = allstr.substring(0, carLmt);
            var secdHalf = allstr.substring(carLmt, allstr.length);
            var strtoadd = firstSet + "<span class='SecSec'>" + secdHalf + "</span><span class='readMore'  title='Click to Show More'>" + readMoreTxt + "</span><span class='readLess' title='Click to Show Less'>" + readLessTxt + "</span>";
            $(this).html(strtoadd);
        }

    });
    $(document).on("click", ".readMore,.readLess", function () {
        $(this).closest(".addReadMore").toggleClass("showlesscontent showmorecontent");
    });
}
$(function () {
    AddReadMore();
});
(function (exports) {
    function valOrFunction(val, ctx, args) {
        if (typeof val == "function") {
            return val.apply(ctx, args);
        } else {
            return val;
        }
    }

    function InvalidInputHelper(input, options) {

        input.setCustomValidity(valOrFunction(options.defaultText, window, [input]));

        function changeOrInput() {
            if (input.value == "") {
                input.setCustomValidity(valOrFunction(options.emptyText, window, [input]));
            } else {
                input.setCustomValidity("");
            }
        }

        function invalid() {
            if (input.value == "") {
                input.setCustomValidity(valOrFunction(options.emptyText, window, [input]));
            } else {
                input.setCustomValidity(valOrFunction(options.invalidText, window, [input]));
            }
        }
      
        input.addEventListener("change", changeOrInput);
        input.addEventListener("input", changeOrInput);
        input.addEventListener("invalid", invalid);
    }

    function floatNumber(number) {      
        var regexp = /^\d+(\.\d{1,2})?$/;
        return regexp.test(number);        
    }


    exports.InvalidInputHelper = InvalidInputHelper;
})(window);
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
(function ($) {
    $.fn.extend({
        grouploop: function (options) {
            var settings = $.extend(
                {
                    velocity: 2,
                    forward: true,
                    childNode: ".item",
                    childWrapper: ".item-wrap",
                    pauseOnHover: true,
                    complete: null,
                    stickFirstItem: false
                },
                options
            );

            var curXPos;
            var stickyItemWidth;
            var el = this;
            var windowWidth = $(window).width() < 768 ? $(window).width() * 2 : $(window).width();
            var v = settings.velocity; // velocity
            var wrapperWidth = $(window).width() < 768 ? $(el).width() * 2 : $(el).width();
            var firstItem = $(el)
                .find(settings.childWrapper + " " + settings.childNode)
                .first();
            var numChildren = $(el).find(settings.childWrapper + " " + settings.childNode).length;

            if (settings.stickFirstItem) {
                stickFirstItemFunc();
            } else {
                stickyItemWidth = 0;
            }

            function stickFirstItemFunc() {
                stickyItemWidth = wrapperWidth / (numChildren - 1);
                el.css("position", "relative");
                firstItem.remove();
                firstItem.css({
                    position: "absolute",
                    top: "0",
                    left: "0",
                    width: stickyItemWidth,
                    height: "100%",
                    "z-index": "999"
                });
                firstItem.prependTo(el).find(settings.childWrapper);
                // console.log(
                //   "wrapperWidth:" +
                //     wrapperWidth +
                //     "\n" +
                //     "numChildren: " +
                //     numChildren +
                //     "\n" +
                //     "stickyItemWidth: " +
                //     stickyItemWidth
                // );
            }

            window.addEventListener("resize", function () {
                windowWidth = $(window).width();

                // console.log(windowWidth);
                if (windowWidth < 768) {
                    console.log("Small breakpoint. Wrapper width is currently doubled.");
                    windowWidth = windowWidth * 2;
                    wrapperWidth = $(el).width() * 2;
                } else {
                    windowWidth = $(window).width();
                    wrapperWidth = $(el).width();
                }
                if (settings.stickFirstItem) {
                    stickFirstItemFunc();
                }
            });

            if (settings.forward) {
                curXPos = -windowWidth;

                $(el).each(function (index, elem) {
                    $(elem)
                        .find(settings.childWrapper)
                        .each(function (index, elem) {
                            $(
                                $(elem).find(
                                    $(settings.childNode)
                                        .get()
                                        .reverse()
                                )
                            ).each(function () {
                                $(this)
                                    .clone()
                                    .prependTo(elem);
                            });
                        });
                });
            } else {
                if (
                    $(el)
                        .find(settings.childWrapper)
                        .css("transform") !== "none"
                ) {
                    curXPos = $(el)
                        .find(settings.childWrapper)
                        .css("transform")
                        .split(/[()]/)[1]
                        .split(",")[4];
                } else {
                    curXPos = 0;
                }

                $(el).each(function (index, elem) {
                    $(elem)
                        .find(settings.childWrapper)
                        .each(function (index, elem) {
                            $(elem)
                                .find(settings.childNode)
                                .each(function () {
                                    $(this)
                                        .clone()
                                        .appendTo(elem);
                                });
                        });
                });
            }

            var myReq;

            function step() {
                if (settings.forward) {
                    if (curXPos <= 0) {
                        curXPos = curXPos + 1 * v;
                        $(el)
                            .find(settings.childWrapper)
                            .css("transform", "translateX(" + curXPos + "px)");
                    } else {
                        $(el)
                            .find(settings.childWrapper)
                            .css("transform", "translateX(" + -windowWidth - stickyItemWidth + ")");
                        curXPos = -windowWidth;
                    }
                } else {
                    if (curXPos >= -windowWidth) {
                        curXPos = curXPos - 1 * v;
                        $(el)
                            .find(settings.childWrapper)
                            .css("transform", "translateX(" + curXPos + "px)");
                    } else {
                        $(el)
                            .find(settings.childWrapper)
                            .css("transform", "translateX(" + 0 + ")");
                        curXPos = 0;
                    }
                }

                myReq = window.requestAnimationFrame(step);
            }

            myReq = window.requestAnimationFrame(step);

            if (settings.pauseOnHover) {
                $(this).hover(
                    function () {
                        cancelAnimationFrame(myReq);
                    },
                    function () {
                        myReq = window.requestAnimationFrame(step);
                    }
                );
            }

            return this.each(function () {
                if ($.isFunction(settings.complete)) {
                    settings.complete.call(this);
                }
            });
        }
    });
})(jQuery);

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
;( function( $, window, document, undefined ) {
	
	"use strict";
	
	var pluginName = "pinlogin",
	
	defaults = {
		fields: 6,						// number of fields
		placeholder : '•',				// character that's displayed after entering a number in a field
		autofocus : true,				// focus on the first field at loading time
		hideinput : true,				// hide the input digits and replace them with placeholder
		reset : true,					// resets all fields when completely filled
		complete : function(pin){		// fires when all fields are filled in 
			// pin	:	the entered pincode
		},
		invalid : function(field, nr){	// fires when user enters an invalid value in a field
			// field: 	the jquery field object
			// nr	:	the field number
		},
		keydown : function(e, field, nr){	// fires when user pressed a key down in a field
			// e	:	the event object
			// field: 	the jquery field object
			// nr	:	the field number
		},		
		input : function(e, field, nr){	// fires when a value is entered in a field
			// e	:	the event object
			// field: the jquery field object
			// nr:	the field number
		}			
	};

	
	// constructor
	function Plugin ( element, options ) {
		this.element = element;

		// jQuery has an extend method which merges the contents of two or
		// more objects, storing the result in the first object. The first object
		// is generally empty as we don't want to alter the default options for
		// future instances of the plugin
		this.settings = $.extend( {}, defaults, options );
		this._defaults = defaults;
		this._name = pluginName;
		this.init();
	}	
	
		// Avoid Plugin.prototype conflicts
		$.extend( Plugin.prototype, {
			init: function() {

				// Place initialization logic here
				// You already have access to the DOM element and
				// the options via the instance, e.g. this.element
				// and this.settings
				// you can add more functions like the one below and
				// call them like the example below
				//this.yourOtherFunction( "jQuery Boilerplate" );
				this._values = new Array(this.settings.fields); // keeping track of the entered values
	
				this._container = $('<div />').prop({
					'id' : this._name,
					'class' : this._name
				});
			
				// main loop creating the fields
				for (var i = 0; i < this.settings.fields; i++)
				{
					var input = this._createInput(this._getFieldId(i), i);
					
					this._attachEvents(input, i);
					
					this._container.append(input);
				}
				
				$(this.element).append(this._container);
				
				// reset all fields to starting state
				this.reset();
			},
			
			// create a single input field
			_createInput : function(id, nr)
			{
				return $('<input>').prop({
					'type' : 'tel',				// Thanks to Manuja Jayawardana (https://gitlab.com/mjayawardana) this is 'tel' and not 'text'!:)
					'id': id,
					'name': id,
					'maxlength': 1,
					'inputmode' : 'numeric',
					'x-inputmode' : 'numeric',
					'pattern' : '^[0-9]*$',
					'autocomplete' : 'off',
					'class' : 'pinlogin-field'
				});
			},
			
			// attach events to the field
			_attachEvents : function(field, nr)
			{
				var that = this;

				field.on('focus', $.proxy(function(e){
					$(this).val('');
				}));
				
				field.on('blur', $.proxy(function(e){
					if (!$(this).is('[readonly]') && that._values[nr] != undefined && that.settings.hideinput)
						$(this).val(that.settings.placeholder);
				}));				
				
				field.on('input', $.proxy(function(e){
					
					// validate input pattern
					var pattern = new RegExp($(this).prop('pattern'));
					if (!$(this).val().match(pattern)) {
		
						$(this)
							.val('')
							.addClass('invalid');
							
						// fire error callback
						that.settings.invalid($(this), nr);

						e.preventDefault();
						e.stopPropagation();						
						
						return;
					}
					else
					{
						$(this).removeClass('invalid');
					}
					
					// fire input callback
					that.settings.input(e, $(this), nr);
					
					// store value
					that._values[nr] = $(this).val();
					
					if (that.settings.hideinput)
						$(this).val(that.settings.placeholder);

					// when it's not the last field
					if (nr < (that.settings.fields-1))
					{
						// make next field editable
						that._getField(nr + 1).removeProp('readonly');
						
						// set focus to the next field
						that.focus(nr + 1);
					}
					// and when you're done
					else
					{
						var pin = that._values.join('');
						
						// reset the plugin
						if (that.settings.reset)
							that.reset();
						
						// fire complete callback
						that.settings.complete(pin);
					}
					
				}));	

				field.on('keydown', $.proxy(function(e){
					
					// fire keydown callback
					that.settings.keydown(e, $(this), nr);
					
					// when user goes back
					if ((e.keyCode == 37 || e.keyCode == 8) && nr > 0) // arrow back, backspace
					{
						that.resetField(nr);

						// set focus to previous input
						that.focus(nr-1);
						
						e.preventDefault();
						e.stopPropagation(); 						
					}
					
				}));
			},
			
			// get the id for a given input number 
			_getFieldId : function (nr)
			{
				return this.element.id + '_' + this._name + '_' + nr;
			},
			
			// get the input field object
			_getField : function(nr)
			{
				return $('#' + this._getFieldId(nr));
			},
			
			// focus on the input field object
			focus : function(nr)
			{
				this.enableField(nr);	// make sure its enabled
				this._getField(nr).focus();
			},
			
			// reset the saved value and input fields
			reset : function()
			{
				this._values = new Array(this.settings.fields);
				
				this._container.children('input').each(function(index){

					$(this).val('');

					if (index > 0)
						$(this)
							.prop('readonly', true)
							.removeClass('invalid');
				});
		
				// focus on first field
				if (this.settings.autofocus)
					this.focus(0);
			},
			
			// reset a single field
			resetField : function(nr)
			{
				this._values[nr] = '';
				this._getField(nr)
					.val('')
					.prop('readonly',true)
					.removeClass('invalid');
			},
			
			// disable all fields
			disable : function()
			{
				
				//console.log('disable all fields');
				this._container.children('input').each(function(index){
					$(this).prop('readonly', true);
				});
			},
			
			// disable specified field
			disableField : function(nr)
			{
				this._getField(nr).prop('readonly', true);
				
			},
			
			// enable all fields
			enable : function()
			{
				this._container.children('input').each(function(index){
					$(this).prop('readonly', false);
				});				
				
			},
			
			// enable specified field
			enableField : function(nr)
			{
				this._getField(nr).prop('readonly', false);
				
			}			
		});

		// A really lightweight plugin wrapper around the constructor,
		$.fn[pluginName] = function (options) {
		  var plugin;
		  this.each(function() {
			plugin = $.data(this, 'plugin_' + pluginName);
			if (!plugin) {
			  plugin = new Plugin(this, options);
			  $.data(this, 'plugin_' + pluginName, plugin);
			}
		  });
		  return plugin;
		};

		
} )( jQuery, window, document );



function callGetAjax(placeHolder, urlAction, model) {
    //console.log(window.async);
    debugger;
    var ctr = $("#" + placeHolder);
    if (placeHolder == "goal_assetsInspection") {
        var loadingClass = "crxInnerloader";
    }
    else {
        var loadingClass = "asyncloader_none";
    }
    
    ctr.addClass(loadingClass);

    $.ajax({
        url: urlAction,
        type: "GET",
        global: false,
        data: model,
        success: function (data) {
            ctr.empty();
            ctr.html(data);
            ctr.removeClass(loadingClass);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ctr.removeClass(loadingClass);
            ctr.append("<h3>We are unable to process your request.</h3>")
        }
    });
}
$(() => {
    $(".chatbox-open").click(() => {
        debugger;
        $(".chatbox-popup-content-data").fadeOut();
        $(".chatbox-popup-content-main").fadeIn();
        $(".chatbox-open").fadeOut();
        $(".chatbox-popup").fadeIn();
        $(".more-link").html("Suggest a Tip for users or CRx improvement");
    });

    $(".chatbox-popup-close").click(() => {
        $(".chatbox-open").fadeIn();
        $(".chatbox-popup").fadeOut();
    });

    $(".chatbox-panel-close").click(() => {
        $(".chatbox-popup, .chatbox-close").fadeOut();
    });

    $(".chatbox-maximize").click(() => {
        $(".chatbox-popup, .chatbox-open, .chatbox-close").fadeOut();
        $(".chatbox-panel").fadeIn();
        $(".chatbox-panel").css({ display: "flex" });
    });


    $(".chatbox-minimize").click(() => {
        $(".chatbox-panel").fadeOut();
        $(".chatbox-popup, .chatbox-open, .chatbox-close").fadeIn();
    });

    $(".chatbox-panel-close").click(() => {
        $(".chatbox-panel").fadeOut();
        $(".chatbox-open").fadeIn();
    });


    $("#btn_user").click(() => {
        $(".btn_user").fadeIn();
        $(".btn_ImprovementSuggestion").fadeOut();
    });

    $("#btn_ImprovementSuggestion").click(() => {
        $(".btn_ImprovementSuggestion").fadeIn();
        $(".btn_user").fadeOut();
    });

    $(".more-link").click(() => {
        //debugger;
        $(".more-link").toggleClass('toggleActive');
        if ($(".more-link").hasClass('toggleActive')) {
            $(".more-link").html("Back to help");
            $(".chatbox-popup-content-data").fadeIn();
            $(".chatbox-popup-content-main").fadeOut();
        } else {
            $(".more-link").html("Suggest a Tip for users or CRx improvement");
            $(".chatbox-popup-content-data").fadeOut();
            $(".chatbox-popup-content-main").fadeIn();
        }
    });



});

$(".tips-suggestionbtnBox .comm-button").click(function () {
    if ($(this).hasClass("current")) {
        $(this).removeClass("current");
    } else {
        $(".tips-suggestionbtnBox .comm-button.current").removeClass("current");
        $(this).addClass("current");
    }
});

$('body').on('click', '#tipsubmitButton', function () {
    var controllername = $("#helpPageControllerName").val();
    var pageName = $("#helpPageActionName").val();
    var description = $('#user_tips').val();
    if (description != "") {
        $.get(CRxUrls.Tips_SaveTip + '?description=' + description + '&pagename=' + controllername + "_" + pageName + '&mode=tip', function (data) {
            if (data != null) {
                $("#user_tips").val("");
                successAlert("Thank You For Sharing Your Feedback");
            }
        });
    } else {
        successAlert('Please Enter any Tip First!');
    }
});

$('body').on('click', '#Improvement_btn', function () {
    var controllername = $("#helpPageControllerName").val();
    var pageName = $("#helpPageActionName").val();
    var description = $("#user_suggestions").val();
    if (description != "") {
        $.get(CRxUrls.Tips_SaveTip + '?description=' + description + '&pagename=' + controllername + "_" + pageName + '&mode=crxImprovement', function (data) {
            if (data != null) {
                $("#user_suggestions").val("");
                successAlert("Thank You For Sharing Your Feedback");
            }
        });
    } else {
        successAlert('Please Enter any Suggestion First!');
    }
});