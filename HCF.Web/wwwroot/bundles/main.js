/*
 * bootstrap-tagsinput v0.8.0
 * 
 */

(function ($) {
  "use strict";

  var defaultOptions = {
    tagClass: function(item) {
      return 'label label-info';
    },
    focusClass: 'focus',
    itemValue: function(item) {
      return item ? item.toString() : item;
    },
    itemText: function(item) {
      return this.itemValue(item);
    },
    itemTitle: function(item) {
      return null;
    },
    freeInput: true,
    addOnBlur: true,
    maxTags: undefined,
    maxChars: undefined,
    confirmKeys: [13, 44],
    delimiter: ',',
    delimiterRegex: null,
    cancelConfirmKeysOnEmpty: false,
    onTagExists: function(item, $tag) {
      $tag.hide().fadeIn();
    },
    trimValue: false,
    allowDuplicates: false,
    triggerChange: true
  };

  /**
   * Constructor function
   */
  function TagsInput(element, options) {
    this.isInit = true;
    this.itemsArray = [];

    this.$element = $(element);
    this.$element.hide();

    this.isSelect = (element.tagName === 'SELECT');
    this.multiple = (this.isSelect && element.hasAttribute('multiple'));
    this.objectItems = options && options.itemValue;
    this.placeholderText = element.hasAttribute('placeholder') ? this.$element.attr('placeholder') : '';
    this.inputSize = Math.max(1, this.placeholderText.length);

    this.$container = $('<div class="bootstrap-tagsinput"></div>');
    this.$input = $('<input type="text" placeholder="' + this.placeholderText + '"/>').appendTo(this.$container);

    this.$element.before(this.$container);

    this.build(options);
    this.isInit = false;
  }

  TagsInput.prototype = {
    constructor: TagsInput,

    /**
     * Adds the given item as a new tag. Pass true to dontPushVal to prevent
     * updating the elements val()
     */
    add: function(item, dontPushVal, options) {
      var self = this;

      if (self.options.maxTags && self.itemsArray.length >= self.options.maxTags)
        return;

      // Ignore falsey values, except false
      if (item !== false && !item)
        return;

      // Trim value
      if (typeof item === "string" && self.options.trimValue) {
        item = $.trim(item);
      }

      // Throw an error when trying to add an object while the itemValue option was not set
      if (typeof item === "object" && !self.objectItems)
        throw("Can't add objects when itemValue option is not set");

      // Ignore strings only containg whitespace
      if (item.toString().match(/^\s*$/))
        return;

      // If SELECT but not multiple, remove current tag
      if (self.isSelect && !self.multiple && self.itemsArray.length > 0)
        self.remove(self.itemsArray[0]);

      if (typeof item === "string" && this.$element[0].tagName === 'INPUT') {
        var delimiter = (self.options.delimiterRegex) ? self.options.delimiterRegex : self.options.delimiter;
        var items = item.split(delimiter);
        if (items.length > 1) {
          for (var i = 0; i < items.length; i++) {
            this.add(items[i], true);
          }

          if (!dontPushVal)
            self.pushVal(self.options.triggerChange);
          return;
        }
      }

      var itemValue = self.options.itemValue(item),
          itemText = self.options.itemText(item),
          tagClass = self.options.tagClass(item),
          itemTitle = self.options.itemTitle(item);

      // Ignore items allready added
      var existing = $.grep(self.itemsArray, function(item) { return self.options.itemValue(item) === itemValue; } )[0];
      if (existing && !self.options.allowDuplicates) {
        // Invoke onTagExists
        if (self.options.onTagExists) {
          var $existingTag = $(".tag", self.$container).filter(function() { return $(this).data("item") === existing; });
          self.options.onTagExists(item, $existingTag);
        }
        return;
      }

      // if length greater than limit
      if (self.items().toString().length + item.length + 1 > self.options.maxInputLength)
        return;

      // raise beforeItemAdd arg
      var beforeItemAddEvent = $.Event('beforeItemAdd', { item: item, cancel: false, options: options});
      self.$element.trigger(beforeItemAddEvent);
      if (beforeItemAddEvent.cancel)
        return;

      // register item in internal array and map
      self.itemsArray.push(item);

      // add a tag element

      var $tag = $('<span class="tag ' + htmlEncode(tagClass) + (itemTitle !== null ? ('" title="' + itemTitle) : '') + '">' + htmlEncode(itemText) + '<span data-role="remove"></span></span>');
      $tag.data('item', item);
      self.findInputWrapper().before($tag);
      $tag.after(' ');

      // Check to see if the tag exists in its raw or uri-encoded form
      var optionExists = (
        $('option[value="' + encodeURIComponent(itemValue) + '"]', self.$element).length ||
        $('option[value="' + htmlEncode(itemValue) + '"]', self.$element).length
      );

      // add <option /> if item represents a value not present in one of the <select />'s options
      if (self.isSelect && !optionExists) {
        var $option = $('<option selected>' + htmlEncode(itemText) + '</option>');
        $option.data('item', item);
        $option.attr('value', itemValue);
        self.$element.append($option);
      }

      if (!dontPushVal)
        self.pushVal(self.options.triggerChange);

      // Add class when reached maxTags
      if (self.options.maxTags === self.itemsArray.length || self.items().toString().length === self.options.maxInputLength)
        self.$container.addClass('bootstrap-tagsinput-max');

      // If using typeahead, once the tag has been added, clear the typeahead value so it does not stick around in the input.
      if ($('.typeahead, .twitter-typeahead', self.$container).length) {
        self.$input.typeahead('val', '');
      }

      if (this.isInit) {
        self.$element.trigger($.Event('itemAddedOnInit', { item: item, options: options }));
      } else {
        self.$element.trigger($.Event('itemAdded', { item: item, options: options }));
      }
    },

    /**
     * Removes the given item. Pass true to dontPushVal to prevent updating the
     * elements val()
     */
    remove: function(item, dontPushVal, options) {
      var self = this;

      if (self.objectItems) {
        if (typeof item === "object")
          item = $.grep(self.itemsArray, function(other) { return self.options.itemValue(other) ==  self.options.itemValue(item); } );
        else
          item = $.grep(self.itemsArray, function(other) { return self.options.itemValue(other) ==  item; } );

        item = item[item.length-1];
      }

      if (item) {
        var beforeItemRemoveEvent = $.Event('beforeItemRemove', { item: item, cancel: false, options: options });
        self.$element.trigger(beforeItemRemoveEvent);
        if (beforeItemRemoveEvent.cancel)
          return;

        $('.tag', self.$container).filter(function() { return $(this).data('item') === item; }).remove();
        $('option', self.$element).filter(function() { return $(this).data('item') === item; }).remove();
        if($.inArray(item, self.itemsArray) !== -1)
          self.itemsArray.splice($.inArray(item, self.itemsArray), 1);
      }

      if (!dontPushVal)
        self.pushVal(self.options.triggerChange);

      // Remove class when reached maxTags
      if (self.options.maxTags > self.itemsArray.length)
        self.$container.removeClass('bootstrap-tagsinput-max');

      self.$element.trigger($.Event('itemRemoved',  { item: item, options: options }));
    },

    /**
     * Removes all items
     */
    removeAll: function() {
      var self = this;

      $('.tag', self.$container).remove();
      $('option', self.$element).remove();

      while(self.itemsArray.length > 0)
        self.itemsArray.pop();

      self.pushVal(self.options.triggerChange);
    },

    /**
     * Refreshes the tags so they match the text/value of their corresponding
     * item.
     */
    refresh: function() {
      var self = this;
      $('.tag', self.$container).each(function() {
        var $tag = $(this),
            item = $tag.data('item'),
            itemValue = self.options.itemValue(item),
            itemText = self.options.itemText(item),
            tagClass = self.options.tagClass(item);

          // Update tag's class and inner text
          $tag.attr('class', null);
          $tag.addClass('tag ' + htmlEncode(tagClass));
          $tag.contents().filter(function() {
            return this.nodeType == 3;
          })[0].nodeValue = htmlEncode(itemText);

          if (self.isSelect) {
            var option = $('option', self.$element).filter(function() { return $(this).data('item') === item; });
            option.attr('value', itemValue);
          }
      });
    },

    /**
     * Returns the items added as tags
     */
    items: function() {
      return this.itemsArray;
    },

    /**
     * Assembly value by retrieving the value of each item, and set it on the
     * element.
     */
    pushVal: function() {
      var self = this,
          val = $.map(self.items(), function(item) {
            return self.options.itemValue(item).toString();
          });

      self.$element.val(val, true);

      if (self.options.triggerChange)
        self.$element.trigger('change');
    },

    /**
     * Initializes the tags input behaviour on the element
     */
    build: function(options) {
      var self = this;

      self.options = $.extend({}, defaultOptions, options);
      // When itemValue is set, freeInput should always be false
      if (self.objectItems)
        self.options.freeInput = false;

      makeOptionItemFunction(self.options, 'itemValue');
      makeOptionItemFunction(self.options, 'itemText');
      makeOptionFunction(self.options, 'tagClass');

      // Typeahead Bootstrap version 2.3.2
      if (self.options.typeahead) {
        var typeahead = self.options.typeahead || {};

        makeOptionFunction(typeahead, 'source');

        self.$input.typeahead($.extend({}, typeahead, {
          source: function (query, process) {
            function processItems(items) {
              var texts = [];

              for (var i = 0; i < items.length; i++) {
                var text = self.options.itemText(items[i]);
                map[text] = items[i];
                texts.push(text);
              }
              process(texts);
            }

            this.map = {};
            var map = this.map,
                data = typeahead.source(query);

            if ($.isFunction(data.success)) {
              // support for Angular callbacks
              data.success(processItems);
            } else if ($.isFunction(data.then)) {
              // support for Angular promises
              data.then(processItems);
            } else {
              // support for functions and jquery promises
              $.when(data)
               .then(processItems);
            }
          },
          updater: function (text) {
            self.add(this.map[text]);
            return this.map[text];
          },
          matcher: function (text) {
            return (text.toLowerCase().indexOf(this.query.trim().toLowerCase()) !== -1);
          },
          sorter: function (texts) {
            return texts.sort();
          },
          highlighter: function (text) {
            var regex = new RegExp( '(' + this.query + ')', 'gi' );
            return text.replace( regex, "<strong>$1</strong>" );
          }
        }));
      }

      // typeahead.js
      if (self.options.typeaheadjs) {
          var typeaheadConfig = null;
          var typeaheadDatasets = {};

          // Determine if main configurations were passed or simply a dataset
          var typeaheadjs = self.options.typeaheadjs;
          if ($.isArray(typeaheadjs)) {
            typeaheadConfig = typeaheadjs[0];
            typeaheadDatasets = typeaheadjs[1];
          } else {
            typeaheadDatasets = typeaheadjs;
          }

          self.$input.typeahead(typeaheadConfig, typeaheadDatasets).on('typeahead:selected', $.proxy(function (obj, datum) {
            if (typeaheadDatasets.valueKey)
              self.add(datum[typeaheadDatasets.valueKey]);
            else
              self.add(datum);
            self.$input.typeahead('val', '');
          }, self));
      }

      self.$container.on('click', $.proxy(function(event) {
        if (! self.$element.attr('disabled')) {
          self.$input.removeAttr('disabled');
        }
        self.$input.focus();
      }, self));

        if (self.options.addOnBlur && self.options.freeInput) {
          self.$input.on('focusout', $.proxy(function(event) {
              // HACK: only process on focusout when no typeahead opened, to
              //       avoid adding the typeahead text as tag
              if ($('.typeahead, .twitter-typeahead', self.$container).length === 0) {
                self.add(self.$input.val());
                self.$input.val('');
              }
          }, self));
        }

      // Toggle the 'focus' css class on the container when it has focus
      self.$container.on({
        focusin: function() {
          self.$container.addClass(self.options.focusClass);
        },
        focusout: function() {
          self.$container.removeClass(self.options.focusClass);
        },
      });

      self.$container.on('keydown', 'input', $.proxy(function(event) {
        var $input = $(event.target),
            $inputWrapper = self.findInputWrapper();

        if (self.$element.attr('disabled')) {
          self.$input.attr('disabled', 'disabled');
          return;
        }

        switch (event.which) {
          // BACKSPACE
          case 8:
            if (doGetCaretPosition($input[0]) === 0) {
              var prev = $inputWrapper.prev();
              if (prev.length) {
                self.remove(prev.data('item'));
              }
            }
            break;

          // DELETE
          case 46:
            if (doGetCaretPosition($input[0]) === 0) {
              var next = $inputWrapper.next();
              if (next.length) {
                self.remove(next.data('item'));
              }
            }
            break;

          // LEFT ARROW
          case 37:
            // Try to move the input before the previous tag
            var $prevTag = $inputWrapper.prev();
            if ($input.val().length === 0 && $prevTag[0]) {
              $prevTag.before($inputWrapper);
              $input.focus();
            }
            break;
          // RIGHT ARROW
          case 39:
            // Try to move the input after the next tag
            var $nextTag = $inputWrapper.next();
            if ($input.val().length === 0 && $nextTag[0]) {
              $nextTag.after($inputWrapper);
              $input.focus();
            }
            break;
         default:
             // ignore
         }

        // Reset internal input's size
        var textLength = $input.val().length,
            wordSpace = Math.ceil(textLength / 5),
            size = textLength + wordSpace + 1;
        $input.attr('size', Math.max(this.inputSize, $input.val().length));
      }, self));

      self.$container.on('keypress', 'input', $.proxy(function(event) {
         var $input = $(event.target);

         if (self.$element.attr('disabled')) {
            self.$input.attr('disabled', 'disabled');
            return;
         }

         var text = $input.val(),
         maxLengthReached = self.options.maxChars && text.length >= self.options.maxChars;
         if (self.options.freeInput && (keyCombinationInList(event, self.options.confirmKeys) || maxLengthReached)) {
            // Only attempt to add a tag if there is data in the field
            if (text.length !== 0) {
               self.add(maxLengthReached ? text.substr(0, self.options.maxChars) : text);
               $input.val('');
            }

            // If the field is empty, let the event triggered fire as usual
            if (self.options.cancelConfirmKeysOnEmpty === false) {
                event.preventDefault();
            }
         }

         // Reset internal input's size
         var textLength = $input.val().length,
            wordSpace = Math.ceil(textLength / 5),
            size = textLength + wordSpace + 1;
         $input.attr('size', Math.max(this.inputSize, $input.val().length));
      }, self));

      // Remove icon clicked
      self.$container.on('click', '[data-role=remove]', $.proxy(function(event) {
        if (self.$element.attr('disabled')) {
          return;
        }
        self.remove($(event.target).closest('.tag').data('item'));
      }, self));

      // Only add existing value as tags when using strings as tags
      if (self.options.itemValue === defaultOptions.itemValue) {
        if (self.$element[0].tagName === 'INPUT') {
            self.add(self.$element.val());
        } else {
          $('option', self.$element).each(function() {
            self.add($(this).attr('value'), true);
          });
        }
      }
    },

    /**
     * Removes all tagsinput behaviour and unregsiter all event handlers
     */
    destroy: function() {
      var self = this;

      // Unbind events
      self.$container.off('keypress', 'input');
      self.$container.off('click', '[role=remove]');

      self.$container.remove();
      self.$element.removeData('tagsinput');
      self.$element.show();
    },

    /**
     * Sets focus on the tagsinput
     */
    focus: function() {
      this.$input.focus();
    },

    /**
     * Returns the internal input element
     */
    input: function() {
      return this.$input;
    },

    /**
     * Returns the element which is wrapped around the internal input. This
     * is normally the $container, but typeahead.js moves the $input element.
     */
    findInputWrapper: function() {
      var elt = this.$input[0],
          container = this.$container[0];
      while(elt && elt.parentNode !== container)
        elt = elt.parentNode;

      return $(elt);
    }
  };

  /**
   * Register JQuery plugin
   */
  $.fn.tagsinput = function(arg1, arg2, arg3) {
    var results = [];

    this.each(function() {
      var tagsinput = $(this).data('tagsinput');
      // Initialize a new tags input
      if (!tagsinput) {
          tagsinput = new TagsInput(this, arg1);
          $(this).data('tagsinput', tagsinput);
          results.push(tagsinput);

          if (this.tagName === 'SELECT') {
              $('option', $(this)).attr('selected', 'selected');
          }

          // Init tags from $(this).val()
          $(this).val($(this).val());
      } else if (!arg1 && !arg2) {
          // tagsinput already exists
          // no function, trying to init
          results.push(tagsinput);
      } else if(tagsinput[arg1] !== undefined) {
          // Invoke function on existing tags input
            if(tagsinput[arg1].length === 3 && arg3 !== undefined){
               var retVal = tagsinput[arg1](arg2, null, arg3);
            }else{
               var retVal = tagsinput[arg1](arg2);
            }
          if (retVal !== undefined)
              results.push(retVal);
      }
    });

    if ( typeof arg1 == 'string') {
      // Return the results from the invoked function calls
      return results.length > 1 ? results : results[0];
    } else {
      return results;
    }
  };

  $.fn.tagsinput.Constructor = TagsInput;

  /**
   * Most options support both a string or number as well as a function as
   * option value. This function makes sure that the option with the given
   * key in the given options is wrapped in a function
   */
  function makeOptionItemFunction(options, key) {
    if (typeof options[key] !== 'function') {
      var propertyName = options[key];
      options[key] = function(item) { return item[propertyName]; };
    }
  }
  function makeOptionFunction(options, key) {
    if (typeof options[key] !== 'function') {
      var value = options[key];
      options[key] = function() { return value; };
    }
  }
  /**
   * HtmlEncodes the given value
   */
  var htmlEncodeContainer = $('<div />');
  function htmlEncode(value) {
    if (value) {
      return htmlEncodeContainer.text(value).html();
    } else {
      return '';
    }
  }

  /**
   * Returns the position of the caret in the given input field
   * http://flightschool.acylt.com/devnotes/caret-position-woes/
   */
  function doGetCaretPosition(oField) {
    var iCaretPos = 0;
    if (document.selection) {
      oField.focus ();
      var oSel = document.selection.createRange();
      oSel.moveStart ('character', -oField.value.length);
      iCaretPos = oSel.text.length;
    } else if (oField.selectionStart || oField.selectionStart == '0') {
      iCaretPos = oField.selectionStart;
    }
    return (iCaretPos);
  }

  /**
    * Returns boolean indicates whether user has pressed an expected key combination.
    * @param object keyPressEvent: JavaScript event object, refer
    *     http://www.w3.org/TR/2003/WD-DOM-Level-3-Events-20030331/ecma-script-binding.html
    * @param object lookupList: expected key combinations, as in:
    *     [13, {which: 188, shiftKey: true}]
    */
  function keyCombinationInList(keyPressEvent, lookupList) {
      var found = false;
      $.each(lookupList, function (index, keyCombination) {
          if (typeof (keyCombination) === 'number' && keyPressEvent.which === keyCombination) {
              found = true;
              return false;
          }

          if (keyPressEvent.which === keyCombination.which) {
              var alt = !keyCombination.hasOwnProperty('altKey') || keyPressEvent.altKey === keyCombination.altKey,
                  shift = !keyCombination.hasOwnProperty('shiftKey') || keyPressEvent.shiftKey === keyCombination.shiftKey,
                  ctrl = !keyCombination.hasOwnProperty('ctrlKey') || keyPressEvent.ctrlKey === keyCombination.ctrlKey;
              if (alt && shift && ctrl) {
                  found = true;
                  return false;
              }
          }
      });

      return found;
  }

  /**
   * Initialize tagsinput behaviour on inputs and selects which have
   * data-role=tagsinput
   */
  $(function() {
    $("input[data-role=tagsinput], select[multiple][data-role=tagsinput]").tagsinput();
  });
})(window.jQuery);

/*
* jQuery listnav plugin
*
* Add a slick "letter-based" navigation bar to all of your lists.
* Click a letter to quickly filter the list to items that match that letter.
*
* Dual licensed under the MIT and GPL licenses:
*   http://www.opensource.org/licenses/mit-license.php
*   http://www.gnu.org/licenses/gpl.html
*
* Version 2.4.9 (11/03/14)
* Author: Eric Steinborn
* Compatibility: jQuery 1.3.x through 1.11.0 and jQuery 2
* Browser Compatibility: IE6+, FF, Chrome & Safari
* CSS is a little wonky in IE6, just set your listnav class to be 100% width and it works fine.
*
*/
(function ($) {

    $.fn.listnav = function (options) {

        var opts = $.extend({}, $.fn.listnav.defaults, options),
            letters = ['_', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '-'],
            firstClick = false,
            //detect if you are on a touch device easily.
            clickEventType=((document.ontouchstart!==null)?'click':'touchend');

        opts.prefixes = $.map(opts.prefixes, function (n) {

            return n.toLowerCase();

        });

        return this.each(function () {

            var $wrapper, $letters, $letterCount, left, width, count,
                id = this.id,
                $list = $(this),
                counts = {},
                allCount = 0, fullCount = 0,
                isAll = true,
                prevLetter = '';

            if ( !$('#' + id + '-nav').length ) {

                $('<div id="' + id + '-nav" class="listNav"/>').insertBefore($list);
                // Insert the nav if its not been inserted already (preferred method)
                // Legacy method was to add the nav yourself in HTML, I didn't like that requirement

            }

            $wrapper = $('#' + id + '-nav');
            // <ul id="myList"> for list and <div id="myList-nav"> for nav wrapper

            function init() {

                $wrapper.append(createLettersHtml());

                $letters = $('.ln-letters', $wrapper).slice(0, 1);

                if ( opts.showCounts ) {

                    $letterCount = $('.ln-letter-count', $wrapper).slice(0, 1);

                }

                addClasses();

                addNoMatchLI();

                bindHandlers();

                if (opts.flagDisabled) {

                    addDisabledClass();

                }

                // remove nav items we don't need

                if ( !opts.includeAll ) {

                    $('.all', $letters).remove();

                }
                if ( !opts.includeNums ) {

                    $('._', $letters).remove();

                }
                if ( !opts.includeOther ) {

                    $('.-', $letters).remove();

                }
                if ( opts.removeDisabled ) {

                    $('.ln-disabled', $letters).remove();

                }

                $(':last', $letters).addClass('ln-last');

                if ( $.cookie && (opts.cookieName !== null) ) {

                    var cookieLetter = $.cookie(opts.cookieName);

                    if ( cookieLetter !== null && typeof cookieLetter !== "undefined" ) {

                        opts.initLetter = cookieLetter;

                    }

                }

                // decide what to show first

                // Is there an initLetter set, if so, show that letter first
                if ( opts.initLetter !== '' ) {

                    firstClick = true;

                    // click the initLetter if there was one
                    $('.' + opts.initLetter.toLowerCase(), $letters).slice(0, 1).trigger(clickEventType);

                } else {

                    // If no init letter is set, and you included All, then show it
                    if ( opts.includeAll ) {

                        // make the All link look clicked, but don't actually click it
                        $('.all', $letters).addClass('ln-selected');

                    } else {

                        // All was not included, lets find the first letter with a count and show it
                        for ( var i = ((opts.includeNums) ? 0 : 1); i < letters.length; i++) {

                            if ( counts[letters[i]] > 0 ) {

                                firstClick = true;

                                $('.' + letters[i], $letters).slice(0, 1).trigger(clickEventType);

                                break;

                            }
                        }
                    }
                }
            }

            // position the letter count above the letter links
            function setLetterCountTop() {

                // we're going to need to subtract this from the top value of the wrapper to accomodate changes in font-size in CSS.
                var letterCountHeight = $letterCount.outerHeight();

                $letterCount.css({
                    top: $('a:first', $wrapper).slice(0, 1).position().top - letterCountHeight
                    // we're going to grab the first anchor in the list
                    // We can no longer guarantee that a specific letter will be present
                    // since adding the "removeDisabled" option

                });

            }

            // adds a class to each LI that has text content inside of it (ie, inside an <a>, a <div>, nested DOM nodes, etc)
            function addClasses() {

                var str, spl, $this,
                    firstChar = '',
                    hasPrefixes = (opts.prefixes.length > 0),
                    hasFilterSelector = (opts.filterSelector.length > 0);

                // Iterate over the list and set a class on each one and use that to filter by
                $($list).children().each(function () {

                    $this = $(this);

                    // I'm assuming you didn't choose a filterSelector, hopefully saving some cycles
                    if ( !hasFilterSelector ) {

                        //Grab the first text content of the LI, we'll use this to filter by
                        str = $.trim($this.text()).toLowerCase();

                    } else {

                        // You set a filterSelector so lets find it and use that to search by instead
                        str = $.trim($this.find(opts.filterSelector).text()).toLowerCase();

                    }

                    // This will run only if there is something to filter by, skipping over images and non-filterable content.
                    if (str !== '') {

                        // Apply the non-prefix class to LIs that have prefixed content in them
                        if (hasPrefixes) {
                            var prefixes = $.map(opts.prefixes, function(value) {
                                return value.indexOf(' ') <= 0 ? value + ' ' : value;
                            });
                            var matches = $.grep(prefixes, function(value) {
                                return str.indexOf(value) === 0;
                            });
                            if (matches.length > 0) {
                                var afterMatch = str.toLowerCase().split(matches[0])[1];
                                if(afterMatch != null) {
                                    firstChar = $.trim(afterMatch).charAt(0);
                                } else {
                                    firstChar = str.charAt(0);
                                }
                                addLetterClass(firstChar, $this, true);
                                return;
                            }
                        }
                        // Find the first letter in the LI, including prefixes
                        firstChar = str.charAt(0);

                        // Doesn't send true to function, which will ++ the All count on prefixed items
                        addLetterClass(firstChar, $this);
                    }
                });
            }

            // Add the appropriate letter class to the current element
            function addLetterClass(firstChar, $el, isPrefix) {

                if ( /\W/.test(firstChar) ) {

                    firstChar = '-'; // not A-Z, a-z or 0-9, so considered "other"

                }

                if ( !isNaN(firstChar) ) {

                    firstChar = '_'; // use '_' if the first char is a number

                }

                $el.addClass('ln-' + firstChar);

                if ( counts[firstChar] === undefined ) {

                    counts[firstChar] = 0;

                }

                counts[firstChar]++;

                if (!isPrefix) {

                    allCount++;

                }

            }

            function addDisabledClass() {

                for ( var i = 0; i < letters.length; i++ ) {

                    if ( counts[letters[i]] === undefined ) {

                        $('.' + letters[i], $letters).addClass('ln-disabled');

                    }
                }
            }

            function addNoMatchLI() {
                $list.append('<li class="ln-no-match listNavHide">' + opts.noMatchText + '</li>');
            }

            function getLetterCount(el) {
                if ($(el).hasClass('all')) {
                    if (opts.dontCount) {
                        fullCount = allCount - $list.find(opts.dontCount).length;
                    } else {
                        fullCount = allCount;
                    }
                    return fullCount;
                } else {
                    el = '.ln-' + $(el).attr('class').split(' ')[0];

                    if (opts.dontCount) {
                        count = $list.find(el).not(opts.dontCount).length;
                    } else {
                        count = $list.find(el).length;
                    }
                    return (count !== undefined) ? count : 0; // some letters may not have a count in the hash
                }
            }

            function bindHandlers() {

                if (opts.showCounts) {
                    // sets the top position of the count div in case something above it on the page has resized
                    $wrapper.mouseover(function () {
                        setLetterCountTop();
                    });

                    //shows the count above the letter
                    //
                    $('.ln-letters a', $wrapper).mouseover(function () {
                        left = $(this).position().left;
                        width = ($(this).outerWidth()) + 'px';
                        count = getLetterCount(this);

                        $letterCount.css({
                            left: left,
                            width: width
                        }).text(count).addClass("letterCountShow").removeClass("listNavHide"); // set left position and width of letter count, set count text and show it
                    }).mouseout(function () { // mouseout for each letter: hide the count
                        $letterCount.addClass("listNavHide").removeClass("letterCountShow");
                    });
                }

                // click handler for letters: shows/hides relevant LI's
                //
                $('a', $letters).bind(clickEventType, function (e) {
                    e.preventDefault();
                    var $this = $(this),
                        letter = $this.attr('class').split(' ')[0],
                        noMatches = $list.children('.ln-no-match');

                    if ( prevLetter !== letter ) {
                    // Only to run this once for each click, won't double up if they clicked the same letter
                    // Won't hinder firstRun

                        $('a.ln-selected', $letters).removeClass('ln-selected');

                        if ( letter === 'all' ) {
                            // If ALL button is clicked:

                            $list.children().addClass("listNavShow").removeClass("listNavHide"); // Show ALL

                            noMatches.addClass("listNavHide").removeClass("listNavShow"); // Hide the list item for no matches

                            isAll = true; // set this to quickly check later

                        } else {
                            // If you didn't click ALL

                            if ( isAll ) {
                                // since you clicked ALL last time:

                                $list.children().addClass("listNavHide").removeClass("listNavShow");

                                isAll = false;

                            } else if (prevLetter !== '') {

                                $list.children('.ln-' + prevLetter).addClass("listNavHide").removeClass("listNavShow");

                            }

                            var count = getLetterCount(this);

                            if (count > 0) {
                                $list.children('.ln-' + letter).addClass("listNavShow").removeClass("listNavHide");
                                noMatches.addClass("listNavHide").removeClass("listNavShow"); // in case it's showing
                            } else {
                                noMatches.addClass("listNavShow").removeClass("listNavHide");
                            }


                        }

                        prevLetter = letter;

                        if ($.cookie && (opts.cookieName !== null)) {
                            $.cookie(opts.cookieName, letter, {
                                expires: 999
                            });
                        }

                        $this.addClass('ln-selected');

                        $this.blur();

                        if (!firstClick && (opts.onClick !== null)) {

                            opts.onClick(letter);

                        } else {

                            firstClick = false; //return false;

                        }

                    } // end if prevLetter !== letter

                }); // end click()

            } // end BindHandlers()

            // creates the HTML for the letter links
            //
            function createLettersHtml() {
                var html = [];
                for (var i = 1; i < letters.length; i++) {
                    if (html.length === 0) {
                        html.push('<a class="all" href="#">'+ opts.allText + '</a><a class="_" href="#">0-9</a>');
                    }
                    html.push('<a class="' + letters[i] + '" href="#">' + ((letters[i] === '-') ? '...' : letters[i].toUpperCase()) + '</a>');
                }
                return '<div class="ln-letters">' + html.join('') + '</div>' + ((opts.showCounts) ? '<div class="ln-letter-count listNavHide">0</div>' : '');
                // Remove inline styles, replace with css class
                // Element will be repositioned when made visible
            }
            init();
        });
    };

    $.fn.listnav.defaults = {
        initLetter: '',
        includeAll: true,
        allText: 'All',
        includeOther: false,
        includeNums: true,
        flagDisabled: true,
        removeDisabled: false,
        noMatchText: 'No matching entries',
        showCounts: true,
        dontCount: '',
        cookieName: null,
        onClick: null,
        prefixes: [],
        filterSelector: ''
    };
})(jQuery);
/*!
 * ClockPicker v0.0.7 (http://weareoutman.github.io/clockpicker/)
 * Copyright 2014 Wang Shenwei.
 * Licensed under MIT (https://github.com/weareoutman/clockpicker/blob/gh-pages/LICENSE)
 */

;(function(){
	var $ = window.jQuery,
		$win = $(window),
		$doc = $(document),
		$body;

	// Can I use inline svg ?
	var svgNS = 'http://www.w3.org/2000/svg',
		svgSupported = 'SVGAngle' in window && (function(){
			var supported,
				el = document.createElement('div');
			el.innerHTML = '<svg/>';
			supported = (el.firstChild && el.firstChild.namespaceURI) == svgNS;
			el.innerHTML = '';
			return supported;
		})();

	// Can I use transition ?
	var transitionSupported = (function(){
		var style = document.createElement('div').style;
		return 'transition' in style ||
			'WebkitTransition' in style ||
			'MozTransition' in style ||
			'msTransition' in style ||
			'OTransition' in style;
	})();

	// Listen touch events in touch screen device, instead of mouse events in desktop.
	var touchSupported = 'ontouchstart' in window,
		mousedownEvent = 'mousedown' + ( touchSupported ? ' touchstart' : ''),
		mousemoveEvent = 'mousemove.clockpicker' + ( touchSupported ? ' touchmove.clockpicker' : ''),
		mouseupEvent = 'mouseup.clockpicker' + ( touchSupported ? ' touchend.clockpicker' : '');

	// Vibrate the device if supported
	var vibrate = navigator.vibrate ? 'vibrate' : navigator.webkitVibrate ? 'webkitVibrate' : null;

	function createSvgElement(name) {
		return document.createElementNS(svgNS, name);
	}

	function leadingZero(num) {
		return (num < 10 ? '0' : '') + num;
	}

	// Get a unique id
	var idCounter = 0;
	function uniqueId(prefix) {
		var id = ++idCounter + '';
		return prefix ? prefix + id : id;
	}

	// Clock size
	var dialRadius = 100,
		outerRadius = 80,
		// innerRadius = 80 on 12 hour clock
		innerRadius = 54,
		tickRadius = 13,
		diameter = dialRadius * 2,
		duration = transitionSupported ? 350 : 1;

	// Popover template
	var tpl = [
		'<div class="popover clockpicker-popover">',
			'<div class="arrow"></div>',
			'<div class="popover-title">',
				'<span class="clockpicker-span-hours text-primary"></span>',
				' : ',
				'<span class="clockpicker-span-minutes"></span>',
                ' ',
				'<span class="clockpicker-span-am-pm"></span>',
			'</div>',
			'<div class="popover-content">',
				'<div class="clockpicker-plate">',
					'<div class="clockpicker-canvas"></div>',
					'<div class="clockpicker-dial clockpicker-hours"></div>',
					'<div class="clockpicker-dial clockpicker-minutes clockpicker-dial-out"></div>',
				'</div>',
				'<span class="clockpicker-am-pm-block">',
				'</span>',
			'</div>',
		'</div>'
	].join('');

	// ClockPicker
	function ClockPicker(element, options) {
		var popover = $(tpl),
			plate = popover.find('.clockpicker-plate'),
			hoursView = popover.find('.clockpicker-hours'),
			minutesView = popover.find('.clockpicker-minutes'),
			amPmBlock = popover.find('.clockpicker-am-pm-block'),
			isInput = element.prop('tagName') === 'INPUT',
			input = isInput ? element : element.find('input'),
			addon = element.find('.input-group-addon'),
			self = this,
			timer;

		this.id = uniqueId('cp');
		this.element = element;
		this.options = options;
		this.isAppended = false;
		this.isShown = false;
		this.currentView = 'hours';
		this.isInput = isInput;
		this.input = input;
		this.addon = addon;
		this.popover = popover;
		this.plate = plate;
		this.hoursView = hoursView;
		this.minutesView = minutesView;
		this.amPmBlock = amPmBlock;
		this.spanHours = popover.find('.clockpicker-span-hours');
		this.spanMinutes = popover.find('.clockpicker-span-minutes');
		this.spanAmPm = popover.find('.clockpicker-span-am-pm');
		this.amOrPm = "PM";
		
		// Setup for for 12 hour clock if option is selected
		if (options.twelvehour) {
			
			var  amPmButtonsTemplate = ['<div class="clockpicker-am-pm-block">',
				'<button type="button" class="btn btn-sm btn-default clockpicker-button clockpicker-am-button">',
				'AM</button>',
				'<button type="button" class="btn btn-sm btn-default clockpicker-button clockpicker-pm-button">',
				'PM</button>',
				'</div>'].join('');
			
			var amPmButtons = $(amPmButtonsTemplate);
			//amPmButtons.appendTo(plate);
			
			////Not working b/c they are not shown when this runs
			//$('clockpicker-am-button')
			//    .on("click", function() {
			//        self.amOrPm = "AM";
			//        $('.clockpicker-span-am-pm').empty().append('AM');
			//    });
			//    
			//$('clockpicker-pm-button')
			//    .on("click", function() {
			//         self.amOrPm = "PM";
			//        $('.clockpicker-span-am-pm').empty().append('PM');
			//    });
	
			$('<button type="button" class="btn btn-sm btn-default clockpicker-button am-button">' + "AM" + '</button>')
				.on("click", function() {
					self.amOrPm = "AM";
					$('.clockpicker-span-am-pm').empty().append('AM');
				}).appendTo(this.amPmBlock);
				
				
			$('<button type="button" class="btn btn-sm btn-default clockpicker-button pm-button">' + "PM" + '</button>')
				.on("click", function() {
					self.amOrPm = 'PM';
					$('.clockpicker-span-am-pm').empty().append('PM');
				}).appendTo(this.amPmBlock);
				
		}
		
		if (! options.autoclose) {
			// If autoclose is not setted, append a button
			$('<button type="button" class="btn btn-sm btn-default btn-block clockpicker-button">' + options.donetext + '</button>')
				.click($.proxy(this.done, this))
				.appendTo(popover);
		}

		// Placement and arrow align - make sure they make sense.
		if ((options.placement === 'top' || options.placement === 'bottom') && (options.align === 'top' || options.align === 'bottom')) options.align = 'left';
		if ((options.placement === 'left' || options.placement === 'right') && (options.align === 'left' || options.align === 'right')) options.align = 'top';

		popover.addClass(options.placement);
		popover.addClass('clockpicker-align-' + options.align);

		this.spanHours.click($.proxy(this.toggleView, this, 'hours'));
		this.spanMinutes.click($.proxy(this.toggleView, this, 'minutes'));

		// Show or toggle
		input.on('focus.clockpicker click.clockpicker', $.proxy(this.show, this));
		addon.on('click.clockpicker', $.proxy(this.toggle, this));

		// Build ticks
		var tickTpl = $('<div class="clockpicker-tick"></div>'),
			i, tick, radian, radius;

		// Hours view
		if (options.twelvehour) {
			for (i = 1; i < 13; i += 1) {
				tick = tickTpl.clone();
				radian = i / 6 * Math.PI;
				radius = outerRadius;
				tick.css('font-size', '120%');
				tick.css({
					left: dialRadius + Math.sin(radian) * radius - tickRadius,
					top: dialRadius - Math.cos(radian) * radius - tickRadius
				});
				tick.html(i === 0 ? '00' : i);
				hoursView.append(tick);
				tick.on(mousedownEvent, mousedown);
			}
		} else {
			for (i = 0; i < 24; i += 1) {
				tick = tickTpl.clone();
				radian = i / 6 * Math.PI;
				var inner = i > 0 && i < 13;
				radius = inner ? innerRadius : outerRadius;
				tick.css({
					left: dialRadius + Math.sin(radian) * radius - tickRadius,
					top: dialRadius - Math.cos(radian) * radius - tickRadius
				});
				if (inner) {
					tick.css('font-size', '120%');
				}
				tick.html(i === 0 ? '00' : i);
				hoursView.append(tick);
				tick.on(mousedownEvent, mousedown);
			}
		}

		// Minutes view
		for (i = 0; i < 60; i += 5) {
			tick = tickTpl.clone();
			radian = i / 30 * Math.PI;
			tick.css({
				left: dialRadius + Math.sin(radian) * outerRadius - tickRadius,
				top: dialRadius - Math.cos(radian) * outerRadius - tickRadius
			});
			tick.css('font-size', '120%');
			tick.html(leadingZero(i));
			minutesView.append(tick);
			tick.on(mousedownEvent, mousedown);
		}

		// Clicking on minutes view space
		plate.on(mousedownEvent, function(e){
			if ($(e.target).closest('.clockpicker-tick').length === 0) {
				mousedown(e, true);
			}
		});

		// Mousedown or touchstart
		function mousedown(e, space) {
			var offset = plate.offset(),
				isTouch = /^touch/.test(e.type),
				x0 = offset.left + dialRadius,
				y0 = offset.top + dialRadius,
				dx = (isTouch ? e.originalEvent.touches[0] : e).pageX - x0,
				dy = (isTouch ? e.originalEvent.touches[0] : e).pageY - y0,
				z = Math.sqrt(dx * dx + dy * dy),
				moved = false;

			// When clicking on minutes view space, check the mouse position
			if (space && (z < outerRadius - tickRadius || z > outerRadius + tickRadius)) {
				return;
			}
			e.preventDefault();

			// Set cursor style of body after 200ms
			var movingTimer = setTimeout(function(){
				$body.addClass('clockpicker-moving');
			}, 200);

			// Place the canvas to top
			if (svgSupported) {
				plate.append(self.canvas);
			}

			// Clock
			self.setHand(dx, dy, ! space, true);

			// Mousemove on document
			$doc.off(mousemoveEvent).on(mousemoveEvent, function(e){
				e.preventDefault();
				var isTouch = /^touch/.test(e.type),
					x = (isTouch ? e.originalEvent.touches[0] : e).pageX - x0,
					y = (isTouch ? e.originalEvent.touches[0] : e).pageY - y0;
				if (! moved && x === dx && y === dy) {
					// Clicking in chrome on windows will trigger a mousemove event
					return;
				}
				moved = true;
				self.setHand(x, y, false, true);
			});

			// Mouseup on document
			$doc.off(mouseupEvent).on(mouseupEvent, function(e){
				$doc.off(mouseupEvent);
				e.preventDefault();
				var isTouch = /^touch/.test(e.type),
					x = (isTouch ? e.originalEvent.changedTouches[0] : e).pageX - x0,
					y = (isTouch ? e.originalEvent.changedTouches[0] : e).pageY - y0;
				if ((space || moved) && x === dx && y === dy) {
					self.setHand(x, y);
				}
				if (self.currentView === 'hours') {
					self.toggleView('minutes', duration / 2);
				} else {
					if (options.autoclose) {
						self.minutesView.addClass('clockpicker-dial-out');
						setTimeout(function(){
							self.done();
						}, duration / 2);
					}
				}
				plate.prepend(canvas);

				// Reset cursor style of body
				clearTimeout(movingTimer);
				$body.removeClass('clockpicker-moving');

				// Unbind mousemove event
				$doc.off(mousemoveEvent);
			});
		}

		if (svgSupported) {
			// Draw clock hands and others
			var canvas = popover.find('.clockpicker-canvas'),
				svg = createSvgElement('svg');
			svg.setAttribute('class', 'clockpicker-svg');
			svg.setAttribute('width', diameter);
			svg.setAttribute('height', diameter);
			var g = createSvgElement('g');
			g.setAttribute('transform', 'translate(' + dialRadius + ',' + dialRadius + ')');
			var bearing = createSvgElement('circle');
			bearing.setAttribute('class', 'clockpicker-canvas-bearing');
			bearing.setAttribute('cx', 0);
			bearing.setAttribute('cy', 0);
			bearing.setAttribute('r', 2);
			var hand = createSvgElement('line');
			hand.setAttribute('x1', 0);
			hand.setAttribute('y1', 0);
			var bg = createSvgElement('circle');
			bg.setAttribute('class', 'clockpicker-canvas-bg');
			bg.setAttribute('r', tickRadius);
			var fg = createSvgElement('circle');
			fg.setAttribute('class', 'clockpicker-canvas-fg');
			fg.setAttribute('r', 3.5);
			g.appendChild(hand);
			g.appendChild(bg);
			g.appendChild(fg);
			g.appendChild(bearing);
			svg.appendChild(g);
			canvas.append(svg);

			this.hand = hand;
			this.bg = bg;
			this.fg = fg;
			this.bearing = bearing;
			this.g = g;
			this.canvas = canvas;
		}

		raiseCallback(this.options.init);
	}

	function raiseCallback(callbackFunction) {
		if (callbackFunction && typeof callbackFunction === "function") {
			callbackFunction();
		}
	}

	// Default options
	ClockPicker.DEFAULTS = {
		'default': '',       // default time, 'now' or '13:14' e.g.
		fromnow: 0,          // set default time to * milliseconds from now (using with default = 'now')
		placement: 'bottom', // clock popover placement
		align: 'left',       // popover arrow align
		donetext: '??????',    // done button text
		autoclose: false,    // auto close when minute is selected
		twelvehour: false, // change to 12 hour AM/PM clock from 24 hour
		vibrate: true        // vibrate the device when dragging clock hand
	};

	// Show or hide popover
	ClockPicker.prototype.toggle = function(){
		this[this.isShown ? 'hide' : 'show']();
	};

	// Set popover position
	ClockPicker.prototype.locate = function(){
		var element = this.element,
			popover = this.popover,
			offset = element.offset(),
			width = element.outerWidth(),
			height = element.outerHeight(),
			placement = this.options.placement,
			align = this.options.align,
			styles = {},
			self = this;

		popover.show();

		// Place the popover
		switch (placement) {
			case 'bottom':
				styles.top = offset.top + height;
				break;
			case 'right':
				styles.left = offset.left + width;
				break;
			case 'top':
				styles.top = offset.top - popover.outerHeight();
				break;
			case 'left':
				styles.left = offset.left - popover.outerWidth();
				break;
		}

		// Align the popover arrow
		switch (align) {
			case 'left':
				styles.left = offset.left;
				break;
			case 'right':
				styles.left = offset.left + width - popover.outerWidth();
				break;
			case 'top':
				styles.top = offset.top;
				break;
			case 'bottom':
				styles.top = offset.top + height - popover.outerHeight();
				break;
		}

		popover.css(styles);
	};

	// Show popover
	ClockPicker.prototype.show = function(e){
		// Not show again
		if (this.isShown) {
			return;
		}

		raiseCallback(this.options.beforeShow);

		var self = this;

		// Initialize
		if (! this.isAppended) {
			// Append popover to body
			$body = $(document.body).append(this.popover);

			// Reset position when resize
			$win.on('resize.clockpicker' + this.id, function(){
				if (self.isShown) {
					self.locate();
				}
			});

			this.isAppended = true;
		}

		// Get the time
		var value = ((this.input.prop('value') || this.options['default'] || '') + '').split(':');
		if (value[0] === 'now') {
			var now = new Date(+ new Date() + this.options.fromnow);
			value = [
				now.getHours(),
				now.getMinutes()
			];
		}
		this.hours = + value[0] || 0;
		this.minutes = + value[1] || 0;
		this.spanHours.html(leadingZero(this.hours));
		this.spanMinutes.html(leadingZero(this.minutes));

		// Toggle to hours view
		this.toggleView('hours');

		// Set position
		this.locate();

		this.isShown = true;

		// Hide when clicking or tabbing on any element except the clock, input and addon
		$doc.on('click.clockpicker.' + this.id + ' focusin.clockpicker.' + this.id, function(e){
			var target = $(e.target);
			if (target.closest(self.popover).length === 0 &&
					target.closest(self.addon).length === 0 &&
					target.closest(self.input).length === 0) {
				self.hide();
			}
		});

		// Hide when ESC is pressed
		$doc.on('keyup.clockpicker.' + this.id, function(e){
			if (e.keyCode === 27) {
				self.hide();
			}
		});

		raiseCallback(this.options.afterShow);
	};

	// Hide popover
	ClockPicker.prototype.hide = function(){
		raiseCallback(this.options.beforeHide);

		this.isShown = false;

		// Unbinding events on document
		$doc.off('click.clockpicker.' + this.id + ' focusin.clockpicker.' + this.id);
		$doc.off('keyup.clockpicker.' + this.id);

		this.popover.hide();

		raiseCallback(this.options.afterHide);
	};

	// Toggle to hours or minutes view
	ClockPicker.prototype.toggleView = function(view, delay){
		var raiseAfterHourSelect = false;
		if (view === 'minutes' && $(this.hoursView).css("visibility") === "visible") {
			raiseCallback(this.options.beforeHourSelect);
			raiseAfterHourSelect = true;
		}
		var isHours = view === 'hours',
			nextView = isHours ? this.hoursView : this.minutesView,
			hideView = isHours ? this.minutesView : this.hoursView;

		this.currentView = view;

		this.spanHours.toggleClass('text-primary', isHours);
		this.spanMinutes.toggleClass('text-primary', ! isHours);

		// Let's make transitions
		hideView.addClass('clockpicker-dial-out');
		nextView.css('visibility', 'visible').removeClass('clockpicker-dial-out');

		// Reset clock hand
		this.resetClock(delay);

		// After transitions ended
		clearTimeout(this.toggleViewTimer);
		this.toggleViewTimer = setTimeout(function(){
			hideView.css('visibility', 'hidden');
		}, duration);

		if (raiseAfterHourSelect) {
			raiseCallback(this.options.afterHourSelect);
		}
	};

	// Reset clock hand
	ClockPicker.prototype.resetClock = function(delay){
		var view = this.currentView,
			value = this[view],
			isHours = view === 'hours',
			unit = Math.PI / (isHours ? 6 : 30),
			radian = value * unit,
			radius = isHours && value > 0 && value < 13 ? innerRadius : outerRadius,
			x = Math.sin(radian) * radius,
			y = - Math.cos(radian) * radius,
			self = this;
		if (svgSupported && delay) {
			self.canvas.addClass('clockpicker-canvas-out');
			setTimeout(function(){
				self.canvas.removeClass('clockpicker-canvas-out');
				self.setHand(x, y);
			}, delay);
		} else {
			this.setHand(x, y);
		}
	};

	// Set clock hand to (x, y)
	ClockPicker.prototype.setHand = function(x, y, roundBy5, dragging){
		var radian = Math.atan2(x, - y),
			isHours = this.currentView === 'hours',
			unit = Math.PI / (isHours || roundBy5 ? 6 : 30),
			z = Math.sqrt(x * x + y * y),
			options = this.options,
			inner = isHours && z < (outerRadius + innerRadius) / 2,
			radius = inner ? innerRadius : outerRadius,
			value;
			
			if (options.twelvehour) {
				radius = outerRadius;
			}

		// Radian should in range [0, 2PI]
		if (radian < 0) {
			radian = Math.PI * 2 + radian;
		}

		// Get the round value
		value = Math.round(radian / unit);

		// Get the round radian
		radian = value * unit;

		// Correct the hours or minutes
		if (options.twelvehour) {
			if (isHours) {
				if (value === 0) {
					value = 12;
				}
			} else {
				if (roundBy5) {
					value *= 5;
				}
				if (value === 60) {
					value = 0;
				}
			}
		} else {
			if (isHours) {
				if (value === 12) {
					value = 0;
				}
				value = inner ? (value === 0 ? 12 : value) : value === 0 ? 0 : value + 12;
			} else {
				if (roundBy5) {
					value *= 5;
				}
				if (value === 60) {
					value = 0;
				}
			}
		}
		
		// Once hours or minutes changed, vibrate the device
		if (this[this.currentView] !== value) {
			if (vibrate && this.options.vibrate) {
				// Do not vibrate too frequently
				if (! this.vibrateTimer) {
					navigator[vibrate](10);
					this.vibrateTimer = setTimeout($.proxy(function(){
						this.vibrateTimer = null;
					}, this), 100);
				}
			}
		}

		this[this.currentView] = value;
		this[isHours ? 'spanHours' : 'spanMinutes'].html(leadingZero(value));

		// If svg is not supported, just add an active class to the tick
		if (! svgSupported) {
			this[isHours ? 'hoursView' : 'minutesView'].find('.clockpicker-tick').each(function(){
				var tick = $(this);
				tick.toggleClass('active', value === + tick.html());
			});
			return;
		}

		// Place clock hand at the top when dragging
		if (dragging || (! isHours && value % 5)) {
			this.g.insertBefore(this.hand, this.bearing);
			this.g.insertBefore(this.bg, this.fg);
			this.bg.setAttribute('class', 'clockpicker-canvas-bg clockpicker-canvas-bg-trans');
		} else {
			// Or place it at the bottom
			this.g.insertBefore(this.hand, this.bg);
			this.g.insertBefore(this.fg, this.bg);
			this.bg.setAttribute('class', 'clockpicker-canvas-bg');
		}

		// Set clock hand and others' position
		var cx = Math.sin(radian) * radius,
			cy = - Math.cos(radian) * radius;
		this.hand.setAttribute('x2', cx);
		this.hand.setAttribute('y2', cy);
		this.bg.setAttribute('cx', cx);
		this.bg.setAttribute('cy', cy);
		this.fg.setAttribute('cx', cx);
		this.fg.setAttribute('cy', cy);
	};

	// Hours and minutes are selected
	ClockPicker.prototype.done = function () {
	    
		raiseCallback(this.options.beforeDone);
		this.hide();
		var last = this.input.prop('value'),
			value = leadingZero(this.hours) + ':' + leadingZero(this.minutes) + ' ';
		if (this.options.twelvehour) {		
			value = value + this.amOrPm;
		}
		
		this.input.prop('value', value);
		if (value !== last) {
			this.input.triggerHandler('change');
			if (! this.isInput) {
				this.element.trigger('change');
			}
		}

		if (this.options.autoclose) {
			this.input.trigger('blur');
		}

		raiseCallback(this.options.afterDone);
	};

	// Remove clockpicker from input
	ClockPicker.prototype.remove = function() {
		this.element.removeData('clockpicker');
		this.input.off('focus.clockpicker click.clockpicker');
		this.addon.off('click.clockpicker');
		if (this.isShown) {
			this.hide();
		}
		if (this.isAppended) {
			$win.off('resize.clockpicker' + this.id);
			this.popover.remove();
		}
	};

	// Extends $.fn.clockpicker
	$.fn.clockpicker = function(option){
		var args = Array.prototype.slice.call(arguments, 1);
		return this.each(function(){
			var $this = $(this),
				data = $this.data('clockpicker');
			if (! data) {
				var options = $.extend({}, ClockPicker.DEFAULTS, $this.data(), typeof option == 'object' && option);
				$this.data('clockpicker', new ClockPicker($this, options));
			} else {
				// Manual operatsions. show, hide, remove, e.g.
				if (typeof data[option] === 'function') {
					data[option].apply(data, args);
				}
			}
		});
	};
}());

function markallcompliantmsg() {
    swal({
        title: "Message",
        text: "Please make all the status compliant!",
        type: "warning",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: true,
    });
}

function swalalertLogin(title, text, type) {
    if (!title)
        title = "Message";
    debugger;
    swal({
        title: title,
        text: text,
        type: type,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Login",
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = CRxUrls.Account_Login;
        }
    });
}

function redirecttoWOassets(confirmRedirectUrl, cancelRedirectUrl) {
    debugger;
    swal({
        html: true,
        title: "Message",
        text: "<label>First close the Work order</label><br />",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "GO TO WORK ORDERS",
        cancelButtonText: "CANCEL",
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = confirmRedirectUrl;
        } else {
            window.location.href = cancelRedirectUrl;
        }
    });
}

function redirecttodefeciencies(confirmRedirectUrl) {
    debugger;
    swal({
        html: true,
        title: "Message",
        text: "<label>There are still pending deficiencies since last inspection.</label><br /><label> Please fix it before proceeding with new inspection</label>",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "GO TO DEFICIENCIES",
        cancelButtonText: "CANCEL",
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = confirmRedirectUrl;
        } else {
            // window.history.back();
            swal.close();
        }
    });
}

function redirectosetupassets(confirmRedirectUrl, cancelRedirectUrl) {
    swal({
        title: "Attach assets",
        text: "There is no asset for inspection!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "setup",
        cancelButtonText: "cancel",
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = confirmRedirectUrl;
        } else {
            window.location.href = cancelRedirectUrl;
        }
    });
}

function RedirectScheduler(confirmRedirectUrl) {
    swal({
        html: true,
        title: "Message",
        text: "<label>There are no schedules please create schedules first!!</label>",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "GO TO SCHEDULES",
        cancelButtonText: "CANCEL",
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = confirmRedirectUrl;
        } else {
            history.back();
            $("#submitButton").addClass("disable");
            $("#resumeButton").addClass("disable");
            $("#btnSubmit").addClass("disable");
            //window.location.href = cancelRedirectUrl;
        }
    });
}

function InsUploadfile(file, controlId, fileType) {
    var thisfile = $(file);
    var fileExtensionas = [];
    if (fileType === "D") {
        fileExtensionas = ['pdf', 'xlsx', 'xls', 'doc', 'docx', 'csv'];
    }
    if (fileType === "C") {
        fileExtensionas = ['png', 'jpg', 'jpeg'];
    }
    if (fileType === "F") {  // for floorPlan
        fileExtensionas = ['dwg', 'svg', 'png', 'jpg', 'jpeg', 'pdf'];
    }
    var control = file.getAttribute("tempName");
    var fileControl = file.getAttribute("filename");
    var filepath = file.getAttribute("filepath");

    if (file.files.length > 0) {
        var file = file.files[0];
        var fileName = file.name;
        var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);
        if ($.inArray(fileExtension.toLowerCase(), fileExtensionas) == -1) {
            $(thisfile).val("")
            swal("Only formats are allowed : " + fileExtensionas.join(', '));
            return false;
        } else {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {
                // console.log(reader.result);
                var array = reader.result.split(",");
                $("input[name='" + control + "'][type=hidden]").val(array[1]);
                $("input[name='" + fileControl + "'][type=hidden]").val(fileName);
                $("input[name='" + filepath + "'][type=hidden]").val("");
            };
            reader.onloadend = function () {
                if (fileType == "C") {
                    $("#imagePreview_" + controlId).attr('src', this.result);
                }
                else if (fileType == "F") { /// for floor Plan specail requirement
                    if (fileExtension == "pdf") {
                        $("#imagePreview_" + controlId).attr('src', "../dist/Images/Icons/pdf_icon.png");
                    } else {
                        $("#imagePreview_" + controlId).attr('src', this.result);
                    }
                }
                else {
                    $("#imagePreview_" + controlId).attr('src', "/dist/Images/Icons/document_blue-icon.png");
                }
                $("#imagePreview_" + controlId).removeClass("img_prev");
                $("#imagePreview_" + controlId).addClass("img_prev_upload");
            };
            reader.onerror = function (error) {
                $("#FileContent").val("");
            };
            $(".part1").hide();
            $(".part2").show();
        }
    }
}

function InsUploadAndSavefile(file, fileType, stepId) {
    var thisfile = $(file);
    var fileExtensionas = [];
    if (fileType === "D") {
        fileExtensionas = ['pdf', 'xlsx', 'xls', 'doc', 'docx', 'csv'];
    }
    if (fileType === "C") {
        fileExtensionas = ['png', 'jpg', 'jpeg'];
    }
    if (file.files.length > 0) {
        var file = file.files[0];
        const sizebytes = file.size;
        const filesize = Math.round((sizebytes / 1024));
        // The size of the file. 
        if (filesize >= 5120) {
            AlertWarningMsg("File too Big, please select a file less than 5mb");
            return false;
        }
        var fileName = file.name;
        var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);
        if ($.inArray(fileExtension.toLowerCase(), fileExtensionas) == -1) {
            $(thisfile).val("");
            AlertWarningMsg("Only " + fileExtensionas.join(', ') + " formats are allowed.");
            return false;
        } else {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {
                var inspectionId = $("#InspectionId").val();
                var activityId = $("#TinspectionActivity_0__ActivityId").val();
                var fileData = new FormData();
                fileData.append(fileName, file);
                fileData.append('StepsId', stepId);
                fileData.append('FileType', fileType);
                fileData.append('ActivityId', activityId);
                $.ajax({
                    url: "/Goal/AddInspectionFiles?inspectionId=" + inspectionId,
                    method: "POST",
                    data: fileData,
                    contentType: false,
                    dataType: 'html',
                    processData: false,
                    success: function (data) {
                        successAlert('File uploaded successfully.');
                        $("input[type='file']").val('');
                        if (stepId > 0) {
                            debugger;
                            $("#demofiles_" + stepId).children("ul").append(data);
                            var filelength = $('#demofiles_' + stepId + ' ul li').length;
                            debugger;
                            if (filelength >= 3) {
                                $("#divfiles_" + stepId).addClass('disable');
                            }
                        } else {
                            loadilsmdetailpartial();
                        }
                    }
                });
            };
            reader.onerror = function (error) {
                $("#FileContent").val("");
            };
        }
    }
}

function ImageUpload(file) {
    var control = file.getAttribute("tempName");
    var fileContent = file.getAttribute("filecontent");
    if (file.files.length > 0) {
        file = file.files[0];
        var fileName = file.name;
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            var array = reader.result.split(",");
            $("input[name='" + control + "'][type=hidden]").val(fileName);
            $("input[name='" + fileContent + "'][type=hidden]").val(array[1]);
            $("input[name='FilePath'][type=hidden]").val(null);
            $('input[name="FilePath"').attr("data-dirty-initial-value", "");
        };
        reader.onerror = function (error) {
            console.log('Error: ', error);
            $("#FileContent").val("");
        };
    }
}

function ConfirmPopUp(title, text, type, confirmButtonText, confirmRedirectUrl, cancelRedirectUrl, isCloseOnConfirm = false, isCloseOnCancel = false) {
    swal({
        title: title,
        text: text,
        type: type,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confirmButtonText,
        cancelButtonText: "Cancel",
        closeOnConfirm: isCloseOnConfirm,
        closeOnCancel: isCloseOnCancel
    }, function (isConfirm) {
        debugger;
        if (isConfirm) {
            if (!isCloseOnConfirm) {
                window.location.href = confirmRedirectUrl;
            }
        } else {
            if (cancelRedirectUrl === "-1")
                window.history.back();
            else
                window.location.href = cancelRedirectUrl;
        }
    });
}

function SwalConfirmPopUp(title, text, type, confirmButtonText, confirmRedirectUrl, cancelButtonText, cancelRedirectUrl) {

    var isCloseOnConfirm = false;
    if (confirmRedirectUrl)
        isCloseOnConfirm = true;

    var iscloseOnCancel = true;
    if (cancelRedirectUrl)
        iscloseOnCancel = false;

    swal({
        html: true,
        title: title,
        text: text,
        type: type,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText,
        closeOnConfirm: isCloseOnConfirm,
        closeOnCancel: iscloseOnCancel
    }, function (isConfirm) {
        if (isConfirm) {
            if (confirmRedirectUrl != "") {
                window.location.href = confirmRedirectUrl;
            }
            else {
                swal.close();
            }
        } else {
            if (cancelRedirectUrl) {
                window.location.href = cancelRedirectUrl;
            }
        }
    });
}

function ShowConfirmMsg(title, type, text, confirmButtonText, confirmBackUrl) {
    swal({
        html: true,
        title: title,
        text: text,
        type: type,
        showCancelButton: false,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confirmButtonText,
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                window.location.href = confirmBackUrl;
            }
        });
}

function AlertSuccessMsg(sText, title) {
    if (!title)
        title = "Message";
    swal({
        title: title,
        text: sText,
        type: "success",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: true,
        timer: 3000
    });
}

function AlertWarningMsg(sText, title) {
    if (!title)
        title = "Message";
    swal({
        title: title,
        text: sText,
        type: "warning",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: true,
        timer: 3000
    });
}

function AlertInfoMsg(sText, title) {
    if (!title)
        title = "Message";

    swal({
        title: title,
        text: sText,
        type: "info",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: true,
        timer: 3000
    });
}

function swalalert(title, text, type) {
    if (!title)
        title = "Message";

    swal({
        title: title,
        text: text,
        type: type,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: true,
        timer: 3000
    });
}

function showInforMsg(title, text, confirmButtonText, redirectToUrl) {
    swal({
        html: true,
        title: title,
        text: text,
        type: "info",
        showCancelButton: false,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confirmButtonText,
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = redirectToUrl;
        }
        else {
            window.location.href = redirectToUrl;
        }
    });

}




/*!
 * Bootstrap Context Menu
 * Author: @sydcanem
 * https://github.com/sydcanem/bootstrap-contextmenu
 *
 * Inspired by Bootstrap's dropdown plugin.
 * Bootstrap (http://getbootstrap.com).
 *
 * Licensed under MIT
 * ========================================================= */

;(function($) {

	'use strict';

	/* CONTEXTMENU CLASS DEFINITION
	 * ============================ */
	var toggle = '[data-toggle="context"]';

	var ContextMenu = function (element, options) {
		this.$element = $(element);

		this.before = options.before || this.before;
		this.onItem = options.onItem || this.onItem;
		this.scopes = options.scopes || null;

		if (options.target) {
			this.$element.data('target', options.target);
		}

		this.listen();
	};

	ContextMenu.prototype = {

		constructor: ContextMenu
		,show: function(e) {

			var $menu
				, evt
				, tp
				, items
				, relatedTarget = { relatedTarget: this, target: e.currentTarget };

			if (this.isDisabled()) return;

			this.closemenu();

			if (this.before.call(this,e,$(e.currentTarget)) === false) return;

			$menu = this.getMenu();
			$menu.trigger(evt = $.Event('show.bs.context', relatedTarget));

			tp = this.getPosition(e, $menu);
			items = 'li:not(.divider)';
			$menu.attr('style', '')
				.css(tp)
				.addClass('open')
				.on('click.context.data-api', items, $.proxy(this.onItem, this, $(e.currentTarget)))
				.trigger('shown.bs.context', relatedTarget);

			// Delegating the `closemenu` only on the currently opened menu.
			// This prevents other opened menus from closing.
			$('html')
				.on('click.context.data-api', $menu.selector, $.proxy(this.closemenu, this));

			return false;
		}

		,closemenu: function(e) {
			var $menu
				, evt
				, items
				, relatedTarget;

			$menu = this.getMenu();

			if(!$menu.hasClass('open')) return;

			relatedTarget = { relatedTarget: this };
			$menu.trigger(evt = $.Event('hide.bs.context', relatedTarget));

			items = 'li:not(.divider)';
			$menu.removeClass('open')
				.off('click.context.data-api', items)
				.trigger('hidden.bs.context', relatedTarget);

			$('html')
				.off('click.context.data-api', $menu.selector);
			// Don't propagate click event so other currently
			// opened menus won't close.
			if (e) {
				e.stopPropagation();
			}
		}

		,keydown: function(e) {
			if (e.which == 27) this.closemenu(e);
		}

		,before: function(e) {
			return true;
		}

		,onItem: function(e) {
			return true;
		}

		,listen: function () {
			this.$element.on('contextmenu.context.data-api', this.scopes, $.proxy(this.show, this));
			$('html').on('click.context.data-api', $.proxy(this.closemenu, this));
			$('html').on('keydown.context.data-api', $.proxy(this.keydown, this));
		}

		,destroy: function() {
			this.$element.off('.context.data-api').removeData('context');
			$('html').off('.context.data-api');
		}

		,isDisabled: function() {
			return this.$element.hasClass('disabled') || 
					this.$element.attr('disabled');
		}

		,getMenu: function () {
			var selector = this.$element.data('target')
				, $menu;

			if (!selector) {
				selector = this.$element.attr('href');
				selector = selector && selector.replace(/.*(?=#[^\s]*$)/, ''); //strip for ie7
			}

			$menu = $(selector);

			return $menu && $menu.length ? $menu : this.$element.find(selector);
		}

		,getPosition: function(e, $menu) {
			var mouseX = e.clientX
				, mouseY = e.clientY
				, boundsX = $(window).width()
				, boundsY = $(window).height()
				, menuWidth = $menu.find('.dropdown-menu').outerWidth()
				, menuHeight = $menu.find('.dropdown-menu').outerHeight()
				, tp = {"position":"absolute","z-index":9999}
				, Y, X, parentOffset;

			if (mouseY + menuHeight > boundsY) {
				Y = {"top": mouseY - menuHeight + $(window).scrollTop()};
			} else {
				Y = {"top": mouseY + $(window).scrollTop()};
			}

			if ((mouseX + menuWidth > boundsX) && ((mouseX - menuWidth) > 0)) {
				X = {"left": mouseX - menuWidth + $(window).scrollLeft()};
			} else {
				X = {"left": mouseX + $(window).scrollLeft()};
			}

			// If context-menu's parent is positioned using absolute or relative positioning,
			// the calculated mouse position will be incorrect.
			// Adjust the position of the menu by its offset parent position.
			parentOffset = $menu.offsetParent().offset();
			X.left = X.left - parentOffset.left;
			Y.top = Y.top - parentOffset.top;
 
			return $.extend(tp, Y, X);
		}

	};

	/* CONTEXT MENU PLUGIN DEFINITION
	 * ========================== */

	$.fn.contextmenu = function (option,e) {
		return this.each(function () {
			var $this = $(this)
				, data = $this.data('context')
				, options = (typeof option == 'object') && option;

			if (!data) $this.data('context', (data = new ContextMenu($this, options)));
			if (typeof option == 'string') data[option].call(data, e);
		});
	};

	$.fn.contextmenu.Constructor = ContextMenu;

	/* APPLY TO STANDARD CONTEXT MENU ELEMENTS
	 * =================================== */

	$(document)
	   .on('contextmenu.context.data-api', function() {
			$(toggle).each(function () {
				var data = $(this).data('context');
				if (!data) return;
				data.closemenu();
			});
		})
		.on('contextmenu.context.data-api', toggle, function(e) {
			$(this).contextmenu('show', e);

			e.preventDefault();
			e.stopPropagation();
		});
		
}(jQuery));
